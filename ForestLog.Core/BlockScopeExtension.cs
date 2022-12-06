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
using System.Runtime.CompilerServices;

namespace ForestLog;

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
            logLevel, $"Enter.", arguments, memberName, filePath, line);
        try
        {
            scopedAction(scopedLogger);
        }
        catch (Exception ex)
        {
            scopedLogger.Log(
                logLevel, ex, Utilities.FormatLeaveWithException(ex), null, memberName, filePath, line);
            throw;
        }

        scopedLogger.Log(
            logLevel, $"Leave.", null, memberName, filePath, line);
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
            logLevel, $"Enter.", arguments, memberName, filePath, line);

        T result;
        try
        {
            result = scopedAction(scopedLogger);
        }
        catch (Exception ex)
        {
            scopedLogger.Log(
                logLevel, ex, Utilities.FormatLeaveWithException(ex), null, memberName, filePath, line);
            throw;
        }

        scopedLogger.Log(
            logLevel, $"Leave.", result, memberName, filePath, line);

        return result;
    }

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static void Scope(
        this ILogger logger,
        LogLevels logLevel,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, logLevel, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static T Scope<T>(
        this ILogger logger,
        LogLevels logLevel,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, logLevel, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static void Scope(
        this ILogger logger,
        LogLevels logLevel,
        BlockScopeArguments arguments,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, logLevel, arguments.Arguments, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
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

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void DebugScope(
        this ILogger logger,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Debug, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T DebugScope<T>(
        this ILogger logger,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Debug, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void DebugScope(
        this ILogger logger,
        BlockScopeArguments arguments,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Debug, arguments.Arguments, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T DebugScope<T>(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Debug, arguments.Arguments, scopedAction, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void TraceScope(
        this ILogger logger,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Trace, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T TraceScope<T>(
        this ILogger logger,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Trace, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void TraceScope(
        this ILogger logger,
        BlockScopeArguments arguments,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Trace, arguments.Arguments, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T TraceScope<T>(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Trace, arguments.Arguments, scopedAction, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void InformationScope(
        this ILogger logger,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Information, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T InformationScope<T>(
        this ILogger logger,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Information, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void InformationScope(
        this ILogger logger,
        BlockScopeArguments arguments,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Information, arguments.Arguments, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T InformationScope<T>(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Information, arguments.Arguments, scopedAction, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static void WarningScope(
        this ILogger logger,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Warning, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static T WarningScope<T>(
        this ILogger logger,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Warning, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static void WarningScope(
        this ILogger logger,
        BlockScopeArguments arguments,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Warning, arguments.Arguments, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static T WarningScope<T>(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Warning, arguments.Arguments, scopedAction, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static void ErrorScope(
        this ILogger logger,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Error, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static T ErrorScope<T>(
        this ILogger logger,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Error, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static void ErrorScope(
        this ILogger logger,
        BlockScopeArguments arguments,
        Action<ILogger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Error, arguments.Arguments, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static T ErrorScope<T>(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Error, arguments.Arguments, scopedAction, memberName, filePath, line);
}
