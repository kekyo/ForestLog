////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

#if !NET6_0_OR_GREATER

using System.Diagnostics;

namespace System.Runtime.CompilerServices;

[DebuggerStepThrough]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
internal sealed class InterpolatedStringHandlerAttribute : Attribute
{
    public InterpolatedStringHandlerAttribute()
    {
    }
}

#endif
