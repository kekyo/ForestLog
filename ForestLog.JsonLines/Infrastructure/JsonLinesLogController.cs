﻿////////////////////////////////////////////////////////////////////////////
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
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ForestLog.Infrastructure;

/// <summary>
/// ForestLog Json Lines log controller.
/// </summary>
internal sealed class JsonLinesLogController : LogController
{
    private readonly string basePath;
    private readonly long sizeToNextFile;
    private readonly int maximumLogFiles;
    private readonly LoggerAsyncLock locker = new();
    private readonly LoggerAsyncLock rotationLocker = new();

    private bool requiredMakesSafer = true;

    //////////////////////////////////////////////////////////////////////

    public JsonLinesLogController(
        string basePath,
        LogLevels minimumOutputLogLevel,
        long sizeToNextFile,
        int maximumLogFiles) :
        base(minimumOutputLogLevel)
    {
        this.basePath = Path.GetFullPath(basePath);
        this.sizeToNextFile = sizeToNextFile >= 1 ?
            sizeToNextFile :
            long.MaxValue;
        this.maximumLogFiles = maximumLogFiles >= 1 ?
            maximumLogFiles :
            int.MaxValue;
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
            if (CoreUtilities.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            LogEntry? logEntry = null;
            try
            {
                logEntry = Utilities.JsonSerializer.Deserialize<JsonSerializableLogEntry>(
                    new JsonTextReader(new StringReader(line)));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(
                    $"JsonLineLoggerCore: {ex.GetType().FullName}: {ex.Message}");
            }

            if (logEntry != null)
            {
                if (predicate(logEntry))
                {
                    results.Add(logEntry);
                }
            }
        }

        return results;
    }

    public override async LoggerAwaitable<LogEntry[]> QueryLogEntriesAsync(
        int maximumLogEntries,
        Func<LogEntry, bool> predicate,
        CancellationToken ct)
    {
        if (!Directory.Exists(this.basePath))
        {
            return CoreUtilities.Empty<LogEntry>();
        }

        using var __ = this.rotationLocker.UnsafeLock();

        var preloadPathList =
            CoreUtilities.EnumerateFiles(
                this.basePath, "log*.jsonl", SearchOption.AllDirectories).
            Where(path => int.TryParse(Path.GetFileNameWithoutExtension(path).Substring(3), out var _)).
            ToArray();

        var preloadResultsNotOrdered = (await CoreUtilities.WhenAll(preloadPathList.
            Select(path => LoadLogEntriesFromAsync(path, predicate, ct)))).
            SelectMany(results => results).
            ToArray();

        if (preloadResultsNotOrdered.Length >= maximumLogEntries)
        {
            return preloadResultsNotOrdered.
                OrderBy(logEntry => logEntry.Timestamp).
                Take(maximumLogEntries).
                ToArray();
        }

        LogEntry[] remainsResultsNotOrdered;
        using (var _ = await this.locker.LockAsync(ct))
        {
            var remainsPathList = new[] { Path.Combine(this.basePath, "log.jsonl") }.
                Concat(
                    CoreUtilities.EnumerateFiles(
                        this.basePath, "log*.jsonl", SearchOption.AllDirectories).
                    Where(path => int.TryParse(Path.GetFileNameWithoutExtension(path).Substring(3), out var _)).
                    Except(preloadPathList)).
                ToArray();

            remainsResultsNotOrdered = (await CoreUtilities.WhenAll(remainsPathList.
                Select(path => LoadLogEntriesFromAsync(path, predicate, ct)))).
                SelectMany(results => results).
                ToArray();
        }

        return preloadResultsNotOrdered.
            Concat(remainsResultsNotOrdered).
            OrderBy(logEntry => logEntry.Timestamp).
            Take(maximumLogEntries).
            ToArray();
    }

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    protected override object ToExceptionObject(Exception ex) =>
        Utilities.CreateExceptionObject(ex, new());

    private readonly struct CandidatePaths
    {
        public readonly string BackupPath;
        public readonly string[] RemovePaths;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public CandidatePaths(string backupPath, string[] removePaths)
        {
            this.BackupPath = backupPath;
            this.RemovePaths = removePaths;
        }
    }

    private static CandidatePaths GetCandidatePaths(
        string basePath, int maximumLogFiles)
    {
        var indices = CoreUtilities.EnumerateFiles(
            basePath, "log*.jsonl", SearchOption.AllDirectories).
            Select(path => int.TryParse(Path.GetFileNameWithoutExtension(path).Substring(3), out var index) ? (int?)index : null).
            Where(index => index.HasValue).
            Select(index => index!.Value).
            OrderBy(index => index).
            ToArray();

        if (indices.Length == 0)
        {
            return new(Path.Combine(basePath, "log1.jsonl"), CoreUtilities.Empty<string>());
        }

        var backupPath = Path.Combine(basePath, $"log{indices.Last() + 1}.jsonl");

        // "-1" is one of current log file (log.jsonl).
        if (indices.Length >= (maximumLogFiles - 1))
        {
            var removePaths = indices.
                Take(indices.Length - (maximumLogFiles - 1) + 1).
                Select(index => Path.Combine(basePath, $"log{index}.jsonl")).
                ToArray();
            return new(backupPath, removePaths);
        }
        else
        {
            return new(backupPath, CoreUtilities.Empty<string>());
        }
    }

#if NET35 || NET40
    protected override LoggerAwaitable OnAvailableAsync(WaitingLogEntry waitingLogEntry)
    {
        this.OnAvailable(waitingLogEntry);
        return default;
    }

