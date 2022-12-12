////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ForestLog.AspNet;

[DebuggerStepThrough]
public sealed class AspNetLoggerProvider :
    ILoggerProvider
{
    private readonly ILogController logController;
    private readonly string? headName;
    private readonly ConcurrentDictionary<string, AspNetLogger> loggers = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AspNetLoggerProvider(
        ILogController logController,
        string? headName = null)
    {
        this.logController = logController;
        this.headName = headName;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName) =>
        this.loggers.GetOrAdd(categoryName, categoryName =>
            new AspNetLogger(!string.IsNullOrWhiteSpace(this.headName) ?
                this.logController.CreateLogger($"{this.headName}: {categoryName}") :
                this.logController.CreateLogger(categoryName)));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
    }
}
