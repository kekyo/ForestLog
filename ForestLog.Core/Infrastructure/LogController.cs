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

/// <summary>
/// The log controller base class.
/// </summary>
public abstract class LogController : ILogController
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    , IAsyncDisposable
#endif
{
    private readonly LogLevels minimumOutputLogLevel;
    private readonly Queue<WaitingLogEntry> queue = new();

    private readonly ManualResetEventSlim available = new();
    private readonly ManualResetEventSlim abort = new();

    private readonly ManualResetEventSlim suspending = new();
    private readonly ManualResetEventSlim suspended = new();
    private readonly ManualResetEventSlim resume = new();

    private Task worker;
    private int scopeIdCount;

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="minimumOutputLogLevel">Minimum output log level.</param>
    protected LogController(LogLevels minimumOutputLogLevel)
    {
        this.minimumOutputLogLevel = minimumOutputLogLevel;
        this.worker = Task.Factory.StartNew(
            this.WorkerEntry,
            TaskCreationOptions.LongRunning);
    }

    /// <summary>
    /// Dispose method.
    /// </summary>
    public void Dispose()
    {
        if (this.worker is { } worker)
        {
            this.worker = null!;
            
            this.abort.Set();
            worker.GetAwaiter().GetResult();
        }
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Dispose method.
    /// </summary>
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

    /// <summary>
    /// For reference use only minimum output log level.
    /// </summary>
    public LogLevels MinimumOutputLogLevel
    {
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [DebuggerStepThrough]
        get => this.minimumOutputLogLevel;
    }

    /// <summary>
    /// Create logger interface.
    /// </summary>
    /// <param name="facility">Facility name.</param>
    /// <returns>Logger interface</returns>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public ILogger CreateLogger(string facility) =>
        new Logger(this, facility);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    internal int NewScopeId() =>
        Interlocked.Increment(ref this.scopeIdCount);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// For reference use only current queued entries.
    /// </summary>
    public int CurrentQueuedEntries
    {
        get
        {
            lock (this.queue)
            {
                return this.queue.Count;
            }
        }
    }

    /// <summary>
    /// Dequeue next waiting log entry.
    /// </summary>
    /// <returns>Waiting log entry when available</returns>
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

    /// <summary>
    /// Fire when log entry arrived (wrote to file).
    /// </summary>
    public event EventHandler<LogEntryEventArgs>? Arrived;

    /// <summary>
    /// Invoke Arrived event.
    /// </summary>
    /// <param name="logEntry">Log entry</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    protected void InvokeArrived(LogEntry logEntry) =>
        this.Arrived?.Invoke(this, new(logEntry));

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Suspend log controller.
    /// </summary>
    /// <remarks>Will writes queued log entries in log files and transition to susupend.</remarks>
    public void Suspend()
    {
        Trace.WriteLine("LogController: Suspending...");

        this.resume.Reset();
        this.suspending.Set();
        this.suspended.Wait();
    }

    /// <summary>
    /// Resume log controller.
    /// </summary>
    /// <remarks>Release suspending state.</remarks>
    public void Resume()
    {
        this.suspending.Reset();
        this.resume.Set();
    }

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Called when arrived log entry.
    /// </summary>
    /// <param name="waitingLogEntry">Log entry</param>
    /// <returns></returns>
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
                if (result == 2)
                {
                    Trace.WriteLine("LogController: Aborted.");

                    break;
                }
                // Suspending
                else if (result == 1)
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

        // Makes safer deadlocking when called Suspend() after shutdown.
        this.suspended.Set();
    }

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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

    /// <summary>
    /// Raw level write log entry.
    /// </summary>
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public void Write(
        WaitingLogEntry logEntry,
        string facility,
        int scopeId)
    {
        if (logEntry.LogLevel >= this.minimumOutputLogLevel &&
            !this.suspending.IsSet)
        {
            logEntry.UpdateAdditionals(facility, scopeId);
            this.InternalWrite(logEntry);
        }
    }

    /// <summary>
    /// Raw level write log entry.
    /// </summary>
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public LoggerAwaitable WriteAsync(
        WaitingLogEntry logEntry,
        string facility,
        int scopeId,
        CancellationToken ct)
    {
        if (logEntry.LogLevel >= this.minimumOutputLogLevel &&
            !this.suspending.IsSet)
        {
            var task = logEntry.UpdateAdditionalsAndGetTask(facility, scopeId, ct);
            this.InternalWrite(logEntry);

            return task;
        }
        else
        {
            return default;
        }
    }

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Query log entries now.
    /// </summary>
    /// <param name="maximumLogEntries">Maximum result log entries.</param>
    /// <param name="predicate">Query predicate.</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Result log entries.</returns>
    public abstract LoggerAwaitable<LogEntry[]> QueryLogEntriesAsync(
        int maximumLogEntries,
        Func<LogEntry, bool> predicate,
        CancellationToken ct);
}
