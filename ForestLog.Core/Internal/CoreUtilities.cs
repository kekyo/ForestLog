////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ForestLog.Internal;

[DebuggerStepThrough]
internal static class CoreUtilities
{
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static IFormattable FormatException(Exception ex) =>
        $"{ex.GetType().FullName}: {ex.Message}";

    [DllImport("kernel32")]
    private static extern int GetCurrentThreadId();

    private static readonly Func<int> getCurrentThreadId =
#if NETSTANDARD || NETCOREAPP
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
#else
        Environment.OSVersion.Platform == PlatformID.Win32NT ?
#endif
            GetCurrentThreadId :
            () => Thread.CurrentThread.ManagedThreadId;

    public static int NativeThreadId
    {
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        get => getCurrentThreadId();
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task<T> FromResult<T>(T value) =>
#if NET35 || NET40
        TaskEx.FromResult(value);
#else
        Task.FromResult(value);
#endif

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task FromException(Exception ex)
    {
#if NET35 || NET40 || NET45 || NET452
        var tcs = new TaskCompletionSource<bool>();
        tcs.SetException(ex);
        return tcs.Task;
#else
        return Task.FromException(ex);
#endif
    }

    public static Task CompletedTask
    {
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        get =>
#if NET35 || NET40
            TaskEx.FromResult(true);
#elif NET45 || NET452
            Task.FromResult(true);
#else
            Task.CompletedTask;
#endif
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task Delay(TimeSpan delay) =>
#if NET35 || NET40
        TaskEx.Delay(delay);
#else
        Task.Delay(delay);
#endif

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task Delay(int millisecondsDelay) =>
#if NET35 || NET40
        TaskEx.Delay(millisecondsDelay);
#else
        Task.Delay(millisecondsDelay);
#endif

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task WhenAll(IEnumerable<Task> enumerable) =>
#if NET35 || NET40
        TaskEx.WhenAll(enumerable);
#else
        Task.WhenAll(enumerable);
#endif

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task<T[]> WhenAll<T>(IEnumerable<Task<T>> enumerable) =>
#if NET35 || NET40
        TaskEx.WhenAll(enumerable);
#else
        Task.WhenAll(enumerable);
#endif

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Task<T> Run<T>(Func<T> action) =>
#if NET35 || NET40
        TaskEx.Run(action);
#else
        Task.Run(action);
#endif

    public static void RethrowAsynchronously(Exception ex)
    {
#if NET35 || NET40
        ThreadPool.QueueUserWorkItem(p =>
        {
            var ex = (Exception)p!;
            throw new TargetInvocationException(ex);
        }, ex);
#else
        var edi = System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex);
        ThreadPool.QueueUserWorkItem(p =>
        {
            var edi = (System.Runtime.ExceptionServices.ExceptionDispatchInfo)p!;
            edi.Throw();
        }, edi);
#endif
    }

#if NET35 || NET40 || NET45 || NET452
    private static class EmptyArray<T>
    {
        public static readonly T[] Empty = new T[0];
    }
#endif

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T[] Empty<T>() =>
#if NET35 || NET40 || NET45 || NET452
        EmptyArray<T>.Empty;
#else
        Array.Empty<T>();
#endif

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static bool IsNullOrWhiteSpace(string? text) =>
#if NET35
        string.IsNullOrEmpty(text) || (text!.Trim().Length == 0);
#else
        string.IsNullOrWhiteSpace(text);
#endif

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static IEnumerable<string> EnumerateFiles(
        string path, string pattern, SearchOption so) =>
#if NET35
        Directory.GetFiles(path, pattern, so);
#else
        Directory.EnumerateFiles(path, pattern, so);
#endif
}
