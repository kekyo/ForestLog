﻿////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using ForestLog.Internal;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ForestLog.Tasks;

[DebuggerStepThrough]
[EditorBrowsable(EditorBrowsableState.Never)]
[AsyncMethodBuilder(typeof(LoggerAwaitableMethodBuilder<>))]
public struct LoggerAwaitable<T>
{
    internal Task<T>? task;
    internal T value;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal LoggerAwaitable(T value)
    {
        this.task = null;
        this.value = value;
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal LoggerAwaitable(Task<T> task)
    {
#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
        if (task.IsCompletedSuccessfully)
#else
        if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
#endif
        {
            this.value = task.GetAwaiter().GetResult();
        }
        else
        {
            this.value = default!;
            this.task = task;
        }
    }

#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal LoggerAwaitable(ValueTask<T> task)
    {
        if (task.IsCompletedSuccessfully)
        {
            this.value = task.GetAwaiter().GetResult();
        }
        else
        {
            this.value = default!;
            this.task = task.AsTask();
        }
    }
#endif

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public LoggerAwaiter<T> GetAwaiter() =>
        new(this.task, this.value);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static implicit operator LoggerAwaitable<T>(Task<T> rhs) =>
        new(rhs);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static implicit operator LoggerAwaitable<T>(T value) =>
        new(value);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static implicit operator Task<T>(LoggerAwaitable<T> rhs) =>
        rhs.task ?? Utilities.FromResult(rhs.value);

#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator LoggerAwaitable<T>(ValueTask<T> rhs) =>
        new(rhs);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ValueTask<T>(LoggerAwaitable<T> rhs) =>
        rhs.task != null ? new(rhs.task) : new(rhs.value);
#endif
}

//////////////////////////////////////////////////////////////////////

[DebuggerStepThrough]
[EditorBrowsable(EditorBrowsableState.Never)]
[AsyncMethodBuilder(typeof(LoggerAwaitableMethodBuilder))]
public partial struct LoggerAwaitable
{
    private Task? task;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal LoggerAwaitable(Task task)
    {
#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
        if (task.IsCompletedSuccessfully)
#else
        if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
#endif
        {
            task.GetAwaiter().GetResult();
        }
        else
        {
            this.task = task;
        }
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal LoggerAwaitable(ValueTask task)
    {
        if (task.IsCompletedSuccessfully)
        {
            task.GetAwaiter().GetResult();
        }
        else
        {
            this.task = task.AsTask();
        }
    }
#endif

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public LoggerAwaiter GetAwaiter() =>
        new(this.task);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static implicit operator LoggerAwaitable(Task rhs) =>
        new(rhs);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static implicit operator Task(LoggerAwaitable rhs) =>
        rhs.task ?? Utilities.CompletedTask;

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator LoggerAwaitable(ValueTask rhs) =>
        new(rhs);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ValueTask(LoggerAwaitable rhs) =>
        rhs.task != null ? new(rhs.task) : default;
#endif
}
