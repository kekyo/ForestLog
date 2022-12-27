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

namespace ForestLog.Handlers;

[DebuggerStepThrough]
internal sealed class LoggerInterpolatedStringArgument<T> : IFormattable
{
    public readonly T Value;
    public readonly string? Format;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public LoggerInterpolatedStringArgument(
        T value, string? format)
    {
        this.Value = value;
        this.Format = format;
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static string ToString(
        T value, string? format, IFormatProvider? formatProvider) =>
        value switch
        {
            IFormattable f => f.ToString(format, formatProvider),
            _ => value?.ToString() ?? string.Empty,
        };

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public string ToString(
        string? format, IFormatProvider? formatProvider) =>
        ToString(this.Value, this.Format, formatProvider);
}
