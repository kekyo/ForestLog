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

[Obsolete]
public static class LoggerFactory
{
    [Obsolete]
    public static ILogController CreateJsonLineLogController(
        string basePath, LogLevels minimumOutputLogLevel, long sizeToNextFile, int maximumLogFiles) =>
        new JsonLinesLogController(basePath, minimumOutputLogLevel, sizeToNextFile, maximumLogFiles);

    [Obsolete]
    public static ILogController CreateJsonLineLogController(
        string basePath, LogLevels minimumOutputLogLevel, long sizeToNextFile) =>
        new JsonLinesLogController(basePath, minimumOutputLogLevel, sizeToNextFile, 0);

    [Obsolete]
    public static ILogController CreateJsonLineLogController(
        string basePath, LogLevels minimumOutputLogLevel) =>
        new JsonLinesLogController(basePath, minimumOutputLogLevel, 0, 0);
}
