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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ForestLog.Infrastructure;

public abstract class LogController : ILogController
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    , IAsyncDisposable
#endif
{
    protected readonly LogLevels minimumOutputLogLevel;
    private readonly Queue<WaitingLogEntry> queue = new();
    private readonly ManualResetEvent available = new(false);
    private readonly ManualResetEvent abort = new(false);

    private Task worker;
    internal int scopeIdCount;

    //////////////////////////////////////////////////////////////////////

    protected LogController(LogLevels minimumOutputLogLevel)
    {
        this.minimumOutputLogLevel = minimumOutputLogLevel;
        this.worker = Task.Factory.StartNew(
            this.WorkerEntry,
            TaskCreationOptions.LongRunning);
    }

    public void Dispose()
    {
        if (this.worker is { } worker)
        {
            this.worker = null!;
            
            this.abort.Set();
            worker.Wait();
        }
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public ValueTask DisposeAsync()
    {
        if (this.worker is { } worker)
        {
            this.worker = null!;

            this.abort.Set();
            return new(worker);
        }
        else
        {
            return default;
        }
    }
#endif

    //////////////////////////////////////////////////////////////////////

    public virtual void Suspend()
    {
    }
    public virtual void Resume()
    {
    }

    //////////////////////////////////////////////////////////////////////

    protected WaitingLogEntry? DequeueWaitingLogEntry()
    {
        while (true)
        {
            WaitingLogEntry logEntry;
            lock (this.queue)
            {
                if (this.queue.Count == 0)
                {
                    this.available.Reset();
                    return null;
                }
                
                logEntry = this.queue.Dequeue();
            }

            if (logEntry.LogLevel < LogLevels.Ignore)
            {
                return logEntry;
            }

            // Invalid log level (Ignore)
            if (logEntry.IsAwaiting)
            {
                logEntry.SetCompleted();
            }
        }
    }

    public event EventHandler<LogEntryEventArgs>? Arrived;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    protected void InvokeArrived(LogEntry logEntry) =>
        this.Arrived?.Invoke(this, new(logEntry));

    //////////////////////////////////////////////////////////////////////

    protected abstract Task OnAvailableAsync(
        WaitingLogEntry waitingLogEntry);

    private void WorkerEntry()
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
                    this.OnAvailableAsync(waitingLogEntry).Wait();
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

#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public void Write(
        string facility, LogLevels logLevel, int scopeId,
        IFormattable message, Exception? ex, object? additionalData,
        string memberName, string filePath, int line)
    {
        if (logLevel >= this.minimumOutputLogLevel)
        {
            var waitingLogEntry = new WaitingLogEntry(
                facility, logLevel, DateTimeOffset.Now, scopeId,
                message, ex, additionalData,
                memberName, filePath, line,
                Thread.CurrentThread.ManagedThreadId,
                CoreUtilities.NativeThreadId,
                Task.CurrentId ?? -1,
                null, default);

            this.InternalWrite(waitingLogEntry);
        }
    }

#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public LoggerAwaitable WriteAsync(
        string facility, LogLevels logLevel, int scopeId,
        IFormattable message, Exception? ex, object? additionalData,
        string memberName, string filePath, int line,
        CancellationToken ct)
    {
        if (logLevel >= this.minimumOutputLogLevel)
        {
            var awaiter = new TaskCompletionSource<bool>();

            var waitingLogEntry = new WaitingLogEntry(
                facility, logLevel, DateTimeOffset.Now, scopeId,
                message, ex, additionalData,
                memberName, filePath, line,
                Thread.CurrentThread.ManagedThreadId,
                CoreUtilities.NativeThreadId,
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
        int maximumLogEntries,
        Func<LogEntry, bool> predicate,
        CancellationToken ct);

    //////////////////////////////////////////////////////////////////////

    public ILogger CreateLogger(string facility = "Unknown") =>
        new Logger(this, facility);
}
