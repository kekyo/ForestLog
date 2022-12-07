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

[EditorBrowsable(EditorBrowsableState.Advanced)]
public readonly struct LoggerAwaiter<T> : ICriticalNotifyCompletion
{
    private readonly Task<T>? task;
    private readonly T value;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    internal LoggerAwaiter(Task<T>? task, T value)
    {
        this.task = task;
        this.value = value;
    }

    public bool IsCompleted
    {
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [DebuggerStepThrough]
        get => this.task?.IsCompleted ?? true;
    }

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

    public void UnsafeOnCompleted(Action continuation)
    {
        if (this.task is { } task)
        {
            task.GetAwaiter().UnsafeOnCompleted(continuation);
        }
        else
        {
            Utilities.CompletedTask.GetAwaiter().UnsafeOnCompleted(continuation);
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
    void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation) =>
        this.UnsafeOnCompleted(continuation);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public T GetResult() =>
        this.task is { } task ? task.Result : this.value;
}

//////////////////////////////////////////////////////////////////////

[EditorBrowsable(EditorBrowsableState.Advanced)]
public readonly struct LoggerAwaiter : ICriticalNotifyCompletion
{
    private readonly Task? task;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    internal LoggerAwaiter(Task? task) =>
        this.task = task;

    public bool IsCompleted
    {
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [DebuggerStepThrough]
        get => this.task?.IsCompleted ?? true;
    }

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

    public void UnsafeOnCompleted(Action continuation)
    {
        if (this.task is { } task)
        {
            task.GetAwaiter().UnsafeOnCompleted(continuation);
        }
        else
        {
            Utilities.CompletedTask.GetAwaiter().UnsafeOnCompleted(continuation);
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
    void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation) =>
        this.UnsafeOnCompleted(continuation);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [DebuggerStepThrough]
    public void GetResult() =>
        this.task?.Wait();
}
