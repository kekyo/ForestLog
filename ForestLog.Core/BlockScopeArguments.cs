////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ForestLog;

[DebuggerStepThrough]
public readonly struct BlockScopeArguments
{
    public readonly object?[] Arguments;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public BlockScopeArguments(params object?[] arguments) =>
        this.Arguments = arguments;
}
