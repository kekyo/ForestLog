////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using ForestLog.Infrastructure;
using ForestLog.Tasks;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ForestLog;

public sealed class LogEntryEventArgs : EventArgs
{
    public readonly LogEntry LogEntry;

    [DebuggerStepThrough]
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public LogEntryEventArgs(LogEntry logEntry) =>
        this.LogEntry = logEntry;
}

/// <summary>
/// ForestLog controller interface.
/// </summary>
public interface ILogController : IDisposable
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    , IAsyncDisposable
#endif
{
    /// <summary>
    /// For reference use only maximum output log level.
    /// </summary>
    LogLevels MinimumOutputLogLevel { get; }

    /// <summary>
    /// For reference use only current queued entries.
    /// </summary>
    int CurrentQueuedEntries { get; }

    /// <summary>
    /// Fire when log entry arrived (wrote to file).
    /// </summary>
    event EventHandler<LogEntryEventArgs> Arrived;

    /// <summary>
    /// Suspend log controller.
    /// </summary>
    /// <remarks>Will writes queued log entries in log files and transition to susupend.</remarks>
    void Suspend();

    /// <summary>
    /// Resume log controller.
    /// </summary>
    /// <remarks>Release suspending state.</remarks>
    void Resume();

    /// <summary>
    /// Create logger interface.
    /// </summary>
    /// <param name="facility">Facility name.</param>
    /// <returns>Logger interface</returns>
    ILogger CreateLogger(string facility = "Unknown");

    /// <summary>
    /// Raw level write log entry.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void Write(
        WaitingLogEntry logEntry, string facility, int scopeId);

    /// <summary>
    /// Raw level write log entry.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    LoggerAwaitable WriteAsync(
        WaitingLogEntry logEntry, string facility, int scopeId,
        CancellationToken ct);

    /// <summary>
    /// Query log entries now.
    /// </summary>
    /// <param name="maximumLogEntries">Maximum result log entries.</param>
    /// <param name="predicate">Query predicate.</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Result log entries.</returns>
    LoggerAwaitable<LogEntry[]> QueryLogEntriesAsync(
        int maximumLogEntries,
        Func<LogEntry, bool> predicate,
        CancellationToken ct = default);
}

public static class LogControllerExtension
{
    /// <summary>
    /// Query log entries now.
    /// </summary>
    /// <param name="logController">Log controller.</param>
    /// <param name="predicate">Query predicate.</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Result log entries.</returns>
    /// <remarks>This overload can be produced 1000 log entries.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable<LogEntry[]> QueryLogEntriesAsync(
        this ILogController logController,
        Func<LogEntry, bool> predicate,
        CancellationToken ct = default) =>
        logController.QueryLogEntriesAsync(1000, predicate, ct);
}
