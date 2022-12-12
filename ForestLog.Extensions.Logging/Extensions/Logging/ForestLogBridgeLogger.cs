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
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ForestLog.Extensions.Logging;

[DebuggerStepThrough]
public sealed class ForestLogBridgeLogger :
    Microsoft.Extensions.Logging.ILogger
{
    private readonly Stack<ILogger> stack = new();
    private readonly Disposer disposer;

    private ILogger logger;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ForestLogBridgeLogger(ILogger logger)
    {
        this.logger = logger;
        this.disposer = new(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public IDisposable BeginScope<TState>(TState state)
    {
        var childLogger = this.logger.NewScope();
        lock (this.stack)
        {
            this.stack.Push(this.logger);
            this.logger = childLogger;
        }

        childLogger.Trace($"Enter.", state);
        return this.disposer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EndScope()
    {
        this.logger.Trace($"Leave.");

        lock (this.stack)
        {
            this.logger = this.stack.Pop();
        }
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
        private readonly ForestLogBridgeLogger parent;

        public Disposer(ForestLogBridgeLogger parent) =>
            this.parent = parent;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
        [DebuggerStepperBoundary]
#endif
        public void Dispose() =>
            this.parent.EndScope();
    }
}
