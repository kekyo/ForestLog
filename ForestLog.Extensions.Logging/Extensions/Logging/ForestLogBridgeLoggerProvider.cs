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

namespace ForestLog.Extensions.Logging;

[DebuggerStepThrough]
public sealed class ForestLogBridgeLoggerProvider :
    ILoggerProvider
{
    private readonly ILogController logController;
    private readonly string? headName;
    private readonly ConcurrentDictionary<string, ForestLogBridgeLogger> loggers = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ForestLogBridgeLoggerProvider(
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
        loggers.GetOrAdd(categoryName, (System.Func<string, ForestLogBridgeLogger>)(categoryName =>

/* Unmerged change from project 'ForestLog.Extensions.Logging (netstandard1.3)'
Before:
            new AspNetLogger(!string.IsNullOrWhiteSpace(headName) ?
After:
            new Logging.AspNetLogger(!string.IsNullOrWhiteSpace(headName) ?
*/

/* Unmerged change from project 'ForestLog.Extensions.Logging (netstandard1.6)'
Before:
            new AspNetLogger(!string.IsNullOrWhiteSpace(headName) ?
After:
            new Logging.AspNetLogger(!string.IsNullOrWhiteSpace(headName) ?
*/
            (ForestLogBridgeLogger)new ForestLogBridgeLogger(!string.IsNullOrWhiteSpace(headName) ?
                logController.CreateLogger($"{headName}: {categoryName}") :
                logController.CreateLogger(categoryName))));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
    }
}
