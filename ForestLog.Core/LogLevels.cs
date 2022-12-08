////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

namespace ForestLog;

public enum LogLevels
{
    Debug,
    Trace,
    Information,
    Warning,
    Error,
    Ignore = int.MaxValue,
}
