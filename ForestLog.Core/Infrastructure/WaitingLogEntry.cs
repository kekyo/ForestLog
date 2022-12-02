////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ForestLog.Infrastructure;

public sealed class WaitingLogEntry
{
    public readonly LogLevels LogLevel;
    public readonly DateTimeOffset Timestamp;
    public readonly int ScopeId;
    public readonly IFormattable Message;
    public readonly Exception? Exception;
    public readonly object? AdditionalData;
    public readonly string MemberName;
    public readonly string FilePath;
    public readonly int Line;
    public readonly int ManagedThreadId;
    public readonly int NativeThreadId;
    public readonly int TaskId;

    private TaskCompletionSource<bool>? awaiter;
    private CancellationTokenRegistration ctr;

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal WaitingLogEntry(
        LogLevels logLevel,
        DateTimeOffset timestamp,
        int scopeId,
        IFormattable message,
        Exception? exception,
        object? additionalData,
        string memberName,
        string filePath,
        int line,
        int managedThreadId,
        int nativeThreadId,
        int taskId,
        TaskCompletionSource<bool>? awaiter,
        CancellationToken ct)
    {
        this.LogLevel = logLevel;
        this.Timestamp = timestamp;
        this.ScopeId = scopeId;
        this.Message = message;
        this.Exception = exception;
        this.AdditionalData = additionalData;
        this.MemberName = memberName;
        this.FilePath = filePath;
        this.Line = line;
        this.ManagedThreadId = managedThreadId;
        this.NativeThreadId = nativeThreadId;
        this.TaskId = taskId;

        if (awaiter is { })
        {
            this.awaiter = awaiter;
            this.ctr = ct.Register(() => this.awaiter.TrySetCanceled());
        }
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void SetCompleted()
    {
        if (this.awaiter is { } awaiter)
        {
            this.awaiter = null;
            awaiter.TrySetResult(true);
            this.ctr.Dispose();
            this.ctr = default;
        }
    }

    public override string ToString() =>
        $"[{this.Timestamp:yyyy/MM/dd HH:mm:ss.fff}]: {this.LogLevel}: [{this.ScopeId}]: {this.Message}";
}
