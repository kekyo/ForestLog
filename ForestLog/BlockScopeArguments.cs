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

/// <summary>
/// Contains method arguments of block logging.
/// </summary>
[DebuggerStepThrough]
public readonly struct BlockScopeArguments
{
    /// <summary>
    /// Method arguments.
    /// </summary>
    public readonly object?[] Arguments;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="arguments">Method arguments</param>
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public BlockScopeArguments(params object?[] arguments) =>
        this.Arguments = arguments;
}
