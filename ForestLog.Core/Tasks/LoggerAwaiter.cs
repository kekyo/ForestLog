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
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ForestLog.Tasks;

[DebuggerStepThrough]
[EditorBrowsable(EditorBrowsableState.Never)]
public readonly struct LoggerAwaiter<T> : INotifyCompletion
{
    private readonly Task<T>? task;
    private readonly T value;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal LoggerAwaiter(Task<T> task)
    {
        this.task = task;
        this.value = default!;
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal LoggerAwaiter(T value)
    {
        this.task = null;        
        this.value = value;
    }

    public bool IsCompleted
    {
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        get => this.task?.IsCompleted ?? true;
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void OnCompleted(Action continuation)
    {
        if (this.task is { } task)
        {
            task.GetAwaiter().OnCompleted(continuation);
        }
        else
        {
            Utilities.CompletedTask.GetAwaiter().OnCompleted(continuation);
        }
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    void INotifyCompletion.OnCompleted(Action continuation) =>
        this.OnCompleted(continuation);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public T GetResult() =>
        this.task != null ? this.task.Result : value;
}

//////////////////////////////////////////////////////////////////////

[DebuggerStepThrough]
[EditorBrowsable(EditorBrowsableState.Never)]
public readonly struct LoggerAwaiter : INotifyCompletion
{
    private readonly Task? task;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal LoggerAwaiter(Task? task) =>
        this.task = task;

    public bool IsCompleted
    {
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        get => this.task?.IsCompleted ?? true;
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void OnCompleted(Action continuation)
    {
        if (this.task is { } task)
        {
            task.GetAwaiter().OnCompleted(continuation);
        }
        else
        {
            Utilities.CompletedTask.GetAwaiter().OnCompleted(continuation);
        }
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    void INotifyCompletion.OnCompleted(Action continuation) =>
        this.OnCompleted(continuation);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void GetResult() =>
        this.task?.Wait();
}
