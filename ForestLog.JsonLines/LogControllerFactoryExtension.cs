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

// Parameter has no matching param tag in the XML comment (but other parameters do)
#pragma warning disable CS1573

/// <summary>
/// ForestLog controller factory extension.
/// </summary>
public static class LogControllerFactoryExtension
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
        this LogControllerFactory _,
        string basePath, LogLevels minimumOutputLogLevel, long sizeToNextFile = 0, int maximumLogFiles = 0) =>
        new JsonLinesLogController(basePath, minimumOutputLogLevel, sizeToNextFile, maximumLogFiles);
}