    private void OnAvailable(WaitingLogEntry waitingLogEntry)
#else
    protected override async LoggerAwaitable OnAvailableAsync(WaitingLogEntry waitingLogEntry)
#endif
    {
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

        //////////////////////////////////////////////////////////////

        var path = Path.Combine(this.basePath, "log.jsonl");

        // Need to backup when file size exceeded.
        var fi = new FileInfo(path);
        if (fi.Exists && (fi.Length >= this.sizeToNextFile))
        {
#if NET35 || NET40
            using var __ = this.rotationLocker.UnsafeLock();
#else
            using var __ = await this.rotationLocker.LockAsync(default);
#endif
            var candidatePaths = GetCandidatePaths(this.basePath, this.maximumLogFiles);

            try
            {
                // Need to backup.
                if (this.maximumLogFiles >= 2)
                {
                    fi.MoveTo(candidatePaths.BackupPath);
                }
                // Need to reset.
                else
                {
                    fi.Delete();
                }

                // (Will create new file)
                this.requiredMakesSafer = false;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }

            // Need to rotate.
            foreach (var removePath in candidatePaths.RemovePaths)
            {
                try
                {
                    File.Delete(removePath);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                }
            }
        }

        //////////////////////////////////////////////////////////////

#if NET35 || NET40
        using var _ = this.locker.UnsafeLock();
#else
        using var _ = await this.locker.LockAsync(default);
#endif

        using var fs = new FileStream(
            path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read, 65536
#if !(NET35 || NET40)
            , true
#endif
            );
        fs.Seek(0, SeekOrigin.End);

        var tw = new StreamWriter(fs, Utilities.UTF8);
        // Force only LF.
        tw.NewLine = "\n";
        var jw = new JsonTextWriter(tw);

#if !(NET35 || NET40)
        LoggerAwaitable lastOffloadedTask;
#endif

        if (this.requiredMakesSafer)
        {
            this.requiredMakesSafer = false;

            // Will make safer by adding a newline into jsonl file when last output was broken.
#if NET35 || NET40
            tw.WriteLine();
            tw.Flush();
#else
            static async LoggerAwaitable MakeSaferByAddingNewLineAsync(TextWriter tw)
            {
                await tw.WriteLineAsync();
                await tw.FlushAsync();
            }

            lastOffloadedTask = MakeSaferByAddingNewLineAsync(tw);
#endif
        }
#if !(NET35 || NET40)
        else
        {
            lastOffloadedTask = default;
        }
#endif

        do
        {
            JsonSerializableLogEntry? logEntry = null;
            try
            {
                logEntry = new JsonSerializableLogEntry(waitingLogEntry);

                var jt = JToken.FromObject(logEntry, Utilities.JsonSerializer);

#if NET35 || NET40
                try
                {
                    jt.WriteTo(jw);

                    // TODO: We need to flush because will append LF after json body.
                    //   But flush method maybe "flushing" entire stream I/O.
                    jw.Flush();
                    tw.WriteLine();
                }
                catch
                {
                    if (waitingLogEntry.IsAwaiting)
                    {
                        try
                        {
                            tw.Flush();
                        }
                        catch
                        {
                            waitingLogEntry.SetCompleted();
                            throw;
                        }
                        // Avoid unwinding.
                        waitingLogEntry.SetCompleted();
                    }
                    throw;
                }
                // Avoid unwinding.
                if (waitingLogEntry.IsAwaiting)
                {
                    try
                    {
                        tw.Flush();
                    }
                    catch
                    {
                        waitingLogEntry.SetCompleted();
                        throw;
                    }
                    // Avoid unwinding.
                    waitingLogEntry.SetCompleted();
                }
#else
                static async LoggerAwaitable WriteLogAsync(
                    WaitingLogEntry waitingLogEntry,
                    JToken jt, JsonTextWriter jw, TextWriter tw)
                {
                    try
                    {
                        await jt.WriteToAsync(jw);

                        // TODO: We need to flush because will append LF after json body.
                        //   But the flush method maybe "flushing" entire stream I/O related internal buffers...
                        //   It is decreased performance.
                        await jw.FlushAsync();
                        await tw.WriteLineAsync();
                    }
                    catch
                    {
                        if (waitingLogEntry.IsAwaiting)
                        {
                            try
                            {
                                await tw.FlushAsync();
                            }
                            catch
                            {
                                waitingLogEntry.SetCompleted();
                                throw;
                            }
                            // Avoid unwinding.
                            waitingLogEntry.SetCompleted();
                        }
                        throw;
                    }
                    // Avoid unwinding.
                    if (waitingLogEntry.IsAwaiting)
                    {
                        try
                        {
                            await tw.FlushAsync();
                        }
                        catch
                        {
                            waitingLogEntry.SetCompleted();
                            throw;
                        }
                        // Avoid unwinding.
                        waitingLogEntry.SetCompleted();
                    }
                }

                try
                {
                    await lastOffloadedTask;
                }
                catch
                {
                    lastOffloadedTask = default;
                    throw;
                }

                lastOffloadedTask = WriteLogAsync(
                    waitingLogEntry, jt, jw, tw);
#endif
            }
            catch (Exception ex)
            {
                // (Will recover error line)
                this.requiredMakesSafer = true;

                Trace.WriteLine(
                    $"JsonLineLoggerCore: {ex.GetType().FullName}: {ex.Message}");
            }

            if (logEntry != null)
            {
                this.InvokeArrived(logEntry);
            }

            // Length is not contained buffered data in readers.
            // This gate will make splitting as is.
            if (fs.Length >= this.sizeToNextFile)
            {
                break;
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
