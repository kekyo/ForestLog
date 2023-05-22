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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ForestLog;

// Parameter has no matching param tag in the XML comment (but other parameters do)
#pragma warning disable CS1573

/// <summary>
/// ForestLog logger scoping interface extension.
/// </summary>
[DebuggerStepThrough]
public static class ScopingAsyncExtension
{
    private static async LoggerAwaitable RunAsync(
        ILogger? logger,
        LogLevels logLevel,
        object?[]? arguments,
        Func<ILogger?, LoggerAwaitable> scopedAction,
        CancellationToken? ct,
        string memberName,
        string filePath,
        int line)
    {
        if (logger is { })
        {
            var scopedLogger = new ScopedLogger(
                logger, logLevel, memberName, filePath, line);

            if (ct.HasValue)
            {
                await scopedLogger.EnterAsync(arguments, ct.Value);
            }
            else
            {
                scopedLogger.Enter(arguments);
            }

            try
            {
                await scopedAction(scopedLogger);
            }
            catch (Exception ex)
            {
                if (ct.HasValue)
                {
                    await scopedLogger.LeaveAsync(ex, ct.Value);
                }
                else
                {
                    scopedLogger.Leave(ex);
                }
                throw;
            }

            if (ct.HasValue)
            {
                await scopedLogger.LeaveAsync(null, ct.Value);
            }
            else
            {
                scopedLogger.Leave(null);
            }
        }
        else
        {
            await scopedAction(logger);
        }
    }

