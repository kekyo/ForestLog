////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using ForestLog.Handlers;
using ForestLog.Infrastructure;
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
[DebuggerStepThrough]
public static class LoggerHandlerExtension
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
    [EditorBrowsable(EditorBrowsableState.Advanced)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public static void Log(
        this ILogger logger,
        LogLevels logLevel,
        [InterpolatedStringHandlerArgument("logger", "logLevel")] LoggerInterpolatedStringHandler message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(
            logLevel,
            message, additionalData,
            memberName, filePath, line);

    /// <summary>
    /// Write a log entry.
    /// </summary>
    /// <param name="logLevel">Log level</param>
    /// <param name="ex">Exception</param>
    /// <param name="message">Message (Mostly string interpolation)</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public static void Log(
        this ILogger logger,
        LogLevels logLevel,
        Exception ex,
        [InterpolatedStringHandlerArgument("logger", "logLevel")] LoggerInterpolatedStringHandler message,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(
            logLevel,
            ex, message,
            memberName, filePath, line);

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
    public static void Debug(
        this ILogger logger,
        [InterpolatedStringHandlerArgument("logger")] DebugInterpolatedStringHandler message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(
            LogLevels.Debug,
            message, additionalData,
            memberName, filePath, line);

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
    public static void Debug(
        this ILogger logger,
        Exception ex,
        [InterpolatedStringHandlerArgument("logger")] DebugInterpolatedStringHandler message,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(
            LogLevels.Debug,
            ex, message,
            memberName, filePath, line);

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
    public static void Trace(
        this ILogger logger,
        [InterpolatedStringHandlerArgument("logger")] TraceInterpolatedStringHandler message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(
            LogLevels.Trace,
            message, additionalData,
            memberName, filePath, line);

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
    public static void Trace(
        this ILogger logger,
        Exception ex,
        [InterpolatedStringHandlerArgument("logger")] TraceInterpolatedStringHandler message,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(
            LogLevels.Trace,
            ex, message,
            memberName, filePath, line);

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
    public static void Information(
        this ILogger logger,
        [InterpolatedStringHandlerArgument("logger")] InformationInterpolatedStringHandler message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(
            LogLevels.Information, 
            message, additionalData,
            memberName, filePath, line);

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
    public static void Information(
        this ILogger logger,
        Exception ex,
        [InterpolatedStringHandlerArgument("logger")] InformationInterpolatedStringHandler message,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(
            LogLevels.Information,
            ex, message,
            memberName, filePath, line);

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
    public static void Warning(
        this ILogger logger,
        [InterpolatedStringHandlerArgument("logger")] WarningInterpolatedStringHandler message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(
            LogLevels.Warning,
            message, additionalData,
            memberName, filePath, line);

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
    public static void Warning(
        this ILogger logger,
        Exception ex,
        [InterpolatedStringHandlerArgument("logger")] WarningInterpolatedStringHandler message,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(
            LogLevels.Warning,
            ex, message,
            memberName, filePath, line);

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
    public static void Error(
        this ILogger logger,
        [InterpolatedStringHandlerArgument("logger")] ErrorInterpolatedStringHandler message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(
            LogLevels.Error,
            message, additionalData,
            memberName, filePath, line);

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
    public static void Error(
        this ILogger logger,
        Exception ex,
        [InterpolatedStringHandlerArgument("logger")] ErrorInterpolatedStringHandler message,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(
            LogLevels.Error,
            ex, message,
            memberName, filePath, line);

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
    public static void Fatal(
        this ILogger logger,
        [InterpolatedStringHandlerArgument("logger")] FatalInterpolatedStringHandler message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(
            LogLevels.Fatal,
            message, additionalData,
            memberName, filePath, line);

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
    public static void Fatal(
        this ILogger logger,
        Exception ex,
        [InterpolatedStringHandlerArgument("logger")] FatalInterpolatedStringHandler message,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(
            LogLevels.Fatal,
            ex, message,
            memberName, filePath, line);
}
