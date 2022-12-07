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
using System.Threading;
using System.Threading.Tasks;

namespace ForestLog.Tasks;

// Field 'LoggerAwaitableMethodBuilder<T>.value' is never assigned to, and will always have its default value
#pragma warning disable 649

[EditorBrowsable(EditorBrowsableState.Never)]
[DebuggerStepThrough]
public struct LoggerAwaitableMethodBuilder<T>
{
    private static readonly TaskCompletionSource<T> completedGuard = new();

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
            if (this.tcs == completedGuard)
            {
                return new(this.value);
            }
            else
            {
                this.tcs ??= new();
                return new(this.tcs.Task);
            }
        }
    }

    public void Start<TStateMachine>(ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine
    {
        var sc = SynchronizationContext.Current;
#if NET5_0_OR_GREATER
        var ec = ExecutionContext.Capture();
#endif
        try
        {
            stateMachine.MoveNext();
        }
        catch
        {
            SynchronizationContext.SetSynchronizationContext(sc);
#if NET5_0_OR_GREATER
            if (ec != null)
            {
                ExecutionContext.Restore(ec);
            }
#endif
            throw;
        }

        SynchronizationContext.SetSynchronizationContext(sc);
#if NET5_0_OR_GREATER
        if (ec != null)
        {
            ExecutionContext.Restore(ec);
        }
#endif
    }

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
        if (this.tcs != null)
        {
            this.tcs.SetResult(value);
        }
        else
        {
            this.value = value;
            this.tcs = completedGuard;
        }
    }

    public void SetException(Exception ex)
    {
        this.tcs ??= new();

        Debug.Assert(this.tcs != completedGuard);

        if (ex is OperationCanceledException)
        {
            this.tcs.SetCanceled();
        }
        else
        {
            this.tcs.SetException(ex);
        }
    }

    public void AwaitOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        this.tcs ??= new();

        // Will make boxed copy myself (inside of state machine).
        IAsyncStateMachine boxed = stateMachine;

        //Debug.Assert(boxed.builder.tcs != null);

        try
        {
            awaiter.OnCompleted(boxed.MoveNext);
        }
        catch (Exception ex)
        {
            Utilities.RethrowAsynchronously(ex);
        }
    }

    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        this.tcs ??= new();

        // Will make boxed copy myself (inside of state machine).
        IAsyncStateMachine boxed = stateMachine;

        //Debug.Assert(boxed.builder.tcs != null);

        try
        {
            awaiter.UnsafeOnCompleted(boxed.MoveNext);
        }
        catch (Exception ex)
        {
            Utilities.RethrowAsynchronously(ex);
        }
    }
}

//////////////////////////////////////////////////////////////////////

[EditorBrowsable(EditorBrowsableState.Never)]
[DebuggerStepThrough]
public struct LoggerAwaitableMethodBuilder
{
    private static readonly TaskCompletionSource<bool> completedGuard = new();

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
            if (this.tcs == completedGuard)
            {
                return new();
            }
            else
            {
                this.tcs ??= new();
                return new(this.tcs.Task);
            }
        }
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void Start<TStateMachine>(ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine
    {
        var sc = SynchronizationContext.Current;
#if NET5_0_OR_GREATER
        var ec = ExecutionContext.Capture();
#endif
        try
        {
            stateMachine.MoveNext();
        }
        catch
        {
            SynchronizationContext.SetSynchronizationContext(sc);
#if NET5_0_OR_GREATER
            if (ec != null)
            {
                ExecutionContext.Restore(ec);
            }
#endif
            throw;
        }

        SynchronizationContext.SetSynchronizationContext(sc);
#if NET5_0_OR_GREATER
        if (ec != null)
        {
            ExecutionContext.Restore(ec);
        }
#endif
    }

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
        if (this.tcs != null)
        {
            this.tcs.SetResult(true);
        }
        else
        {
            this.tcs = completedGuard;
        }
    }

    public void SetException(Exception ex)
    {
        this.tcs ??= new();

        Debug.Assert(this.tcs != completedGuard);

        if (ex is OperationCanceledException)
        {
            this.tcs.SetCanceled();
        }
        else
        {
            this.tcs.SetException(ex);
        }
    }

    public void AwaitOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        this.tcs ??= new();

        // Will make boxed copy myself (inside of state machine).
        IAsyncStateMachine boxed = stateMachine;

        //Debug.Assert(boxed.builder.tcs != null);

        try
        {
            awaiter.OnCompleted(boxed.MoveNext);
        }
        catch (Exception ex)
        {
            Utilities.RethrowAsynchronously(ex);
        }
    }

    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        this.tcs ??= new();

        // Will make boxed copy myself (inside of state machine).
        IAsyncStateMachine boxed = stateMachine;

        //Debug.Assert(boxed.builder.tcs != null);

        try
        {
            awaiter.UnsafeOnCompleted(boxed.MoveNext);
        }
        catch (Exception ex)
        {
            Utilities.RethrowAsynchronously(ex);
        }
    }
}
