////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using ForestLog.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ForestLog;

public static class LoggingBuilderExtension
{
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    public static ILoggingBuilder AddForestLog(
        this ILoggingBuilder builder,
        ILogController logController,
        string? headName = null) =>
        builder.AddProvider(new ForestLogBridgeLoggerProvider(logController, headName));
#endif

    public static IServiceCollection AddForestLog(
        this IServiceCollection service,
        ILogController logController,
        string? headName = null) =>
        service.AddSingleton(new ForestLogBridgeLoggerProvider(logController, headName));
}
