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
        new(value);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable<T> FromTask<T>(Task<T> task) =>
        new(task);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static LoggerAwaitable FromTask(Task task) =>
        new(task);

    //////////////////////////////////////////////////////////////////////

#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LoggerAwaitable<T> FromTask<T>(ValueTask<T> task) =>
        new(task);
#endif

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LoggerAwaitable FromTask(ValueTask task) =>
        new(task);
#endif
}
