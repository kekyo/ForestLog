////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using ForestLog.Internal;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ForestLog;

// Parameter has no matching param tag in the XML comment (but other parameters do)
#pragma warning disable CS1573

/// <summary>
/// ForestLog logger interface extension.
/// </summary>
public static class BlockScopeExtension
{
    private static void Run(
        ILogger logger,
        LogLevels logLevel,
        object?[]? arguments,
        Action<ILogger> scopedAction,
        string memberName,
        string filePath,
        int line)
    {
        var scopedLogger = logger.NewScope();
        scopedLogger.Log(
            logLevel, $"Enter: Parent={logger.ScopeId}",
            arguments, memberName, filePath, line);

        var sw = Stopwatch.StartNew();
        try
        {
            scopedAction(scopedLogger);
        }
        catch (Exception ex)
        {
            var elasped = sw.Elapsed;
            scopedLogger.Log(
                logLevel, ex, $"Leave with exception: Elapsed={elasped}",
                CoreUtilities.ToExceptionDetailObject(ex),
                memberName, filePath, line);
            throw;
        }

        var elasped2 = sw.Elapsed;
        scopedLogger.Log(
            logLevel, $"Leave: Elapsed={elasped2}",
            null, memberName, filePath, line);
    }

    private static T Run<T>(
        ILogger logger,
        LogLevels logLevel,
        object?[]? arguments,
        Func<ILogger, T> scopedAction,
        string memberName,
        string filePath,
        int line)
    {
        var scopedLogger = logger.NewScope();

        scopedLogger.Log(
            logLevel, $"Enter: Parent={logger.ScopeId}",
            arguments, memberName, filePath, line);

        var sw = Stopwatch.StartNew();
        T result;
        try
        {
            result = scopedAction(scopedLogger);
        }
        catch (Exception ex)
        {
            var elasped = sw.Elapsed;
            scopedLogger.Log(
                logLevel, ex, $"Leave with exception: Elapsed={elasped}",
                CoreUtilities.ToExceptionDetailObject(ex),
                memberName, filePath, line);
            throw;
        }

        var elasped2 = sw.Elapsed;
        scopedLogger.Log(
            logLevel, $"Leave: Elapsed={elasped2}",
            result, memberName, filePath, line);

        return result;
    }

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write enter and leave log entries.
    /// </summary>
    /// <param name="logLevel">Log level</param>
    /// <param name="scopedAction">Delegate to execute</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [DebuggerStepThrough]
    public static void Scope(
        this ILogger logger,
        LogLevels logLevel,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, logLevel, null, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="logLevel">Log level</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <returns>Return value from delegate execution</returns>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [DebuggerStepThrough]
    public static T Scope<T>(
        this ILogger logger,
        LogLevels logLevel,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, logLevel, null, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave log entries.
    /// </summary>
    /// <param name="logLevel">Log level</param>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [DebuggerStepThrough]
    public static void Scope(
        this ILogger logger,
        LogLevels logLevel,
        BlockScopeArguments arguments,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, logLevel, arguments.Arguments, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="logLevel">Log level</param>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <returns>Return value from delegate execution</returns>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [DebuggerStepThrough]
    public static T Scope<T>(
        this ILogger logger,
        LogLevels logLevel,
        BlockScopeArguments arguments,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, logLevel, arguments.Arguments, scopedAction, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write enter and leave debug log entries.
    /// </summary>
    /// <param name="scopedAction">Delegate to execute</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static void DebugScope(
        this ILogger logger,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Debug, null, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave debug log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <returns>Return value from delegate execution</returns>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static T DebugScope<T>(
        this ILogger logger,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Debug, null, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave debug log entries.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static void DebugScope(
        this ILogger logger,
        BlockScopeArguments arguments,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Debug, arguments.Arguments, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave debug log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <returns>Return value from delegate execution</returns>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static T DebugScope<T>(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Debug, arguments.Arguments, scopedAction, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write enter and leave trace log entries.
    /// </summary>
    /// <param name="scopedAction">Delegate to execute</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static void TraceScope(
        this ILogger logger,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Trace, null, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave trace log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <returns>Return value from delegate execution</returns>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static T TraceScope<T>(
        this ILogger logger,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Trace, null, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave trace log entries.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static void TraceScope(
        this ILogger logger,
        BlockScopeArguments arguments,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Trace, arguments.Arguments, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave trace log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <returns>Return value from delegate execution</returns>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static T TraceScope<T>(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Trace, arguments.Arguments, scopedAction, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write enter and leave information log entries.
    /// </summary>
    /// <param name="scopedAction">Delegate to execute</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static void InformationScope(
        this ILogger logger,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Information, null, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave information log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <returns>Return value from delegate execution</returns>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static T InformationScope<T>(
        this ILogger logger,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Information, null, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave information log entries.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static void InformationScope(
        this ILogger logger,
        BlockScopeArguments arguments,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Information, arguments.Arguments, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave information log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <returns>Return value from delegate execution</returns>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static T InformationScope<T>(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Information, arguments.Arguments, scopedAction, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write enter and leave warning log entries.
    /// </summary>
    /// <param name="scopedAction">Delegate to execute</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [DebuggerStepThrough]
    public static void WarningScope(
        this ILogger logger,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Warning, null, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave warning log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <returns>Return value from delegate execution</returns>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [DebuggerStepThrough]
    public static T WarningScope<T>(
        this ILogger logger,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Warning, null, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave warning log entries.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [DebuggerStepThrough]
    public static void WarningScope(
        this ILogger logger,
        BlockScopeArguments arguments,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Warning, arguments.Arguments, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave warning log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <returns>Return value from delegate execution</returns>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [DebuggerStepThrough]
    public static T WarningScope<T>(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Warning, arguments.Arguments, scopedAction, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write enter and leave error log entries.
    /// </summary>
    /// <param name="scopedAction">Delegate to execute</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [DebuggerStepThrough]
    public static void ErrorScope(
        this ILogger logger,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Error, null, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave error log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <returns>Return value from delegate execution</returns>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [DebuggerStepThrough]
    public static T ErrorScope<T>(
        this ILogger logger,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Error, null, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave error log entries.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [DebuggerStepThrough]
    public static void ErrorScope(
        this ILogger logger,
        BlockScopeArguments arguments,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Error, arguments.Arguments, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave error log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <returns>Return value from delegate execution</returns>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [DebuggerStepThrough]
    public static T ErrorScope<T>(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Error, arguments.Arguments, scopedAction, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write enter and leave fatal log entries.
    /// </summary>
    /// <param name="scopedAction">Delegate to execute</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [DebuggerStepThrough]
    public static void FatalScope(
        this ILogger logger,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Fatal, null, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave fatal log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <returns>Return value from delegate execution</returns>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [DebuggerStepThrough]
    public static T FatalScope<T>(
        this ILogger logger,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Fatal, null, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave fatal log entries.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [DebuggerStepThrough]
    public static void FatalScope(
        this ILogger logger,
        BlockScopeArguments arguments,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Fatal, arguments.Arguments, scopedAction, memberName, filePath, line);

    /// <summary>
    /// Write enter and leave fatal log entries.
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    /// <param name="arguments">Method arguments</param>
    /// <param name="scopedAction">Delegate to execute</param>
    /// <returns>Return value from delegate execution</returns>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [DebuggerStepThrough]
    public static T FatalScope<T>(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Fatal, arguments.Arguments, scopedAction, memberName, filePath, line);
}
