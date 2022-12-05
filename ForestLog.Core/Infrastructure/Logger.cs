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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ForestLog.Infrastructure;

internal sealed class Logger : ILogger
{
    private readonly LogController controller;
    private readonly int scopeId;

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public Logger(LogController controller)
    {
        this.controller = controller;
        this.scopeId = Interlocked.Increment(
            ref this.controller.scopeIdCount);
    }

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public void Write(
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
    [DebuggerStepThrough]
    public LoggerAwaitable WriteAsync(
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
    [DebuggerStepThrough]
    public ILogger NewScope() =>
        new Logger(this.controller);
}
