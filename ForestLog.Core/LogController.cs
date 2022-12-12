////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

namespace ForestLog;

public sealed class LogControllerFactory
{
    internal LogControllerFactory()
    {
    }
}

/// <summary>
/// ForestLog controller factory base class.
/// </summary>
public static class LogController
{
    public static readonly LogControllerFactory Factory = new();
}
