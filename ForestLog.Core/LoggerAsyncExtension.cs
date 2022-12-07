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
using ForestLog.Internal;
using ForestLog.Tasks;

namespace ForestLog;

public static class LoggerAsyncExtension
{
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable LogAsync(
        this ILogger logger,
        LogLevels logLevel,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            logLevel,
            message, null, additionalData,
            memberName, filePath, line, ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable LogAsync(
        this ILogger logger,
        LogLevels logLevel,
        Exception ex,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            logLevel,
            Utilities.FormatException(ex), ex, additionalData,
            memberName, filePath, line, ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable LogAsync(
        this ILogger logger,
        LogLevels logLevel,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            logLevel,
            message, ex, additionalData,
            memberName, filePath, line, ct);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable DebugAsync(
        this ILogger logger,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Debug,
            message, null, additionalData,
            memberName, filePath, line, ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable DebugAsync(
        this ILogger logger,
        Exception ex,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Debug,
            Utilities.FormatException(ex), ex, additionalData,
            memberName, filePath, line, ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable DebugAsync(
        this ILogger logger,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Debug,
            message, ex, additionalData,
            memberName, filePath, line, ct);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable TraceAsync(
        this ILogger logger,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Trace,
            message, null, additionalData,
            memberName, filePath, line, ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable TraceAsync(
        this ILogger logger,
        Exception ex,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Trace,
            Utilities.FormatException(ex), ex, additionalData,
            memberName, filePath, line, ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable TraceAsync(
        this ILogger logger,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Trace,
            message, ex, additionalData,
            memberName, filePath, line, ct);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable InformationAsync(
        this ILogger logger,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Information,
            message, null, additionalData,
            memberName, filePath, line, ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable InformationAsync(
        this ILogger logger,
        Exception ex,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Information,
            Utilities.FormatException(ex), ex, additionalData,
            memberName, filePath, line, ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable InformationAsync(
        this ILogger logger,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Information,
            message, ex, additionalData,
            memberName, filePath, line, ct);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable WarningAsync(
        this ILogger logger,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Warning,
            message, null, additionalData,
            memberName, filePath, line, ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable WarningAsync(
        this ILogger logger,
        Exception ex,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Warning,
            Utilities.FormatException(ex), ex, additionalData,
            memberName, filePath, line, ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable WarningAsync(
        this ILogger logger,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Warning,
            message, ex, additionalData,
            memberName, filePath, line, ct);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable ErrorAsync(
        this ILogger logger,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Error,
            message, null, additionalData,
            memberName, filePath, line, ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable ErrorAsync(
        this ILogger logger,
        Exception ex,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Error,
            Utilities.FormatException(ex), ex, additionalData,
            memberName, filePath, line, ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable ErrorAsync(
        this ILogger logger,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Error,
            message, ex, additionalData,
            memberName, filePath, line, ct);
}
