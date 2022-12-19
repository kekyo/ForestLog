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
using System.Threading;
using System.Threading.Tasks;

namespace ForestLog.Infrastructure;

/// <summary>
/// Waiting log entry.
/// </summary>
public sealed class WaitingLogEntry
{
    public readonly LogLevels LogLevel;
    public readonly string MemberName;
    public readonly string FilePath;
    public readonly int Line;

    public string Facility { get; private set; } = "Unknown";
    public int ScopeId { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }
    public IFormattable Message { get; private set; }
    public object? AdditionalData { get; private set; }
    public int ManagedThreadId { get; private set; }
    public int NativeThreadId { get; private set; }
    public int TaskId { get; private set; }

    private Exception? exception;
    private TaskCompletionSource<bool>? awaiter;
    private CancellationTokenRegistration ctr;

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public WaitingLogEntry(
        LogLevels logLevel,
        IFormattable? message,
        object? additionalData,
        Exception? exception,
        string memberName,
        string filePath,
        int line)
    {
        this.LogLevel = logLevel;
        this.Message = message!;
        this.AdditionalData = additionalData;
        this.exception = exception;
        this.MemberName = memberName;
        this.FilePath = filePath;
        this.Line = line;
    }

    internal void UpdateAdditionals(
        string facility, int scopeId)
    {
        this.Facility = facility;
        this.ScopeId = scopeId;
        this.Timestamp = DateTimeOffset.Now;
        this.ManagedThreadId = Thread.CurrentThread.ManagedThreadId;
        this.NativeThreadId = CoreUtilities.NativeThreadId;
        this.TaskId = Task.CurrentId ?? -1;

        if (this.exception != null)
        {
            this.AdditionalData = CoreUtilities.ToExceptionDetailObject(this.exception);
            if (this.Message == null)
            {
                this.Message = CoreUtilities.FormatException(this.exception);
            }
            this.exception = null;
        }
    }

    internal Task UpdateAdditionalsAndGetTask(
        string facility, int scopeId, CancellationToken ct)
    {
        this.Facility = facility;
        this.ScopeId = scopeId;
        this.Timestamp = DateTimeOffset.Now;
        this.ManagedThreadId = Thread.CurrentThread.ManagedThreadId;
        this.NativeThreadId = CoreUtilities.NativeThreadId;
        this.TaskId = Task.CurrentId ?? -1;

        if (this.exception != null)
        {
            this.AdditionalData = CoreUtilities.ToExceptionDetailObject(this.exception);
            if (this.Message == null)
            {
                this.Message = CoreUtilities.FormatException(this.exception);
            }
            this.exception = null;
        }

        this.awaiter = new();
        this.ctr = ct.Register(() => this.awaiter.TrySetCanceled());
        return this.awaiter.Task;
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
