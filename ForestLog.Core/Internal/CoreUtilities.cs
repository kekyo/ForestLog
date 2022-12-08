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

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static object ToExceptionDetailObject(Exception ex) =>
        new { Name = ex.GetType().FullName, Message = ex.Message };

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

    public static Task CompletedTask
    {
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        get =>
#if NET35 || NET40
            TaskEx.FromResult(true);
#elif NET45
            Task.FromResult(true);
#else
            Task.CompletedTask;
#endif
    }

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
}
