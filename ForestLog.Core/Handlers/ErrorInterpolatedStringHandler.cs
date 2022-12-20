////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ForestLog.Handlers;

[InterpolatedStringHandler]
[DebuggerStepThrough]
[EditorBrowsable(EditorBrowsableState.Never)]
public struct ErrorInterpolatedStringHandler : IFormattable
{
    private readonly LoggerInterpolatedStringHandler handler;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ErrorInterpolatedStringHandler(
        int literalLength, int formattedCount,
        ILogger logger,
        out bool cont) =>
        this.handler = new(literalLength, formattedCount, logger, LogLevels.Error, out cont);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendLiteral(string literal) =>
        this.handler.AppendLiteral(literal);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted<T>(T value) =>
        this.handler.AppendFormatted(value);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted<T>(T value, string format) =>
        this.handler.AppendFormatted(value, format);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted<T>(Func<T> delayed) =>
        this.handler.AppendFormatted(delayed);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted<T>(Func<T> delayed, string format) =>
        this.handler.AppendFormatted(delayed, format);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() =>
        this.handler.ToString();

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public string ToString(string? format, IFormatProvider? formatProvider) =>
        this.handler.ToString(format, formatProvider);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) =>
        this.handler.ToString(format, formatProvider);
}
