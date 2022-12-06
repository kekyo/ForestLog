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
    private readonly LoggerAsyncLock locker = new();

    //////////////////////////////////////////////////////////////////////

    public JsonLineLogController(LogLevels minimumLogLevel, string basePath) :
        base(minimumLogLevel)
    {
        this.basePath = Path.GetFullPath(basePath);
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
                    jsLogEntry.LogLevel,
                    jsLogEntry.Timestamp,
                    jsLogEntry.ScopeId,
                    jsLogEntry.Message,
                    jsLogEntry.ExceptionType,
                    jsLogEntry.ExceptionMessage,
                    jsLogEntry.AdditionalData?.ToString(),
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

    public override async Task<LogEntry[]> QueryLogEntriesAsync(
        Func<LogEntry, bool> predicate, CancellationToken ct)
    {
        // TODO: Giant lock
        using var _ = await this.locker.LockAsync(ct);

        var results = (await Utilities.WhenAll(
            Utilities.EnumerateFiles(
                this.basePath, "log*.jsonl", SearchOption.AllDirectories).
            Select(path => LoadLogEntriesFromAsync(path, predicate, ct)))).
            SelectMany(results => results).
            ToArray();

        return results;
    }

    //////////////////////////////////////////////////////////////////////

    protected override void OnAvailable(WaitingLogEntry waitingLogEntry)
    {
        // TODO: Giant lock
        using var _ = this.locker.UnsafeLock();

        var path = Path.Combine(this.basePath, "log.jsonl");

        using var fs = new FileStream(
            path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read, 65536);
        fs.Seek(0, SeekOrigin.End);

        var tw = new StreamWriter(fs, Utilities.UTF8);
        var jw = new JsonTextWriter(tw);

        do
        {
            try
            {
                var logEntry = new JsonSerializableLogEntry(
                    Guid.NewGuid(),
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

                Utilities.JsonSerializer.Serialize(jw, logEntry);
                jw.Flush();
                tw.WriteLine();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(
                    $"JsonLineLoggerCore: {ex.GetType().FullName}: {ex.Message}");
            }

            waitingLogEntry.SetCompleted();

            waitingLogEntry = this.DequeueWaitingLogEntry()!;
        } while (waitingLogEntry != null);

        tw.Flush();
    }
}
