////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using MQTTnet.Diagnostics.Logger;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ForestLog.Internal;

[DebuggerStepThrough]
internal static class Utilities
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LogLevels ToLogLevel(
        MqttNetLogLevel logLevel) =>
        logLevel switch
        {
            MqttNetLogLevel.Verbose => LogLevels.Trace,
            MqttNetLogLevel.Info => LogLevels.Information,
            MqttNetLogLevel.Warning => LogLevels.Warning,
            MqttNetLogLevel.Error => LogLevels.Error,
            _ => LogLevels.Debug,
        };
}
