////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using MQTTnet.Diagnostics.Logger;
using System.Runtime.CompilerServices;
using System;
using ForestLog.Internal;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace ForestLog;

/// <summary>
/// Wrapper of producing MQTTnet 3.1 logger interface.
/// </summary>
[DebuggerStepThrough]
public sealed class MqttNetLogger : IMqttNetLogger
{
    private readonly ILogController logController;
    private readonly ConcurrentDictionary<string, ILogger> loggers = new();

    public MqttNetLogger(ILogController logController) =>
        this.logController = logController;

    public bool IsEnabled =>
        true;

    public void Publish(
        MqttNetLogLevel logLevel,
        string source,
        string message,
        object?[]? parameters,
        Exception? exception)
    {
        if (exception is { })
        {
            this.loggers.GetOrAdd(source, this.logController.CreateLogger).
                Log(Utilities.ToLogLevel(logLevel),
                    exception,
                    FormattableStringFactory.Create(message, parameters ?? CoreUtilities.Empty<object?>()));
        }
        else
        {
            this.loggers.GetOrAdd(source, this.logController.CreateLogger).
                Log(Utilities.ToLogLevel(logLevel),
                    FormattableStringFactory.Create(message, parameters ?? CoreUtilities.Empty<object?>()));
        }
    }
}
