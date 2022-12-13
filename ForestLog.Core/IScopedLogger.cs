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
using System.Threading;

namespace ForestLog;

/// <summary>
/// ForestLog nested scope logger interface.
/// </summary>
public interface IScopedLogger : ILogger, IDisposable
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    , IAsyncDisposable
#endif
{
    /// <summary>
    /// Enter method.
    /// </summary>
    /// <param name="arguments">Arguments hint.</param>
    /// <remarks>This is a low-level API interface.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void Enter(
        object?[]? arguments);

    /// <summary>
    /// Enter method.
    /// </summary>
    /// <param name="arguments">Arguments hint.</param>
    /// <param name="ct">CancellationToken</param>
    /// <remarks>This is a low-level API interface.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LoggerAwaitable EnterAsync(
        object?[]? arguments,
        CancellationToken ct);

    /// <summary>
    /// Leave with exception.
    /// </summary>
    /// <param name="ex">Exception</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void Leave(Exception? ex);

    /// <summary>
    /// Leave with exception.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="ct">CancellationToken</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    LoggerAwaitable LeaveAsync(Exception? ex, CancellationToken ct);
}
