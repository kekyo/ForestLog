////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using ForestLog.Infrastructure;

namespace ForestLog;

public static class LoggerFactory
{
    public static Logger CreateJsonLineLogger(LogLevels minimumLogLevel, string basePath) =>
        new JsonLineLogController(minimumLogLevel, basePath).CreateLogger();
}
