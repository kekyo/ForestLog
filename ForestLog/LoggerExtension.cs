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
public static class LoggerExtension
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
    [DebuggerStepThrough]
    public static void Log(
        this ILogger logger,
        LogLevels logLevel,
        IFormattable message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(logLevel, message, null, additionalData, memberName, filePath, line);

    /// <summary>
    /// Write a log entry.
    /// </summary>
    /// <param name="logLevel">Log level</param>
    /// <param name="ex">Exception</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static void Log(
        this ILogger logger,
        LogLevels logLevel,
        Exception ex,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(logLevel, CoreUtilities.FormatException(ex), ex, additionalData, memberName, filePath, line);

    /// <summary>
    /// Write a log entry.
    /// </summary>
    /// <param name="logLevel">Log level</param>
    /// <param name="ex">Exception</param>
    /// <param name="message">Message (Mostly string interpolation)</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static void Log(
        this ILogger logger,
        LogLevels logLevel,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(logLevel, message, ex, additionalData, memberName, filePath, line);

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
    public static void Debug(
        this ILogger logger,
        IFormattable message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(LogLevels.Debug, message, null, additionalData, memberName, filePath, line);

    /// <summary>
    /// Write a debug log entry.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static void Debug(
        this ILogger logger,
        Exception ex,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(LogLevels.Debug, CoreUtilities.FormatException(ex), ex, additionalData, memberName, filePath, line);

    /// <summary>
    /// Write a debug log entry.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="message">Message (Mostly string interpolation)</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static void Debug(
        this ILogger logger,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(LogLevels.Debug, message, ex, additionalData, memberName, filePath, line);

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
    public static void Trace(
        this ILogger logger,
        IFormattable message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(LogLevels.Trace, message, null, additionalData, memberName, filePath, line);

    /// <summary>
    /// Write a trace log entry.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static void Trace(
        this ILogger logger,
        Exception ex,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(LogLevels.Trace, CoreUtilities.FormatException(ex), ex, additionalData, memberName, filePath, line);

    /// <summary>
    /// Write a trace log entry.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="message">Message (Mostly string interpolation)</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static void Trace(
        this ILogger logger,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(LogLevels.Trace, message, ex, additionalData, memberName, filePath, line);

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
    public static void Information(
        this ILogger logger,
        IFormattable message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(LogLevels.Information, message, null, additionalData, memberName, filePath, line);

    /// <summary>
    /// Write a information log entry.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static void Information(
        this ILogger logger,
        Exception ex,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(LogLevels.Information, CoreUtilities.FormatException(ex), ex, additionalData, memberName, filePath, line);

    /// <summary>
    /// Write a information log entry.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="message">Message (Mostly string interpolation)</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static void Information(
        this ILogger logger,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(LogLevels.Information, message, ex, additionalData, memberName, filePath, line);

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
    public static void Warning(
        this ILogger logger,
        IFormattable message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(LogLevels.Warning, message, null, additionalData, memberName, filePath, line);

    /// <summary>
    /// Write a warning log entry.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static void Warning(
        this ILogger logger,
        Exception ex,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(LogLevels.Warning, CoreUtilities.FormatException(ex), ex, additionalData, memberName, filePath, line);

    /// <summary>
    /// Write a warning log entry.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="message">Message (Mostly string interpolation)</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static void Warning(
        this ILogger logger,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(LogLevels.Warning, message, ex, additionalData, memberName, filePath, line);

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
    public static void Error(
        this ILogger logger,
        IFormattable message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(LogLevels.Error, message, null, additionalData, memberName, filePath, line);

    /// <summary>
    /// Write a error log entry.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static void Error(
        this ILogger logger,
        Exception ex,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(LogLevels.Error, CoreUtilities.FormatException(ex), ex, additionalData, memberName, filePath, line);

    /// <summary>
    /// Write a error log entry.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="message">Message (Mostly string interpolation)</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static void Error(
        this ILogger logger,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(LogLevels.Error, message, ex, additionalData, memberName, filePath, line);

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
    public static void Fatal(
        this ILogger logger,
        IFormattable message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(LogLevels.Fatal, message, null, additionalData, memberName, filePath, line);

    /// <summary>
    /// Write a fatal log entry.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static void Fatal(
        this ILogger logger,
        Exception ex,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(LogLevels.Fatal, CoreUtilities.FormatException(ex), ex, additionalData, memberName, filePath, line);

    /// <summary>
    /// Write a fatal log entry.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="message">Message (Mostly string interpolation)</param>
    /// <param name="additionalData">Additional data object when need to write</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public static void Fatal(
        this ILogger logger,
        Exception ex,
        IFormattable message,
        object? additionalData = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0) =>
        logger.Write(LogLevels.Fatal, message, ex, additionalData, memberName, filePath, line);
}
