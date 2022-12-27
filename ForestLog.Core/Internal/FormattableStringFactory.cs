////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

#if NET35 || NET40 || NET45 || NET452

using System.Diagnostics;

namespace System.Runtime.CompilerServices;

[DebuggerStepThrough]
internal static class FormattableStringFactory
{
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static IFormattable Create(string format, params object?[] arguments) =>
        new FormattableString(format, arguments);

    private sealed class FormattableString : IFormattable
    {
        private readonly string format;
        private readonly object?[] arguments;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public FormattableString(string format, object?[] arguments)
        {
            this.format = format;
            this.arguments = arguments;
        }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public string ToString(string? format, IFormatProvider? formatProvider) =>
            string.Format(formatProvider, this.format, this.arguments);
    }
}

#endif
