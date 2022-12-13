////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

namespace ForestLog;

/// <summary>
/// ForestLog log level enumeration.
/// </summary>
public enum LogLevels
{
    /// <summary>
    /// For debug.
    /// </summary>
    Debug,

    /// <summary>
    /// For trace.
    /// </summary>
    Trace,

    /// <summary>
    /// For information.
    /// </summary>
    Information,

    /// <summary>
    /// For warning.
    /// </summary>
    Warning,

    /// <summary>
    /// For error.
    /// </summary>
    Error,

    /// <summary>
    /// For fatal.
    /// </summary>
    Fatal,

    /// <summary>
    /// Ignore log entry, only using for log controller limitation.
    /// </summary>
    Ignore = int.MaxValue,
}
