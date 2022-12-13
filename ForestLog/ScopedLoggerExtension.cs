////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ForestLog;

// Parameter has no matching param tag in the XML comment (but other parameters do)
#pragma warning disable CS1573

/// <summary>
/// ForestLog logger interface extension.
/// </summary>
public static class ScopedLoggerExtension
{
    /// <summary>
    /// Create logger scope.
    /// </summary>
    /// <param name="logLevel">Log level</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static ScopedLogger Scope(
        this ILogger logger,
        LogLevels logLevel,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            logLevel,
            memberName, filePath, line);
        scopedLogger.Enter(null);
        return scopedLogger;
    }

    /// <summary>
    /// Create logger scope.
    /// </summary>
    /// <param name="logLevel">Log level</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static ScopedLogger Scope(
        this ILogger logger,
        LogLevels logLevel,
        LoggerScopeArguments arguments,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            logLevel,
            memberName, filePath, line);
        scopedLogger.Enter(arguments.Arguments);
        return scopedLogger;
    }

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Create debug scoped logger.
    /// </summary>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public static ScopedLogger DebugScope(
        this ILogger logger,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Debug,
            memberName, filePath, line);
        scopedLogger.Enter(null);
        return scopedLogger;
    }

    /// <summary>
    /// Create debug scoped logger.
    /// </summary>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public static ScopedLogger DebugScope(
        this ILogger logger,
        LoggerScopeArguments arguments,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Debug,
            memberName, filePath, line);
        scopedLogger.Enter(arguments.Arguments);
        return scopedLogger;
    }

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Create trace scoped logger.
    /// </summary>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public static ScopedLogger TraceScope(
        this ILogger logger,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Trace,
            memberName, filePath, line);
        scopedLogger.Enter(null);
        return scopedLogger;
    }

    /// <summary>
    /// Create trace scoped logger.
    /// </summary>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public static ScopedLogger TraceScope(
        this ILogger logger,
        LoggerScopeArguments arguments,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Trace,
            memberName, filePath, line);
        scopedLogger.Enter(arguments.Arguments);
        return scopedLogger;
    }

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Create information scoped logger.
    /// </summary>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public static ScopedLogger InformationScope(
        this ILogger logger,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Information,
            memberName, filePath, line);
        scopedLogger.Enter(null);
        return scopedLogger;
    }

    /// <summary>
    /// Create information scoped logger.
    /// </summary>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public static ScopedLogger InformationScope(
        this ILogger logger,
        LoggerScopeArguments arguments,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Information,
            memberName, filePath, line);
        scopedLogger.Enter(arguments.Arguments);
        return scopedLogger;
    }

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Create warning scoped logger.
    /// </summary>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public static ScopedLogger WarningScope(
        this ILogger logger,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Warning,
            memberName, filePath, line);
        scopedLogger.Enter(null);
        return scopedLogger;
    }

    /// <summary>
    /// Create warning scoped logger.
    /// </summary>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public static ScopedLogger WarningScope(
        this ILogger logger,
        LoggerScopeArguments arguments,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Warning,
            memberName, filePath, line);
        scopedLogger.Enter(arguments.Arguments);
        return scopedLogger;
    }

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Create error scoped logger.
    /// </summary>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public static ScopedLogger ErrorScope(
        this ILogger logger,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Error,
            memberName, filePath, line);
        scopedLogger.Enter(null);
        return scopedLogger;
    }

    /// <summary>
    /// Create warning scoped logger.
    /// </summary>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public static ScopedLogger ErrorScope(
        this ILogger logger,
        LoggerScopeArguments arguments,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Error,
            memberName, filePath, line);
        scopedLogger.Enter(arguments.Arguments);
        return scopedLogger;
    }

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Create fatal scoped logger.
    /// </summary>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public static ScopedLogger FatalScope(
        this ILogger logger,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Fatal,
            memberName, filePath, line);
        scopedLogger.Enter(null);
        return scopedLogger;
    }

    /// <summary>
    /// Create warning scoped logger.
    /// </summary>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public static ScopedLogger FatalScope(
        this ILogger logger,
        LoggerScopeArguments arguments,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Fatal,
            memberName, filePath, line);
        scopedLogger.Enter(arguments.Arguments);
        return scopedLogger;
    }
}
