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

    private readonly ManualResetEventSlim available = new();
    private readonly ManualResetEventSlim abort = new();

    private readonly ManualResetEventSlim suspending = new();
    private readonly ManualResetEventSlim suspended = new();
    private readonly ManualResetEventSlim resume = new();

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

    public void Suspend()
    {
        Trace.WriteLine("LogController: Suspending...");

        this.suspending.Set();
        this.suspended.Wait();
    }

    public void Resume() =>
        this.resume.Set();

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

    protected abstract LoggerAwaitable OnAvailableAsync(
        WaitingLogEntry waitingLogEntry);

    private void WorkerEntry()
    {
        var waiters = new[]
        {
            this.available.WaitHandle,
            this.suspending.WaitHandle,
            this.abort.WaitHandle,
        };

        while (true)
        {
            try
            {
                var result = WaitHandle.WaitAny(waiters);
                
                // Aborted
                if (result == 1)
                {
                    Trace.WriteLine("LogController: Aborted.");

                    break;
                }
                // Suspending
                else if (result == 2)
                {
                    Trace.WriteLine("LogController: Suspended.");

                    this.suspended.Set();
                    try
                    {
                        // Aborted
                        if (WaitHandle.WaitAny(new[] {
                            this.abort.WaitHandle,
                            this.resume.WaitHandle }) == 0)
                        {
                            Trace.WriteLine("LogController: Aborted.");

                            break;
                        }
                    }
                    finally
                    {
                        this.suspended.Reset();
                        this.suspending.Reset();
                        this.resume.Reset();
                    }

                    Trace.WriteLine("LogController: Resumed.");
                    continue;
                }
                // Available
                else
                {
                    if (this.DequeueWaitingLogEntry() is { } waitingLogEntry)
                    {
                        this.OnAvailableAsync(waitingLogEntry).
                            GetAwaiter().
                            GetResult();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(
                    $"LogController: {ex.GetType().FullName}: {ex.Message}");
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
        if (logLevel >= this.minimumOutputLogLevel &&
            !this.suspending.IsSet)
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
        if (logLevel >= this.minimumOutputLogLevel &&
            !this.suspending.IsSet)
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

    public ILogger CreateLogger(string facility) =>
        new Logger(this, facility);
}
