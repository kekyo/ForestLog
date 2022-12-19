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

namespace ForestLog.Infrastructure;

[DebuggerStepThrough]
internal sealed class LoggerDelayedInterpolatedStringArgument<T> : IFormattable
{
    public readonly Func<T> Delayed;
    public readonly string? Format;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public LoggerDelayedInterpolatedStringArgument(Func<T> delayed, string? format)
    {
        this.Delayed = delayed;
        this.Format = format;
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public string ToString(
        string? format, IFormatProvider? formatProvider) =>
        LoggerInterpolatedStringArgument<T>.ToString(this.Delayed(), this.Format, formatProvider);
}
