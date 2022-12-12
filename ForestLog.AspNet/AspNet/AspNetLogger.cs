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
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ForestLog.AspNet;

[DebuggerStepThrough]
public sealed class AspNetLogger :
    Microsoft.Extensions.Logging.ILogger
{
    private readonly ILogger logger;
    private readonly Disposer disposer;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AspNetLogger(ILogger logger)
    {
        this.logger = logger;
        this.disposer = new(logger);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public IDisposable BeginScope<TState>(TState state)
    {
        var childLogger = this.logger.NewScope();
        childLogger.Trace($"Enter.", state);
        return this.disposer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel) =>
        Utilities.ToLogLevel(logLevel) >= this.logger.MinimumOutputLogLevel;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public void Log<TState>(
        Microsoft.Extensions.Logging.LogLevel logLevel,
        Microsoft.Extensions.Logging.EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter) =>
        this.logger.Log(
            Utilities.ToLogLevel(logLevel),
            $"{eventId}: {formatter(state, exception)}", exception);

    [DebuggerStepThrough]
    private sealed class Disposer : IDisposable
    {
        private readonly ILogger logger;

        public Disposer(ILogger logger) =>
            this.logger = logger;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
        [DebuggerStepperBoundary]
#endif
        public void Dispose() =>
            this.logger.Trace($"Leave.");
    }
}
