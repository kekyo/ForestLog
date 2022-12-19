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
using ForestLog.Infrastructure;
using ForestLog.Internal;
using ForestLog.Tasks;

namespace ForestLog;

// Parameter has no matching param tag in the XML comment (but other parameters do)
#pragma warning disable CS1573

/// <summary>
/// ForestLog logger interface extension.
/// </summary>
public static class LoggerHandlerAsyncExtension
{
    /// <summary>
    /// Write a log entry.
    /// </summary>
    /// <param name="logLevel">Log level</param>
    /// <param name="message">Message (Mostly string interpolation)</param>
    /// <param name="additionalData">Additional data object when need to write</param>
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
        [InterpolatedStringHandlerArgument("logger", "logLevel")] LoggerInterpolatedStringHandler message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            logLevel,
            message, additionalData,
            memberName, filePath, line, ct);

    /// <summary>
    /// Write a log entry.
    /// </summary>
    /// <param name="logLevel">Log level</param>
    /// <param name="ex">Exception</param>
    /// <param name="message">Message (Mostly string interpolation)</param>
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
        [InterpolatedStringHandlerArgument("logger", "logLevel")] LoggerInterpolatedStringHandler message,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            logLevel,
            ex, message,
            memberName, filePath, line, ct);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write a debug log entry.
    /// </summary>
    /// <param name="message">Message (Mostly string interpolation)</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable DebugAsync(
        this ILogger logger,
        [InterpolatedStringHandlerArgument("logger")] DebugInterpolatedStringHandler message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Debug,
            message, additionalData,
            memberName, filePath, line, ct);

    /// <summary>
    /// Write a debug log entry.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="message">Message (Mostly string interpolation)</param>
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
        [InterpolatedStringHandlerArgument("logger")] DebugInterpolatedStringHandler message,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Debug,
            ex, message,
            memberName, filePath, line, ct);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write a trace log entry.
    /// </summary>
    /// <param name="message">Message (Mostly string interpolation)</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable TraceAsync(
        this ILogger logger,
        [InterpolatedStringHandlerArgument("logger")] TraceInterpolatedStringHandler message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Trace,
            message, additionalData,
            memberName, filePath, line, ct);

    /// <summary>
    /// Write a trace log entry.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="message">Message (Mostly string interpolation)</param>
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
        [InterpolatedStringHandlerArgument("logger")] DebugInterpolatedStringHandler message,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Trace,
            ex, message,
            memberName, filePath, line, ct);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write a information log entry.
    /// </summary>
    /// <param name="message">Message (Mostly string interpolation)</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable InformationAsync(
        this ILogger logger,
        [InterpolatedStringHandlerArgument("logger")] InformationInterpolatedStringHandler message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Information,
            message, additionalData,
            memberName, filePath, line, ct);

    /// <summary>
    /// Write a information log entry.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="message">Message (Mostly string interpolation)</param>
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
        [InterpolatedStringHandlerArgument("logger")] InformationInterpolatedStringHandler message,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Information,
            ex, message,
            memberName, filePath, line, ct);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write a warning log entry.
    /// </summary>
    /// <param name="message">Message (Mostly string interpolation)</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable WarningAsync(
        this ILogger logger,
        [InterpolatedStringHandlerArgument("logger")] WarningInterpolatedStringHandler message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Warning,
            message, additionalData,
            memberName, filePath, line, ct);

    /// <summary>
    /// Write a warning log entry.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="message">Message (Mostly string interpolation)</param>
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
        [InterpolatedStringHandlerArgument("logger")] WarningInterpolatedStringHandler message,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Warning,
            ex, message,
            memberName, filePath, line, ct);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write a error log entry.
    /// </summary>
    /// <param name="message">Message (Mostly string interpolation)</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable ErrorAsync(
        this ILogger logger,
        [InterpolatedStringHandlerArgument("logger")] ErrorInterpolatedStringHandler message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Error,
            message, additionalData,
            memberName, filePath, line, ct);

    /// <summary>
    /// Write a error log entry.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="message">Message (Mostly string interpolation)</param>
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
        [InterpolatedStringHandlerArgument("logger")] ErrorInterpolatedStringHandler message,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Error,
            ex, message,
            memberName, filePath, line, ct);

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Write a fatal log entry.
    /// </summary>
    /// <param name="message">Message (Mostly string interpolation)</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable FatalAsync(
        this ILogger logger,
        [InterpolatedStringHandlerArgument("logger")] FatalInterpolatedStringHandler message,
        object? additionalData = null,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Fatal,
            message, additionalData,
            memberName, filePath, line, ct);

    /// <summary>
    /// Write a fatal log entry.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="message">Message (Mostly string interpolation)</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static LoggerAwaitable FatalAsync(
        this ILogger logger,
        Exception ex,
        [InterpolatedStringHandlerArgument("logger")] FatalInterpolatedStringHandler message,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.WriteAsync(
            LogLevels.Fatal,
            ex, message,
            memberName, filePath, line, ct);
}
