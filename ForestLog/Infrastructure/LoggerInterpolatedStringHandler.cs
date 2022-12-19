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
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace ForestLog.Infrastructure;

[InterpolatedStringHandler]
[DebuggerStepThrough]
[EditorBrowsable(EditorBrowsableState.Never)]
public struct LoggerInterpolatedStringHandler : IFormattable
{
    private readonly List<IFormattable>? arguments;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LoggerInterpolatedStringHandler(
        int literalLength, int formattedCount,
        ILogger logger, LogLevels logLevel,
        out bool cont)
    {
        if (logLevel >= logger.MinimumOutputLogLevel)
        {
            cont = true;

            // formattedCount: 0, 1,  1,  1,   2
            // capacity:       1, 3,  3,  3,   5
            // items:          L, LF, FL, LFL, FLF
            var capacity = formattedCount * 2 + 1;
            this.arguments = new(capacity);
        }
        else
        {
            cont = false;
            this.arguments = null;
        }
    }

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendLiteral(string literal) =>
        this.arguments!.Add(
            new LoggerInterpolatedStringArgument<string>(literal, null));

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted<T>(T value) =>
        this.arguments!.Add(
            value is IFormattable f ? f : new LoggerInterpolatedStringArgument<T>(value, null));

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted<T>(T value, string format) =>
        this.arguments!.Add(
            (format == null && value is IFormattable f) ? f : new LoggerInterpolatedStringArgument<T>(value, format));

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted<T>(Func<T> delayed) =>
        this.arguments!.Add(
            new LoggerDelayedInterpolatedStringArgument<T>(delayed, null));

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted<T>(Func<T> delayed, string format) =>
        this.arguments!.Add(
            new LoggerDelayedInterpolatedStringArgument<T>(delayed, format));

    //////////////////////////////////////////////////////////////////////

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString()
    {
        if (this.arguments is { })
        {
            var sb = new StringBuilder();
            foreach (var arg in this.arguments)
            {
                sb.Append(arg.ToString());
            }
            return sb.ToString();
        }
        else
        {
            return string.Empty;
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (this.arguments is { })
        {
            var sb = new StringBuilder();
            foreach (var arg in this.arguments)
            {
                sb.Append(arg.ToString(null, formatProvider));
            }
            return sb.ToString();
        }
        else
        {
            return string.Empty;
        }
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) =>
        this.ToString(format, formatProvider);
}
