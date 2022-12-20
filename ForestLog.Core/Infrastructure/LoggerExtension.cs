////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using ForestLog.Tasks;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ForestLog.Infrastructure;

// Because F# could not find any internal marked extension method.
[EditorBrowsable(EditorBrowsableState.Never)]
[DebuggerStepThrough]
public static class LoggerExtension
{
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Write(
        this ILogger logger,
        LogLevels logLevel,
        IFormattable message,
        object? additionalData,
        string memberName,
        string filePath,
        int line)
    {
        if (logLevel >= logger.MinimumOutputLogLevel)
        {
            logger.Write(new(
                logLevel,
                message, additionalData, null,
                memberName, filePath, line));
        }
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Write(
        this ILogger logger,
        LogLevels logLevel,
        Exception ex,
        string memberName,
        string filePath,
        int line)
    {
        if (logLevel >= logger.MinimumOutputLogLevel)
        {
            logger.Write(new(
                logLevel,
                null, null, ex,
                memberName, filePath, line));
        }
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Write(
        this ILogger logger,
        LogLevels logLevel,
        Exception ex,
        IFormattable message,
        string memberName,
        string filePath,
        int line)
    {
        if (logLevel >= logger.MinimumOutputLogLevel)
        {
            logger.Write(new(
                logLevel,
                message, null, ex,
                memberName, filePath, line));
        }
    }

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static LoggerAwaitable WriteAsync(
        this ILogger logger,
        LogLevels logLevel,
        IFormattable message,
        object? additionalData,
        string memberName,
        string filePath,
        int line,
        CancellationToken ct) =>
        logLevel >= logger.MinimumOutputLogLevel ?
            logger.WriteAsync(new(
                logLevel,
                message, additionalData, null,
                memberName, filePath, line), ct) :
            default;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static LoggerAwaitable WriteAsync(
        this ILogger logger,
        LogLevels logLevel,
        Exception ex,
        string memberName,
        string filePath,
        int line,
        CancellationToken ct) =>
        logLevel >= logger.MinimumOutputLogLevel ?
            logger.WriteAsync(new(
                logLevel,
                null, null, ex,
                memberName, filePath, line), ct) :
            default;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static LoggerAwaitable WriteAsync(
        this ILogger logger,
        LogLevels logLevel,
        Exception ex,
        IFormattable message,
        string memberName,
        string filePath,
        int line,
        CancellationToken ct) =>
        logLevel >= logger.MinimumOutputLogLevel ?
            logger.WriteAsync(new(
                logLevel,
                message, null, ex,
                memberName, filePath, line), ct) :
            default;
}
