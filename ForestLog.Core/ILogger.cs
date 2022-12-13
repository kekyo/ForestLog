////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using ForestLog.Tasks;
using System;
using System.ComponentModel;
using System.Threading;

namespace ForestLog;

/// <summary>
/// ForestLog logger interface.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// For reference use only minimum output log level.
    /// </summary>
    LogLevels MinimumOutputLogLevel { get; }

    /// <summary>
    /// For reference use only current scope id.
    /// </summary>
    int ScopeId { get; }

    /// <summary>
    /// Write a log entry.
    /// </summary>
    /// <param name="logLevel">Log level</param>
    /// <param name="message">Message (Mostly string interpolation)</param>
    /// <param name="additionalData">Additional data object when need to write</param>
    /// <param name="memberName">Member name</param>
    /// <param name="filePath">File path</param>
    /// <param name="line">File line number</param>
    /// <remarks>This is a low-level API interface.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void Write(
        LogLevels logLevel,
        IFormattable message,
        object? additionalData,
        string memberName,
        string filePath,
        int line);

    /// <summary>
    /// Write a log entry.
    /// </summary>
    /// <param name="logLevel">Log level</param>
    /// <param name="message">Message (Mostly string interpolation)</param>
    /// <param name="additionalData">Additional data object when need to write</param>
    /// <param name="memberName">Member name</param>
    /// <param name="filePath">File path</param>
    /// <param name="line">File line number</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>This is a low-level API interface.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    LoggerAwaitable WriteAsync(
        LogLevels logLevel,
        IFormattable message,
        object? additionalData,
        string memberName,
        string filePath,
        int line,
        CancellationToken ct);

    /// <summary>
    /// Create new scope logger interface.
    /// </summary>
    /// <returns>Logger interface</returns>
    /// <remarks>This is a low-level API interface.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    ILogger NewScope();
}
