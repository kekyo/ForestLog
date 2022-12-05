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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ForestLog.Infrastructure;

public abstract class LogController : ILogController
{
    protected readonly LogLevels minimumLogLevel;
    private readonly Queue<WaitingLogEntry> queue = new();
    private readonly ManualResetEvent available = new(false);
    private readonly ManualResetEvent abort = new(false);

    private Thread thread;

    internal int scopeIdCount;

    //////////////////////////////////////////////////////////////////////

    protected LogController(LogLevels minimumLogLevel)
    {
        this.minimumLogLevel = minimumLogLevel;

        this.thread = new Thread(this.ThreadEntry);
        this.thread.IsBackground = true;
        this.thread.Start();
    }

    public void Dispose()
    {
        if (this.thread is { } thread)
        {
            this.thread = null!;
            
            this.abort.Set();
            thread.Join();
        }
    }

    public virtual void Suspend()
    {
    }
    public virtual void Resume()
    {
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

    protected abstract void OnAvailable(WaitingLogEntry waitingLogEntry);

    private void ThreadEntry()
    {
        var waiters = new WaitHandle[]
        {
            this.available,
            this.abort,
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

                if (this.DequeueWaitingLogEntry() is { } waitingLogEntry)
                {
                    this.OnAvailable(waitingLogEntry);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoggerCore: {ex.GetType().FullName}: {ex.Message}");
            }
        }
    }

    //////////////////////////////////////////////////////////////////////

    private void InternalWrite(WaitingLogEntry waitingLogEntry)
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

    [DebuggerStepThrough]
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

            this.InternalWrite(waitingLogEntry);
        }
    }

    [DebuggerStepThrough]
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

            this.InternalWrite(waitingLogEntry);

            return awaiter.Task;
        }
        else
        {
            return default;
        }
    }

    //////////////////////////////////////////////////////////////////////

    public abstract LoggerAwaitable<LogEntry[]> QueryLogEntriesAsync(
        Func<LogEntry, bool> predicate, CancellationToken ct);

    //////////////////////////////////////////////////////////////////////

    [DebuggerStepThrough]
    public ILogger CreateLogger() =>
        new Logger(this);
}
