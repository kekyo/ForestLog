////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ForestLog.Internal;

internal static class Utilities
{
    public static readonly int ProcessId =
#if NET5_0_OR_GREATER
        Environment.ProcessId;
#else
        Process.GetCurrentProcess().Id;
#endif

    public static readonly Encoding UTF8 = new UTF8Encoding(false, false);
    public static readonly JsonSerializer JsonSerializer;

    static Utilities()
    {
        var defaultNamingStrategy = new CamelCaseNamingStrategy();
        JsonSerializer = new JsonSerializer
        {
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            NullValueHandling = NullValueHandling.Include,
            Formatting = Formatting.None,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            ContractResolver = new DefaultContractResolver
            { NamingStrategy = defaultNamingStrategy, },
        };
        JsonSerializer.Converters.Add(new StringEnumConverter(defaultNamingStrategy));
    }

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

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static bool IsNullOrWhiteSpace(string? text) =>
#if NET35
        string.IsNullOrEmpty(text) || (text!.Trim().Length == 0);
#else
        string.IsNullOrWhiteSpace(text);
#endif

#if NET35 || NET40 || NET45
    private static class EmptyArray<T>
    {
        public static readonly T[] Empty = new T[0]; 
    }
#endif

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T[] Empty<T>() =>
#if NET35 || NET40 || NET45
        EmptyArray<T>.Empty;
#else
        Array.Empty<T>();
#endif
}