    private static async LoggerAwaitable<T> RunAsync<T>(
        ILogger? logger,
        LogLevels logLevel,
        object?[]? arguments,
        Func<ILogger?, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct,
        string memberName,
        string filePath,
        int line)
    {
        if (logger is { })
        {
            var scopedLogger = new ScopedLogger(
                logger, logLevel, memberName, filePath, line);

            if (ct.HasValue)
            {
                await scopedLogger.EnterAsync(arguments, ct.Value);
            }
            else
            {
                scopedLogger.Enter(arguments);
            }

            T result;
            try
            {
                result = await scopedAction(scopedLogger);
            }
            catch (Exception ex)
            {
                if (ct.HasValue)
                {
                    await scopedLogger.LeaveAsync(ex, ct.Value);
                }
                else
                {
                    scopedLogger.Leave(ex);
                }
                throw;
            }

            if (ct.HasValue)
            {
                await scopedLogger.LeaveAsync(null, ct.Value);
            }
            else
            {
                scopedLogger.Leave(null);
            }

            return result;
        }
        else
        {
            return await scopedAction(logger);
        }
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static LoggerAwaitable RunAsync(
        ILogger? logger,
        LogLevels logLevel,
        object?[]? arguments,
        Func<LoggerAwaitable> scopedAction,
        CancellationToken? ct,
        string memberName,
        string filePath,
        int line) =>
        RunAsync(logger, logLevel, arguments, _ => scopedAction(), ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static LoggerAwaitable<T> RunAsync<T>(
        ILogger? logger,
        LogLevels logLevel,
        object?[]? arguments,
        Func<LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct,
        string memberName,
        string filePath,
        int line) =>
        RunAsync(logger, logLevel, arguments, _ => scopedAction(), ct, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write enter and leave log entries.
    /// </summary>
    /// <param name="logLevel">Log level</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable ScopeAsync(
        this ILogger? logger,
        LogLevels logLevel,
        Func<LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, logLevel, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="logLevel">Log level</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable<T> ScopeAsync<T>(
        this ILogger? logger,
        LogLevels logLevel,
        Func<LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, logLevel, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave log entries.
    /// </summary>
    /// <param name="logLevel">Log level</param>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable ScopeAsync(
        this ILogger? logger,
        LogLevels logLevel,
        LoggerScopeArguments arguments,
        Func<LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, logLevel, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="logLevel">Log level</param>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable<T> ScopeAsync<T>(
        this ILogger? logger,
        LogLevels logLevel,
        LoggerScopeArguments arguments,
        Func<LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, logLevel, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    ////////////////////////////////////

    /// <summary>
    /// Write enter and leave log entries.
    /// </summary>
    /// <param name="logLevel">Log level</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable ScopeAsync(
        this ILogger? logger,
        LogLevels logLevel,
        Func<ILogger?, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, logLevel, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="logLevel">Log level</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable<T> ScopeAsync<T>(
        this ILogger? logger,
        LogLevels logLevel,
        Func<ILogger?, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, logLevel, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave log entries.
    /// </summary>
    /// <param name="logLevel">Log level</param>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable ScopeAsync(
        this ILogger? logger,
        LogLevels logLevel,
        LoggerScopeArguments arguments,
        Func<ILogger?, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, logLevel, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="logLevel">Log level</param>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable<T> ScopeAsync<T>(
        this ILogger? logger,
        LogLevels logLevel,
        LoggerScopeArguments arguments,
        Func<ILogger?, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, logLevel, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write enter and leave debug log entries.
    /// </summary>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable DebugScopeAsync(
        this ILogger? logger,
        Func<LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Debug, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave debug log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable<T> DebugScopeAsync<T>(
        this ILogger? logger,
        Func<LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Debug, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave debug log entries.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable DebugScopeAsync(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Debug, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave debug log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable<T> DebugScopeAsync<T>(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Debug, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    ////////////////////////////////////

    /// <summary>
    /// Write enter and leave debug log entries.
    /// </summary>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable DebugScopeAsync(
        this ILogger? logger,
        Func<ILogger?, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Debug, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave debug log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable<T> DebugScopeAsync<T>(
        this ILogger? logger,
        Func<ILogger?, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Debug, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave debug log entries.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable DebugScopeAsync(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<ILogger?, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Debug, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave debug log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable<T> DebugScopeAsync<T>(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<ILogger?, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Debug, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write enter and leave trace log entries.
    /// </summary>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable TraceScopeAsync(
        this ILogger? logger,
        Func<LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Trace, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave trace log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable<T> TraceScopeAsync<T>(
        this ILogger? logger,
        Func<LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Trace, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave trace log entries.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable TraceScopeAsync(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Trace, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave trace log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable<T> TraceScopeAsync<T>(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Trace, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    ////////////////////////////////////

    /// <summary>
    /// Write enter and leave trace log entries.
    /// </summary>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable TraceScopeAsync(
        this ILogger? logger,
        Func<ILogger?, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Trace, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave trace log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable<T> TraceScopeAsync<T>(
        this ILogger? logger,
        Func<ILogger?, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Trace, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave trace log entries.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable TraceScopeAsync(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<ILogger?, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Trace, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave trace log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable<T> TraceScopeAsync<T>(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<ILogger?, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Trace, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write enter and leave information log entries.
    /// </summary>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable InformationScopeAsync(
        this ILogger? logger,
        Func<LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Information, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave information log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable<T> InformationScopeAsync<T>(
        this ILogger? logger,
        Func<LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Information, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave information log entries.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable InformationScopeAsync(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Information, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave information log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable<T> InformationScopeAsync<T>(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Information, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    ////////////////////////////////////

    /// <summary>
    /// Write enter and leave information log entries.
    /// </summary>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable InformationScopeAsync(
        this ILogger? logger,
        Func<ILogger?, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Information, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave information log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable<T> InformationScopeAsync<T>(
        this ILogger? logger,
        Func<ILogger?, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Information, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave information log entries.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable InformationScopeAsync(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<ILogger?, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Information, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave information log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable<T> InformationScopeAsync<T>(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<ILogger?, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Information, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write enter and leave warning log entries.
    /// </summary>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable WarningScopeAsync(
        this ILogger? logger,
        Func<LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Warning, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave warning log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable<T> WarningScopeAsync<T>(
        this ILogger? logger,
        Func<LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Warning, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave warning log entries.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable WarningScopeAsync(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Warning, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave warning log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable<T> WarningScopeAsync<T>(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Warning, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    ////////////////////////////////////

    /// <summary>
    /// Write enter and leave warning log entries.
    /// </summary>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable WarningScopeAsync(
        this ILogger? logger,
        Func<ILogger?, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Warning, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave warning log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable<T> WarningScopeAsync<T>(
        this ILogger? logger,
        Func<ILogger?, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Warning, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave warning log entries.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable WarningScopeAsync(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<ILogger?, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Warning, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave warning log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable<T> WarningScopeAsync<T>(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<ILogger?, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Warning, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write enter and leave error log entries.
    /// </summary>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable ErrorScopeAsync(
        this ILogger? logger,
        Func<LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Error, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave error log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable<T> ErrorScopeAsync<T>(
        this ILogger? logger,
        Func<LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Error, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave error log entries.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable ErrorScopeAsync(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Error, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave error log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable<T> ErrorScopeAsync<T>(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Error, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    ////////////////////////////////////

    /// <summary>
    /// Write enter and leave error log entries.
    /// </summary>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable ErrorScopeAsync(
        this ILogger? logger,
        Func<ILogger?, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Error, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave error log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable<T> ErrorScopeAsync<T>(
        this ILogger? logger,
        Func<ILogger?, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Error, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave error log entries.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable ErrorScopeAsync(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<ILogger?, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Error, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave error log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable<T> ErrorScopeAsync<T>(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<ILogger?, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Error, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write enter and leave fatal log entries.
    /// </summary>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable FatalScopeAsync(
        this ILogger? logger,
        Func<LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Fatal, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave fatal log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable<T> FatalScopeAsync<T>(
        this ILogger? logger,
        Func<LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Fatal, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave fatal log entries.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable FatalScopeAsync(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Fatal, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave fatal log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable<T> FatalScopeAsync<T>(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Fatal, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    ////////////////////////////////////

    /// <summary>
    /// Write enter and leave fatal log entries.
    /// </summary>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable FatalScopeAsync(
        this ILogger? logger,
        Func<ILogger?, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Fatal, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave fatal log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable<T> FatalScopeAsync<T>(
        this ILogger? logger,
        Func<ILogger?, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Fatal, null, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave fatal log entries.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable FatalScopeAsync(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<ILogger?, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Fatal, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave fatal log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return value from delegate execution</returns>
    /// <remarks>Awaited each writing log entry when CancellationToken is provided.</remarks>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable<T> FatalScopeAsync<T>(
        this ILogger? logger,
        LoggerScopeArguments arguments,
        Func<ILogger?, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Fatal, arguments.Arguments, scopedAction, ct, memberName, filePath, line);
}
