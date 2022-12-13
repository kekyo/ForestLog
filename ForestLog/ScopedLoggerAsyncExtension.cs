////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using ForestLog.Tasks;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ForestLog;

// Parameter has no matching param tag in the XML comment (but other parameters do)
#pragma warning disable CS1573

/// <summary>
/// ForestLog logger interface extension.
/// </summary>
public static class ScopedLoggerAsyncExtension
{
    /// <summary>
    /// Create scoped logger.
    /// </summary>
    /// <param name="logLevel">Log level</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public static async LoggerAwaitable<ScopedLogger> ScopeAsync(
        this ILogger logger,
        LogLevels logLevel,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            logLevel,
            memberName, filePath, line);
        await scopedLogger.EnterAsync(null, ct);
        return scopedLogger;
    }

    /// <summary>
    /// Create scoped logger.
    /// </summary>
    /// <param name="logLevel">Log level</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public static async LoggerAwaitable<ScopedLogger> ScopeAsync(
        this ILogger logger,
        LogLevels logLevel,
        LoggerScopeArguments arguments,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            logLevel,
            memberName, filePath, line);
        await scopedLogger.EnterAsync(arguments.Arguments, ct);
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
    public static async LoggerAwaitable<ScopedLogger> DebugScopeAsync(
        this ILogger logger,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Debug,
            memberName, filePath, line);
        await scopedLogger.EnterAsync(null, ct);
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
    public static async LoggerAwaitable<ScopedLogger> DebugScopeAsync(
        this ILogger logger,
        LoggerScopeArguments arguments,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Debug,
            memberName, filePath, line);
        await scopedLogger.EnterAsync(arguments.Arguments, ct);
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
    public static async LoggerAwaitable<ScopedLogger> TraceScopeAsync(
        this ILogger logger,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Trace,
            memberName, filePath, line);
        await scopedLogger.EnterAsync(null, ct);
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
    public static async LoggerAwaitable<ScopedLogger> TraceScopeAsync(
        this ILogger logger,
        LoggerScopeArguments arguments,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Trace,
            memberName, filePath, line);
        await scopedLogger.EnterAsync(arguments.Arguments, ct);
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
    public static async LoggerAwaitable<ScopedLogger> InformationScopeAsync(
        this ILogger logger,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Information,
            memberName, filePath, line);
        await scopedLogger.EnterAsync(null, ct);
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
    public static async LoggerAwaitable<ScopedLogger> InformationScopeAsync(
        this ILogger logger,
        LoggerScopeArguments arguments,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Information,
            memberName, filePath, line);
        await scopedLogger.EnterAsync(arguments.Arguments, ct);
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
    public static async LoggerAwaitable<ScopedLogger> WarningScopeAsync(
        this ILogger logger,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Warning,
            memberName, filePath, line);
        await scopedLogger.EnterAsync(null, ct);
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
    public static async LoggerAwaitable<ScopedLogger> WarningScopeAsync(
        this ILogger logger,
        LoggerScopeArguments arguments,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Warning,
            memberName, filePath, line);
        await scopedLogger.EnterAsync(arguments.Arguments, ct);
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
    public static async LoggerAwaitable<ScopedLogger> ErrorScopeAsync(
        this ILogger logger,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Error,
            memberName, filePath, line);
        await scopedLogger.EnterAsync(null, ct);
        return scopedLogger;
    }

    /// <summary>
    /// Create error scoped logger.
    /// </summary>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public static async LoggerAwaitable<ScopedLogger> ErrorScopeAsync(
        this ILogger logger,
        LoggerScopeArguments arguments,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Error,
            memberName, filePath, line);
        await scopedLogger.EnterAsync(arguments.Arguments, ct);
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
    public static async LoggerAwaitable<ScopedLogger> FatalScopeAsync(
        this ILogger logger,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Fatal,
            memberName, filePath, line);
        await scopedLogger.EnterAsync(null, ct);
        return scopedLogger;
    }

    /// <summary>
    /// Create fatal scoped logger.
    /// </summary>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public static async LoggerAwaitable<ScopedLogger> FatalScopeAsync(
        this ILogger logger,
        LoggerScopeArguments arguments,
        CancellationToken ct = default,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int line = 0)
    {
        var scopedLogger = new ScopedLogger(
            logger,
            LogLevels.Fatal,
            memberName, filePath, line);
        await scopedLogger.EnterAsync(arguments.Arguments, ct);
        return scopedLogger;
    }
}
