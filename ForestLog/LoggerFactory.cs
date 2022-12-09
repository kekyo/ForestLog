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
    /// <summary>
    /// Create Json Lines log controller.
    /// </summary>
    /// <param name="basePath">Store to base path.</param>
    /// <param name="minimumOutputLogLevel">Minimum output log level.</param>
    /// <param name="sizeToNextFile">Size to change next file.</param>
    /// <param name="maximumLogFiles">Maximum log files.</param>
    /// <returns>Log controller.</returns>
    /// <remarks>Enabled log rotation by size and files.</remarks>
    public static ILogController CreateJsonLinesLogController(
        string basePath, LogLevels minimumOutputLogLevel, long sizeToNextFile = 0, int maximumLogFiles = 0) =>
        new JsonLineLogController(basePath, minimumOutputLogLevel, sizeToNextFile, maximumLogFiles);

    /// <summary>
    /// Create Json Lines log controller.
    /// </summary>
    /// <param name="basePath">Store to base path.</param>
    /// <param name="minimumOutputLogLevel">Minimum output log level.</param>
    /// <param name="sizeToNextFile">Size to change next file.</param>
    /// <param name="maximumLogFiles">Maximum log files.</param>
    /// <returns>Log controller.</returns>
    /// <remarks>Enabled log rotation by size and files.</remarks>
    [Obsolete]
    public static ILogController CreateJsonLineLogController(
        string basePath, LogLevels minimumOutputLogLevel, long sizeToNextFile, int maximumLogFiles) =>
        new JsonLineLogController(basePath, minimumOutputLogLevel, sizeToNextFile, maximumLogFiles);

    /// <summary>
    /// Create Json Lines log controller.
    /// </summary>
    /// <param name="basePath">Store to base path.</param>
    /// <param name="minimumOutputLogLevel">Minimum output log level.</param>
    /// <param name="sizeToNextFile">Size to change next file.</param>
    /// <returns>Log controller.</returns>
    /// <remarks>Enabled log splitting by size.</remarks>
    [Obsolete]
    public static ILogController CreateJsonLineLogController(
        string basePath, LogLevels minimumOutputLogLevel, long sizeToNextFile) =>
        new JsonLineLogController(basePath, minimumOutputLogLevel, sizeToNextFile, 0);

    /// <summary>
    /// Create Json Lines log controller.
    /// </summary>
    /// <param name="basePath">Store to base path.</param>
    /// <param name="minimumOutputLogLevel">Minimum output log level.</param>
    /// <returns>Log controller.</returns>
    /// <remarks>Disabled log splitting and rotation feature.</remarks>
    [Obsolete]
    public static ILogController CreateJsonLineLogController(
        string basePath, LogLevels minimumOutputLogLevel) =>
        new JsonLineLogController(basePath, minimumOutputLogLevel, 0, 0);
}
