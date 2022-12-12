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
using Microsoft.Extensions.Logging;

namespace ForestLog.Internal;

[DebuggerStepThrough]
internal static class Utilities
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LogLevels ToLogLevel(
        LogLevel logLevel) =>
        logLevel switch
        {
            LogLevel.Debug => LogLevels.Debug,
            LogLevel.Trace => LogLevels.Trace,
            LogLevel.Information => LogLevels.Information,
            LogLevel.Warning => LogLevels.Warning,
            LogLevel.Error => LogLevels.Error,
            _ => LogLevels.Fatal,
        };
}
