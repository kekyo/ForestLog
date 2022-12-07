////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using ForestLog.Infrastructure;
using System;

namespace ForestLog;

public static class LoggerFactory
{
    public static ILogController CreateJsonLineLogController(
        string basePath, LogLevels minimumLogLevel, long sizeToNextFile) =>
        new JsonLineLogController(basePath, minimumLogLevel, sizeToNextFile);

    public static ILogController CreateJsonLineLogController(
        string basePath, LogLevels minimumLogLevel) =>
        new JsonLineLogController(basePath, minimumLogLevel, 1 * 1024 * 1024);

    [Obsolete]
    public static ILogController CreateJsonLineLogController(
        LogLevels minimumLogLevel, string basePath) =>
        new JsonLineLogController(basePath, minimumLogLevel, 1 * 1024 * 1024);
}
