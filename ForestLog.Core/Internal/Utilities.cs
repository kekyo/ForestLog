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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ForestLog.Internal;

internal static class Utilities
{
    public static IFormattable FormatException(Exception ex) =>
        $"{ex.GetType().FullName}: {ex.Message}";

    public static IFormattable FormatLeaveWithException(Exception ex) =>
        $"Leave with: {ex.GetType().FullName}: {ex.Message}";

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

    public static Task<T> FromResult<T>(T value) =>
#if NET35 || NET40
        TaskEx.FromResult(value);
#else
        Task.FromResult(value);
#endif

    public static Task CompletedTask =>
#if NET35 || NET40
        TaskEx.FromResult(true);
#elif NET45
        Task.FromResult(true);
#else
        Task.CompletedTask;
#endif
}
