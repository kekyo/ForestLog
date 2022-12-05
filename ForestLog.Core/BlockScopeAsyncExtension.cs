﻿////////////////////////////////////////////////////////////////////////////
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
using System.Threading;
using System.Threading.Tasks;
using ForestLog.Tasks;

namespace ForestLog;

public static class BlockScopeAsyncExtension
{
    private static async LoggerAwaitable RunAsync(
        ILogger logger,
        LogLevels logLevel,
        object?[]? arguments,
        Func<ILogger, LoggerAwaitable> scopedAction,
        CancellationToken? ct,
        string memberName,
        string filePath,
        int line)
    {
        var scopedLogger = logger.NewScope();

        if (ct.HasValue)
        {
            await scopedLogger.LogAsync(
                logLevel, $"Enter.", arguments, ct.Value, memberName, filePath, line);
        }
        else
        {
            scopedLogger.Log(
                logLevel, $"Enter.", arguments, memberName, filePath, line);
        }

        try
        {
            await scopedAction(scopedLogger);
        }
        catch (Exception ex)
        {
            if (ct.HasValue)
            {
                await scopedLogger.LogAsync(
                    logLevel, ex, $"Leave with exception.", null, ct.Value, memberName, filePath, line);
            }
            else
            {
                scopedLogger.Log(
                    logLevel, ex, $"Leave with exception.", null, memberName, filePath, line);
            }
            throw;
        }

        if (ct.HasValue)
        {
            await scopedLogger.LogAsync(
                logLevel, $"Leave.", null, ct.Value, memberName, filePath, line);
        }
        else
        {
            scopedLogger.Log(
                logLevel, $"Leave.", null, memberName, filePath, line);
        }
    }

    private static async LoggerAwaitable<T> RunAsync<T>(
        ILogger logger,
        LogLevels logLevel,
        object?[]? arguments,
        Func<ILogger, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct,
        string memberName,
        string filePath,
        int line)
    {
        var scopedLogger = logger.NewScope();

        if (ct.HasValue)
        {
            await scopedLogger.LogAsync(
                logLevel, $"Enter.", arguments, ct.Value, memberName, filePath, line);
        }
        else
        {
            scopedLogger.Log(
                logLevel, $"Enter.", arguments, memberName, filePath, line);
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
                await scopedLogger.LogAsync(
                    logLevel, ex, $"Leave with exception.", null, ct.Value, memberName, filePath, line);
            }
            else
            {
                scopedLogger.Log(
                    logLevel, ex, $"Leave with exception.", null, memberName, filePath, line);
            }
            throw;
        }

        if (ct.HasValue)
        {
            await scopedLogger.LogAsync(
                logLevel, $"Leave.", result, ct.Value, memberName, filePath, line);
        }
        else
        {
            scopedLogger.Log(
                logLevel, $"Leave.", result, memberName, filePath, line);
        }

        return result;
    }

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable ScopeAsync(
        this ILogger logger,
        LogLevels logLevel,
        Func<ILogger, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, logLevel, null, scopedAction, ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static LoggerAwaitable<T> ScopeAsync<T>(
        this ILogger logger,
        LogLevels logLevel,
        Func<ILogger, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, logLevel, null, scopedAction, ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static Task ScopeAsync(
        this ILogger logger,
        LogLevels logLevel,
        BlockScopeArguments arguments,
        Func<ILogger, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, logLevel, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static Task<T> ScopeAsync<T>(
        this ILogger logger,
        LogLevels logLevel,
        BlockScopeArguments arguments,
        Func<ILogger, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, logLevel, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task DebugScopeAsync(
        this ILogger logger,
        Func<ILogger, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Debug, null, scopedAction, ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task<T> DebugScopeAsync<T>(
        this ILogger logger,
        Func<ILogger, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Debug, null, scopedAction, ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task DebugScopeAsync(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Debug, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task<T> DebugScopeAsync<T>(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Debug, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task TraceScopeAsync(
        this ILogger logger,
        Func<ILogger, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Trace, null, scopedAction, ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task<T> TraceScopeAsync<T>(
        this ILogger logger,
        Func<ILogger, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Trace, null, scopedAction, ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task TraceScopeAsync(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Trace, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task<T> TraceScopeAsync<T>(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Trace, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task InformationScopeAsync(
        this ILogger logger,
        Func<ILogger, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Information, null, scopedAction, ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task<T> InformationScopeAsync<T>(
        this ILogger logger,
        Func<ILogger, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Information, null, scopedAction, ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task InformationScopeAsync(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Information, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task<T> InformationScopeAsync<T>(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Information, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static Task WarningScopeAsync(
        this ILogger logger,
        Func<ILogger, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Warning, null, scopedAction, ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static Task<T> WarningScopeAsync<T>(
        this ILogger logger,
        Func<ILogger, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Warning, null, scopedAction, ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static Task WarningScopeAsync(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Warning, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static Task<T> WarningScopeAsync<T>(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Warning, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static Task ErrorScopeAsync(
        this ILogger logger,
        Func<ILogger, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Error, null, scopedAction, ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static Task<T> ErrorScopeAsync<T>(
        this ILogger logger,
        Func<ILogger, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Error, null, scopedAction, ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static Task ErrorScopeAsync(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, LoggerAwaitable> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Error, arguments.Arguments, scopedAction, ct, memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static Task<T> ErrorScopeAsync<T>(
        this ILogger logger,
        BlockScopeArguments arguments,
        Func<ILogger, LoggerAwaitable<T>> scopedAction,
        CancellationToken? ct = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        RunAsync(logger, LogLevels.Error, arguments.Arguments, scopedAction, ct, memberName, filePath, line);
}
