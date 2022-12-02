////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ForestLog;

public static class BlockScopeExtension
{
    private static void Run(
        Logger logger,
        LogLevels logLevel,
        object?[]? arguments,
        Action<Logger> scopedAction,
        string memberName,
        string filePath,
        int line)
    {
        var scopedLogger = logger.InternalNewScope();

        scopedLogger.Log(
            logLevel, $"Enter.", arguments, memberName, filePath, line);
        try
        {
            scopedAction(scopedLogger);
        }
        catch (Exception ex)
        {
            scopedLogger.Log(
                logLevel, ex, $"Leave with exception.", null, memberName, filePath, line);
            throw;
        }

        scopedLogger.Log(
            logLevel, $"Leave.", null, memberName, filePath, line);
    }

    private static T Run<T>(
        Logger logger,
        LogLevels logLevel,
        object?[]? arguments,
        Func<Logger, T> scopedAction,
        string memberName,
        string filePath,
        int line)
    {
        var scopedLogger = logger.InternalNewScope();

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
                logLevel, ex, $"Leave with exception.", null, memberName, filePath, line);
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
        this Logger logger,
        LogLevels logLevel,
        Action<Logger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, logLevel, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static T Scope<T>(
        this Logger logger,
        LogLevels logLevel,
        Func<Logger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, logLevel, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static void Scope(
        this Logger logger,
        LogLevels logLevel,
        BlockScopeArguments arguments,
        Action<Logger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, logLevel, arguments.Arguments, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static T Scope<T>(
        this Logger logger,
        LogLevels logLevel,
        BlockScopeArguments arguments,
        Func<Logger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, logLevel, arguments.Arguments, scopedAction, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void DebugScope(
        this Logger logger,
        Action<Logger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Debug, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T DebugScope<T>(
        this Logger logger,
        Func<Logger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Debug, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void DebugScope(
        this Logger logger,
        BlockScopeArguments arguments,
        Action<Logger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Debug, arguments.Arguments, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T DebugScope<T>(
        this Logger logger,
        BlockScopeArguments arguments,
        Func<Logger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Debug, arguments.Arguments, scopedAction, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void TraceScope(
        this Logger logger,
        Action<Logger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Trace, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T TraceScope<T>(
        this Logger logger,
        Func<Logger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Trace, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void TraceScope(
        this Logger logger,
        BlockScopeArguments arguments,
        Action<Logger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Trace, arguments.Arguments, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T TraceScope<T>(
        this Logger logger,
        BlockScopeArguments arguments,
        Func<Logger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Trace, arguments.Arguments, scopedAction, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void InformationScope(
        this Logger logger,
        Action<Logger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Information, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T InformationScope<T>(
        this Logger logger,
        Func<Logger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Information, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void InformationScope(
        this Logger logger,
        BlockScopeArguments arguments,
        Action<Logger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Information, arguments.Arguments, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T InformationScope<T>(
        this Logger logger,
        BlockScopeArguments arguments,
        Func<Logger, T> scopedAction,
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
        this Logger logger,
        Action<Logger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Warning, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static T WarningScope<T>(
        this Logger logger,
        Func<Logger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Warning, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static void WarningScope(
        this Logger logger,
        BlockScopeArguments arguments,
        Action<Logger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Warning, arguments.Arguments, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static T WarningScope<T>(
        this Logger logger,
        BlockScopeArguments arguments,
        Func<Logger, T> scopedAction,
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
        this Logger logger,
        Action<Logger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Error, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static T ErrorScope<T>(
        this Logger logger,
        Func<Logger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Error, null, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static void ErrorScope(
        this Logger logger,
        BlockScopeArguments arguments,
        Action<Logger> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Error, arguments.Arguments, scopedAction, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static T ErrorScope<T>(
        this Logger logger,
        BlockScopeArguments arguments,
        Func<Logger, T> scopedAction,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        Run(logger, LogLevels.Error, arguments.Arguments, scopedAction, memberName, filePath, line);
}
