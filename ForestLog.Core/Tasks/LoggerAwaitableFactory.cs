////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ForestLog.Tasks;

partial struct LoggerAwaitable
{
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable<T> FromResult<T>(T value) =>
        new LoggerAwaitable<T>(value);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable FromTask(Task task)
    {
        if (task.IsCompleted)
        {
            task.Wait();
            return default;
        }
        else
        {
            return new(task);
        }
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable<T> FromTask<T>(Task<T> task) =>
        task.IsCompleted ?
            new(task.Result) :
            new(task);

    //////////////////////////////////////////////////////////////////////

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LoggerAwaitable FromValueTask(ValueTask task)
    {
        if (task.IsCompleted)
        {
            task.GetAwaiter().GetResult();
            return default;
        }
        else
        {
            return new(task.AsTask());
        }
    }
#endif

#if NETCOREAPP || NETSTANDARD2_1
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LoggerAwaitable<T> FromValueTask<T>(ValueTask<T> task) =>
        task.IsCompleted ?
            new(task.GetAwaiter().GetResult()) :
            new(task.AsTask());
#endif
}
