////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using ForestLog.Tasks;

namespace ForestLog;

public static class LoggerAsyncExtension
{
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable LogAsync(
        this Logger logger,
        LogLevels logLevel,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.InternalWriteAsync(
            logLevel,
            message, null, additionalData,
            memberName, filePath, line, ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable LogAsync(
        this Logger logger,
        LogLevels logLevel,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.InternalWriteAsync(
            logLevel,
            message, ex, additionalData,
            memberName, filePath, line, ct);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable DebugAsync(
        this Logger logger,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.InternalWriteAsync(
            LogLevels.Debug,
            message, null, additionalData,
            memberName, filePath, line, ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable DebugAsync(
        this Logger logger,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.InternalWriteAsync(
            LogLevels.Debug,
            message, ex, additionalData,
            memberName, filePath, line, ct);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable TraceAsync(
        this Logger logger,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.InternalWriteAsync(
            LogLevels.Trace,
            message, null, additionalData,
            memberName, filePath, line, ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable TraceAsync(
        this Logger logger,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.InternalWriteAsync(
            LogLevels.Trace,
            message, ex, additionalData,
            memberName, filePath, line, ct);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable InformationAsync(
        this Logger logger,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.InternalWriteAsync(
            LogLevels.Information,
            message, null, additionalData,
            memberName, filePath, line, ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable InformationAsync(
        this Logger logger,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.InternalWriteAsync(
            LogLevels.Information,
            message, ex, additionalData,
            memberName, filePath, line, ct);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable WarningAsync(
        this Logger logger,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.InternalWriteAsync(
            LogLevels.Warning,
            message, null, additionalData,
            memberName, filePath, line, ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable WarningAsync(
        this Logger logger,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.InternalWriteAsync(
            LogLevels.Warning,
            message, ex, additionalData,
            memberName, filePath, line, ct);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable ErrorAsync(
        this Logger logger,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.InternalWriteAsync(
            LogLevels.Error,
            message, null, additionalData,
            memberName, filePath, line, ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable ErrorAsync(
        this Logger logger,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.InternalWriteAsync(
            LogLevels.Error,
            message, ex, additionalData,
            memberName, filePath, line, ct);
}
