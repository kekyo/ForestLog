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
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ForestLog;

/// <summary>
/// ForestLog nested scope logger interface.
/// </summary>
[DebuggerStepThrough]
public readonly struct ScopedLogger : ILogger, IDisposable
{
    private readonly ILogger logger;
    private readonly LogLevels logLevel;
    private readonly string memberName;
    private readonly string filePath;
    private readonly int line;
    private readonly Stopwatch sw = new();

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">ILogger</param>
    /// <param name="logLevel">Log level</param>
    /// <param name="memberName">Method name</param>
    /// <param name="filePath">File path</param>
    /// <param name="line">Line number</param>
    /// <remarks>This is a low-level API interface.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ScopedLogger(
        ILogger logger,
        LogLevels logLevel,
        string memberName, string filePath, int line)
    {
        this.logger = logger.NewScope();
        this.logLevel = logLevel;
        this.memberName = memberName;
        this.filePath = filePath;
        this.line = line;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Enter(object?[]? arguments)
    {
        this.logger.Write(
            this.logLevel, $"Enter: Parent={logger.ScopeId}", arguments,
            this.memberName, this.filePath, this.line);
        this.sw.Start();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public async LoggerAwaitable EnterAsync(
        object?[]? arguments, CancellationToken ct)
    {
        await this.logger.WriteAsync(
            this.logLevel, $"Enter: Parent={logger.ScopeId}", arguments,
            this.memberName, this.filePath, this.line, ct);
        this.sw.Start();
    }

    /// <summary>
    /// Dispose method.
    /// </summary>
    public void Dispose()
    {
        if (this.sw.IsRunning)
        {
            this.sw.Stop();
            var elasped = this.sw.Elapsed;
            this.logger.Write(
                this.logLevel, $"Leave: Elapsed={elasped}", null,
                this.memberName, this.filePath, this.line);
        }
    }

    /// <summary>
    /// Dispose method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LoggerAwaitable LeaveAsync(CancellationToken ct)
    {
        if (this.sw.IsRunning)
        {
            this.sw.Stop();
            var elasped = this.sw.Elapsed;
            return this.logger.WriteAsync(
                this.logLevel, $"Leave: Elapsed={elasped}", null,
                this.memberName, this.filePath, this.line, ct);
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// Leave with exception.
    /// </summary>
    /// <param name="ex">Exception</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Leave(Exception ex)
    {
        if (this.sw.IsRunning)
        {
            this.sw.Stop();
            var elasped = this.sw.Elapsed;
            this.logger.Write(
                this.logLevel, $"Leave with exception: Elapsed={elasped}", CoreUtilities.ToExceptionDetailObject(ex),
                this.memberName, this.filePath, this.line);
        }
    }

    /// <summary>
    /// Leave with exception.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="ct">CancellationToken</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LoggerAwaitable LeaveAsync(Exception ex, CancellationToken ct)
    {
        if (this.sw.IsRunning)
        {
            this.sw.Stop();
            var elasped = this.sw.Elapsed;
            return this.logger.WriteAsync(
                this.logLevel, $"Leave with exception: Elapsed={elasped}", CoreUtilities.ToExceptionDetailObject(ex),
                this.memberName, this.filePath, this.line, ct);
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// For reference use only minimum output log level.
    /// </summary>
    public LogLevels MinimumOutputLogLevel
    {
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        get => this.logger.MinimumOutputLogLevel;
    }

    /// <summary>
    /// For reference use only current scope id.
    /// </summary>
    public int ScopeId
    {
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        get => this.logger.ScopeId;
    }

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
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public void Write(
        LogLevels logLevel,
        IFormattable message,
        object? additionalData,
        string memberName,
        string filePath,
        int line) =>
        this.logger.Write(logLevel, message, additionalData, memberName, filePath, line);

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
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public LoggerAwaitable WriteAsync(
        LogLevels logLevel,
        IFormattable message,
        object? additionalData,
        string memberName,
        string filePath,
        int line,
        CancellationToken ct) =>
        this.logger.WriteAsync(logLevel, message, additionalData, memberName, filePath, line, ct);

    /// <summary>
    /// Create new scope logger interface.
    /// </summary>
    /// <returns>Logger interface</returns>
    /// <remarks>This is a low-level API interface.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public ILogger NewScope() =>
        this.logger.NewScope();
}
