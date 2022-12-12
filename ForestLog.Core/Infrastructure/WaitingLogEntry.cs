////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ForestLog.Infrastructure;

public sealed class WaitingLogEntry
{
    public readonly string Facility;
    public readonly LogLevels LogLevel;
    public readonly DateTimeOffset Timestamp;
    public readonly int ScopeId;
    public readonly IFormattable Message;
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
        string facility,
        LogLevels logLevel,
        DateTimeOffset timestamp,
        int scopeId,
        IFormattable message,
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
        this.Facility = facility;
        this.LogLevel = logLevel;
        this.Timestamp = timestamp;
        this.ScopeId = scopeId;
        this.Message = message;
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

    public bool IsAwaiting
    {
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        get => this.awaiter is { };
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void SetCompleted()
    {
        Debug.Assert(this.awaiter is { });

        var awaiter = this.awaiter;
        var ctr = this.ctr;

        this.awaiter = null;
        this.ctr = default;

        // HACK: TCS will deadlock when assigned continuation is hard-blocked.
        ThreadPool.QueueUserWorkItem(_ =>
        {
            awaiter!.TrySetResult(true);
            ctr.Dispose();
        });
    }

    public override string ToString() =>
        $"[{this.Timestamp:yyyy/MM/dd HH:mm:ss.fff}]: {this.Facility}: {this.LogLevel}: [{this.ScopeId}]: {this.Message}";
}
