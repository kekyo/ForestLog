////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using ForestLog.Internal;
using ForestLog.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForestLog.Infrastructure;

internal sealed class JsonLineLogController : LogController
{
    private readonly string basePath;
    private readonly long sizeToNextFile;
    private readonly LoggerAsyncLock locker = new();

    //////////////////////////////////////////////////////////////////////

    public JsonLineLogController(
        string basePath,
        LogLevels minimumLogLevel,
        long sizeToNextFile) :
        base(minimumLogLevel)
    {
        this.basePath = Path.GetFullPath(basePath);
        this.sizeToNextFile = sizeToNextFile;
    }

    //////////////////////////////////////////////////////////////////////

#if NET35
    private static Task<List<LogEntry>> LoadLogEntriesFromAsync(
        string path, Func<LogEntry, bool> predicate, CancellationToken ct) =>
        TaskEx.Run(() => LoadLogEntriesFrom(path, predicate, ct));

    private static List<LogEntry> LoadLogEntriesFrom(
        string path, Func<LogEntry, bool> predicate, CancellationToken ct)
#else
    private static async Task<List<LogEntry>> LoadLogEntriesFromAsync(
        string path, Func<LogEntry, bool> predicate, CancellationToken ct)
#endif
    {
        using var fs = new FileStream(
            path, FileMode.Open, FileAccess.Read, FileShare.Read, 65536, true);

        var tr = new StreamReader(fs, Utilities.UTF8);
        var results = new List<LogEntry>();

        while (true)
        {
#if NET35
            ct.ThrowIfCancellationRequested();
            var line = tr.ReadLine();
#elif NET7_0_OR_GREATER
            var line = await tr.ReadLineAsync(ct);
#else
            ct.ThrowIfCancellationRequested();
            var line = await tr.ReadLineAsync();
#endif
            if (line == null)
            {
                break;
            }
            if (Utilities.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            JsonSerializableLogEntry? jsLogEntry = null;
            try
            {
                jsLogEntry = Utilities.JsonSerializer.Deserialize<JsonSerializableLogEntry>(
                    new JsonTextReader(new StringReader(line)));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(
                    $"JsonLineLoggerCore: {ex.GetType().FullName}: {ex.Message}");
            }

            if (jsLogEntry != null)
            {
                var logEntry = new LogEntry(
                    jsLogEntry.Id,
                    jsLogEntry.Facility,
                    jsLogEntry.LogLevel,
                    jsLogEntry.Timestamp,
                    jsLogEntry.ScopeId,
                    jsLogEntry.Message,
                    jsLogEntry.ExceptionType,
                    jsLogEntry.ExceptionMessage,
                    jsLogEntry.AdditionalData,
                    jsLogEntry.MemberName,
                    jsLogEntry.FilePath,
                    jsLogEntry.Line,
                    jsLogEntry.ManagedThreadId,
                    jsLogEntry.NativeThreadId,
                    jsLogEntry.TaskId,
                    jsLogEntry.ProcessId);
                if (predicate(logEntry))
                {
                    results.Add(logEntry);
                }
            }
        }

        return results;
    }

    public override async LoggerAwaitable<LogEntry[]> QueryLogEntriesAsync(
        Func<LogEntry, bool> predicate, CancellationToken ct)
    {
        var preloadPathList =
            Utilities.EnumerateFiles(
                this.basePath, "log*.jsonl", SearchOption.AllDirectories).
            Where(path => int.TryParse(Path.GetFileNameWithoutExtension(path).Substring(3), out var _)).
            ToArray();
        
        var preloadResults = (await Utilities.WhenAll(preloadPathList.
            Select(path => LoadLogEntriesFromAsync(path, predicate, ct)))).
            SelectMany(results => results).
            ToArray();

        // TODO: Giant lock
        using var _ = await this.locker.LockAsync(ct);

        var remainsPathList = new[] { Path.Combine(this.basePath, "log.jsonl") }.
            Concat(
                Utilities.EnumerateFiles(
                    this.basePath, "log*.jsonl", SearchOption.AllDirectories).
                Where(path => int.TryParse(Path.GetFileNameWithoutExtension(path).Substring(3), out var _)).
                Except(preloadPathList)).
            ToArray();

        var remainsResults = (await Utilities.WhenAll(remainsPathList.
            Select(path => LoadLogEntriesFromAsync(path, predicate, ct)))).
            SelectMany(results => results).
            ToArray();

        return preloadResults.Concat(remainsResults).ToArray();
    }

    //////////////////////////////////////////////////////////////////////

    private static string GetCandidateBackupFilePath(string basePath)
    {
        var lastIndex = new[] { 0 }.
            Concat(
                Utilities.EnumerateFiles(
                    basePath, "log*.jsonl", SearchOption.AllDirectories).
                Select(path => int.TryParse(Path.GetFileNameWithoutExtension(path).Substring(3), out var index) ? (int?)index : null).
                Where(index => index.HasValue).
                Select(index => index!.Value)).
            Max();
        return $"log{lastIndex + 1}.jsonl";
    }

#if NET35 || NET40
    protected override Task OnAvailableAsync(WaitingLogEntry waitingLogEntry) =>
        Task.Factory.StartNew(() => this.OnAvailable(waitingLogEntry));

    private void OnAvailable(WaitingLogEntry waitingLogEntry)
#else
    protected override async Task OnAvailableAsync(WaitingLogEntry waitingLogEntry)
#endif
    {
        // TODO: Giant lock
        using var _ = this.locker.UnsafeLock();

        if (!Directory.Exists(this.basePath))
        {
            try
            {
                Directory.CreateDirectory(this.basePath);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        var path = Path.Combine(this.basePath, "log.jsonl");

        var fi = new FileInfo(path);
        if (fi.Exists && (fi.Length >= this.sizeToNextFile))
        {
            var candidatePath = Path.Combine(
                this.basePath,
                GetCandidateBackupFilePath(this.basePath));
            fi.MoveTo(candidatePath);
        }

        using var fs = new FileStream(
            path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read, 65536
#if !(NET35 || NET40)
            , true
#endif
            );
        fs.Seek(0, SeekOrigin.End);

        var tw = new StreamWriter(fs, Utilities.UTF8);
        var jw = new JsonTextWriter(tw);

#if NET35 || NET40
        tw.WriteLine();
        tw.Flush();
#else
        static async LoggerAwaitable MakeSaferAtTailAsync(TextWriter tw)
        {
            await tw.WriteLineAsync();
            await tw.FlushAsync();
        }

        var lastOffloadedTask = MakeSaferAtTailAsync(tw);
#endif
        do
        {
            JsonSerializableLogEntry? logEntry = null;
            try
            {
                logEntry = new JsonSerializableLogEntry(
                    Guid.NewGuid(),
                    waitingLogEntry.Facility,
                    waitingLogEntry.LogLevel,
                    waitingLogEntry.Timestamp,
                    waitingLogEntry.ScopeId,
                    waitingLogEntry.Message.ToString(null, CultureInfo.InvariantCulture),
                    waitingLogEntry.Exception?.GetType().FullName,
                    waitingLogEntry.Exception?.Message,
                    waitingLogEntry.AdditionalData is { } ad ? JToken.FromObject(ad) : null,
                    waitingLogEntry.MemberName,
                    waitingLogEntry.FilePath,
                    waitingLogEntry.Line,
                    waitingLogEntry.ManagedThreadId,
                    waitingLogEntry.NativeThreadId,
                    waitingLogEntry.TaskId,
                    Utilities.ProcessId);

                var jt = JToken.FromObject(logEntry, Utilities.JsonSerializer);

#if NET35 || NET40
                try
                {
                    jt.WriteTo(jw);
                    jw.Flush();
                    tw.WriteLine();
                }
                finally
                {
                    if (waitingLogEntry.IsAwaiting)
                    {
                        try
                        {
                            tw.Flush();
                        }
                        finally
                        {
                            waitingLogEntry.SetCompleted();
                        }
                    }
                }
#else
                static async LoggerAwaitable WriteLogAsync(
                    WaitingLogEntry waitingLogEntry,
                    JToken jt, JsonTextWriter jw, TextWriter tw)
                {
                    try
                    {
                        await jt.WriteToAsync(jw);
                        await jw.FlushAsync();
                        await tw.WriteLineAsync();
                    }
                    finally
                    {
                        if (waitingLogEntry.IsAwaiting)
                        {
                            try
                            {
                                await tw.FlushAsync();
                            }
                            finally
                            {
                                waitingLogEntry.SetCompleted();
                            }
                        }
                    }
                }

                await lastOffloadedTask;

                lastOffloadedTask = WriteLogAsync(
                    waitingLogEntry, jt, jw, tw);
#endif
            }
            catch (Exception ex)
            {
                Trace.WriteLine(
                    $"JsonLineLoggerCore: {ex.GetType().FullName}: {ex.Message}");
            }

            if (logEntry != null)
            {
                this.InvokeArrived(logEntry);
            }

            waitingLogEntry = this.DequeueWaitingLogEntry()!;
        } while (waitingLogEntry != null);

#if NET35 || NET40
        tw.Flush();
#else
        await lastOffloadedTask;
        await tw.FlushAsync();
#endif
    }
}
