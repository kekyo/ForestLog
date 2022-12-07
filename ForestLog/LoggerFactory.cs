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
        string basePath, LogLevels minimumOutputLogLevel, long sizeToNextFile) =>
        new JsonLineLogController(basePath, minimumOutputLogLevel, sizeToNextFile);

    public static ILogController CreateJsonLineLogController(
        string basePath, LogLevels minimumOutputLogLevel) =>
        new JsonLineLogController(basePath, minimumOutputLogLevel, 1 * 1024 * 1024);
}
