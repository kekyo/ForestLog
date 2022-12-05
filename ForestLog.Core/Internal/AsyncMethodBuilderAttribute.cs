////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

#if !(NETCOREAPP || NETSTANDARD2_1)

namespace System.Runtime.CompilerServices;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false, AllowMultiple = false)]
internal sealed class AsyncMethodBuilderAttribute : Attribute
{
    public AsyncMethodBuilderAttribute(Type builderType) =>
        this.BuilderType = builderType;

    public Type BuilderType { get; }
}

#endif
