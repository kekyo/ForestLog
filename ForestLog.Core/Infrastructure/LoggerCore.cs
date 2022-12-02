////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using ForestLog.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ForestLog.Infrastructure;

public abstract class LoggerCore
{
    protected readonly LogLevels minimumLogLevel;
    private readonly Queue<WaitingLogEntry> queue = new();
    private readonly ManualResetEventSlim available = new();
    private readonly ManualResetEventSlim abort = new();
    private readonly Thread thread;

    //////////////////////////////////////////////////////////////////////

    protected LoggerCore(LogLevels minimumLogLevel)
    {
        this.minimumLogLevel = minimumLogLevel;

        this.thread = new Thread(this.ThreadEntry);
        this.thread.IsBackground = true;
        this.thread.Start();
    }

    //////////////////////////////////////////////////////////////////////

    protected WaitingLogEntry? DequeueWaitingLogEntry()
    {
        lock (this.queue)
        {
            if (this.queue.Count == 0)
            {
                this.available.Reset();
                return null;
            }
            return this.queue.Dequeue();
        }
    }

    //////////////////////////////////////////////////////////////////////

    protected abstract void OnAvailable();

    private void ThreadEntry()
    {
        var waiters = new[]
        {
            this.available.WaitHandle,
            this.abort.WaitHandle
        };

        while (true)
        {
            try
            {
                var result = WaitHandle.WaitAny(waiters);
                if (result == 1)
                {
                    break;
                }

                this.OnAvailable();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoggerCore: {ex.GetType().FullName}: {ex.Message}");
            }
        }
    }

    //////////////////////////////////////////////////////////////////////

    private void OnWrite(WaitingLogEntry waitingLogEntry)
    {
        lock (this.queue)
        {
            this.queue.Enqueue(waitingLogEntry);
            if (this.queue.Count == 1)
            {
                this.available.Set();
            }
        }
    }

    public void Write(
        LogLevels logLevel, int scopeId,
        IFormattable message, Exception? ex, object? additionalData,
        string memberName, string filePath, int line)
    {
        if (logLevel >= minimumLogLevel)
        {
            var waitingLogEntry = new WaitingLogEntry(
                logLevel, DateTimeOffset.Now, scopeId, message, ex, additionalData,
                memberName, filePath, line,
                Thread.CurrentThread.ManagedThreadId,
                Utilities.NativeThreadId,
                Task.CurrentId ?? -1,
                null, default);

            this.OnWrite(waitingLogEntry);
        }
    }

    public LoggerAwaitable WriteAsync(
        LogLevels logLevel, int scopeId,
        IFormattable message, Exception? ex, object? additionalData,
        string memberName, string filePath, int line,
        CancellationToken ct)
    {
        if (logLevel >= minimumLogLevel)
        {
            var awaiter = new TaskCompletionSource<bool>();
            var awaiterCTR = ct.Register(() => awaiter.TrySetCanceled());

            var waitingLogEntry = new WaitingLogEntry(
                logLevel, DateTimeOffset.Now, scopeId, message, ex, additionalData,
                memberName, filePath, line,
                Thread.CurrentThread.ManagedThreadId,
                Utilities.NativeThreadId,
                Task.CurrentId ?? -1,
                awaiter, ct);

            this.OnWrite(waitingLogEntry);

            return new(awaiter.Task);
        }
        else
        {
            return default;
        }
    }

    //////////////////////////////////////////////////////////////////////

    public abstract Task<LogEntry[]> QueryLogEntriesAsync(
        Func<LogEntry, bool> predicate, CancellationToken ct);

    //////////////////////////////////////////////////////////////////////

    public static Logger Create(LoggerCore core) =>
        new Logger(core);
}
