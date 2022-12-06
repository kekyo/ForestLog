////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ForestLog.Tasks;

namespace ForestLog.Internal;

[EditorBrowsable(EditorBrowsableState.Never)]
//[DebuggerStepThrough]
public struct LoggerAwaitableMethodBuilder<T>
{
    private static readonly TaskCompletionSource<T> completed = new();

    private TaskCompletionSource<T>? tcs;
    private T value;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitableMethodBuilder<T> Create() =>
        default;

    public LoggerAwaitable<T> Task
    {
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        get
        {
            if (this.tcs == completed)
            {
                return value;
            }
            else
            {
                Interlocked.CompareExchange(ref this.tcs, new(), null);
                return this.tcs.Task;
            }
        }
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void Start<TStateMachine>(ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine =>
        stateMachine.MoveNext();

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void SetStateMachine(IAsyncStateMachine stateMachine)
    {
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void SetResult(T value)
    {
        if (this.tcs is { } tcs)
        {
            Debug.Assert(tcs != completed);
            tcs.SetResult(value);
        }
        else
        {
            this.value = value;
            this.tcs = completed;
        }
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void SetException(Exception ex)
    {
        if (this.tcs == null)
        {
            this.tcs = new();
        }

        Debug.Assert(tcs != completed);

        if (ex is OperationCanceledException)
        {
            this.tcs.SetCanceled();
        }
        else
        {
            this.tcs.SetException(ex);
        }
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine =>
        awaiter.OnCompleted(stateMachine.MoveNext);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine =>
        awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
}

//////////////////////////////////////////////////////////////////////

[EditorBrowsable(EditorBrowsableState.Never)]
//[DebuggerStepThrough]
public struct LoggerAwaitableMethodBuilder
{
    private static readonly TaskCompletionSource<bool> completed = new();

    private TaskCompletionSource<bool>? tcs;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitableMethodBuilder Create() =>
        default;

    public LoggerAwaitable Task
    {
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        get
        {
            if (this.tcs == completed)
            {
                return default;
            }
            else
            {
                Interlocked.CompareExchange(ref this.tcs, new(), null);
                return this.tcs.Task;
            }
        }
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void Start<TStateMachine>(ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine =>
        stateMachine.MoveNext();

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void SetStateMachine(IAsyncStateMachine stateMachine)
    {
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void SetResult()
    {
        if (this.tcs is { } tcs)
        {
            Debug.Assert(tcs != completed);
            tcs.SetResult(true);
        }
        else
        {
            this.tcs = completed;
        }
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void SetException(Exception ex)
    {
        if (this.tcs == null)
        {
            this.tcs = new();
        }

        Debug.Assert(tcs != completed);

        if (ex is OperationCanceledException)
        {
            this.tcs.SetCanceled();
        }
        else
        {
            this.tcs.SetException(ex);
        }
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine =>
        awaiter.OnCompleted(stateMachine.MoveNext);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine =>
        awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
}
