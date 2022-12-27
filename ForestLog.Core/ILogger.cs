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
using System.ComponentModel;
using System.Threading;

namespace ForestLog;

/// <summary>
/// ForestLog logger interface.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Get facility.
    /// </summary>
    string Facility { get; }

    /// <summary>
    /// For reference use only minimum output log level.
    /// </summary>
    LogLevels MinimumOutputLogLevel { get; }

    /// <summary>
    /// For reference use only current scope id.
    /// </summary>
    int ScopeId { get; }

    /// <summary>
    /// For reference use only parent scope id.
    /// </summary>
    int ParentScopeId { get; }

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write a log entry.
    /// </summary>
    /// <param name="logEntry">Log entry</param>
    /// <remarks>This is a low-level API interface.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void Write(
        WaitingLogEntry logEntry);

    /// <summary>
    /// Write a log entry.
    /// </summary>
    /// <param name="logEntry">Log entry</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>This is a low-level API interface.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    LoggerAwaitable WriteAsync(
        WaitingLogEntry logEntry,
        CancellationToken ct);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Create new scope logger interface.
    /// </summary>
    /// <returns>Logger interface</returns>
    /// <remarks>This is a low-level API interface.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    ILogger NewScope();
}
