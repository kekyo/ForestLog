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

public static class LogControllerFactory
{
    /// <summary>
    /// Create Json Lines log controller.
    /// </summary>
    /// <param name="basePath">Store to base path.</param>
    /// <param name="minimumOutputLogLevel">Minimum output log level.</param>
    /// <param name="sizeToNextFile">Size to change next file.</param>
    /// <param name="maximumLogFiles">Maximum log files.</param>
    /// <returns>Log controller.</returns>
    /// <remarks>Enabled log rotation by size and files.</remarks>
    public static ILogController CreateJsonLines(
        string basePath, LogLevels minimumOutputLogLevel, long sizeToNextFile = 0, int maximumLogFiles = 0) =>
        new JsonLinesLogController(basePath, minimumOutputLogLevel, sizeToNextFile, maximumLogFiles);
}
