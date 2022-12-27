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

/// <summary>
/// ForestLog nested scope logger type.
/// </summary>
[DebuggerStepThrough]
public readonly struct ScopedLogger : IScopedLogger
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
    /// <param name="parentLogger">ILogger</param>
    /// <param name="logLevel">Log level</param>
    /// <param name="memberName">Method name</param>
    /// <param name="filePath">File path</param>
    /// <param name="line">Line number</param>
    /// <remarks>This is a low-level API interface.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ScopedLogger(
        ILogger parentLogger,
        LogLevels logLevel,
        string memberName,
        string filePath,
        int line)
    {
        this.logger = parentLogger.NewScope();
        this.logLevel = logLevel;
        this.memberName = memberName;
        this.filePath = filePath;
        this.line = line;
    }

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Dispose method.
    /// </summary>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public void Dispose() =>
        this.Leave(null);

    /// <summary>
    /// Dispose method.
    /// </summary>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    void IDisposable.Dispose() =>
        this.Leave(null);

#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Dispose method.
    /// </summary>
    /// <remarks>This is a low-level API interface.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepperBoundary]
    public ValueTask DisposeAsync() =>
        this.LeaveAsync(null, default);

    /// <summary>
    /// Dispose method.
    /// </summary>
    /// <remarks>This is a low-level API interface.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepperBoundary]
    ValueTask IAsyncDisposable.DisposeAsync() =>
        this.LeaveAsync(null, default);
#endif

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Enter method.
    /// </summary>
    /// <param name="arguments">Arguments hint.</param>
    /// <remarks>This is a low-level API interface.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public void Enter(
        object?[]? arguments)
    {
        if (this.logLevel >= this.logger.MinimumOutputLogLevel)
        {
            this.logger.Write(
                this.logLevel, $"Enter: Parent={this.logger.ParentScopeId}", arguments,
                this.memberName, this.filePath, this.line);
            this.sw.Start();
        }
    }

    /// <summary>
    /// Enter method.
    /// </summary>
    /// <param name="arguments">Arguments hint.</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>This is a low-level API interface.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public async LoggerAwaitable EnterAsync(
        object?[]? arguments,
        CancellationToken ct)
    {
        if (this.logLevel >= this.logger.MinimumOutputLogLevel)
        {
            await this.logger.WriteAsync(
                this.logLevel, $"Enter: Parent={this.logger.ParentScopeId}", arguments,
                this.memberName, this.filePath, this.line, ct);
            this.sw.Start();
        }
    }

    /// <summary>
    /// Leave with exception.
    /// </summary>
    /// <param name="ex">Exception</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public void Leave(Exception? ex)
    {
        if (this.sw.IsRunning)
        {
            bool isRunning;
            lock (this.sw)
            {
                isRunning = this.sw.IsRunning;
                if (isRunning)
                {
                    this.sw.Stop();
                }
            }

            if (isRunning)
            {
                var elasped = this.sw.Elapsed;
                if (ex is { })
                {
                    this.logger.Write(
                        this.logLevel, ex, $"Leave with exception: Elapsed={elasped}",
                        this.memberName, this.filePath, this.line);
                }
                else
                {
                    this.logger.Write(
                        this.logLevel, $"Leave: Elapsed={elasped}", null,
                        this.memberName, this.filePath, this.line);
                }
            }
        }
    }

    /// <summary>
    /// Leave with exception.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="ct">CancellationToken</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public LoggerAwaitable LeaveAsync(Exception? ex, CancellationToken ct)
    {
        if (this.sw.IsRunning)
        {
            bool isRunning;
            lock (this.sw)
            {
                isRunning = this.sw.IsRunning;
                if (isRunning)
                {
                    this.sw.Stop();
                }
            }

            if (isRunning)
            {
                var elasped = this.sw.Elapsed;
                if (ex is { })
                {
                    return this.logger.WriteAsync(
                        this.logLevel, ex, $"Leave with exception: Elapsed={elasped}",
                        this.memberName, this.filePath, this.line, ct);
                }
                else
                {
                    return this.logger.WriteAsync(
                        this.logLevel, $"Leave: Elapsed={elasped}", null,
                        this.memberName, this.filePath, this.line, ct);
                }
            }
        }
        return default;
    }

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Get facility.
    /// </summary>
    public string Facility
    {
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        get => this.logger.Facility;
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
    /// For reference use only parent scope id.
    /// </summary>
    public int ParentScopeId
    {
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        get => this.logger.ParentScopeId;
    }

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write a log entry.
    /// </summary>
    /// <param name="logEntry">Log entry</param>
    /// <remarks>This is a low-level API interface.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public void Write(
        WaitingLogEntry logEntry) =>
        this.logger.Write(logEntry);

    /// <summary>
    /// Write a log entry.
    /// </summary>
    /// <param name="logEntry">Log entry</param>
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
        WaitingLogEntry logEntry,
        CancellationToken ct) =>
        this.logger.WriteAsync(logEntry, ct);

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

    //////////////////////////////////////////////////////////////////////

    public override string ToString() =>
        $"Scoped: Facility={this.logger.Facility}, Id={this.ScopeId}, ParentId={this.logger.ParentScopeId}";
}
