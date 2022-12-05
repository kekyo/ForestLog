////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using ForestLog.Infrastructure;
using ForestLog.Tasks;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ForestLog;

public sealed class Logger
{
    private static int scopeIdCount;

    private readonly LogController controller;
    private readonly int scopeId;

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal Logger(LogController controller)
    {
        this.controller = controller;
        this.scopeId = Interlocked.Increment(ref scopeIdCount);
    }

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal void InternalWrite(
        LogLevels logLevel,
        IFormattable message,
        Exception? ex,
        object? additionalData,
        string memberName,
        string filePath,
        int line) =>
        this.controller.Write(
            logLevel, this.scopeId,
            message, ex, additionalData,
            memberName, filePath, line);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal LoggerAwaitable InternalWriteAsync(
        LogLevels logLevel,
        IFormattable message,
        Exception? ex,
        object? additionalData,
        string memberName,
        string filePath,
        int line,
        CancellationToken ct) =>
        this.controller.WriteAsync(
            logLevel, this.scopeId,
            message, ex, additionalData,
            memberName, filePath, line,
            ct);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal Logger InternalNewScope() =>
        new Logger(this.controller);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal Task<LogEntry[]> InternalQueryLogEntriesAsync(
        Func<LogEntry, bool> predicate, CancellationToken ct) =>
        this.controller.QueryLogEntriesAsync(predicate, ct);
}
