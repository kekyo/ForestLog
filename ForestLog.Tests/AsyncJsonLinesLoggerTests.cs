////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using ForestLog.Internal;
using ForestLog.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ForestLog;

public sealed class AsyncJsonLinesLoggerTests
{
    public static IEnumerable<JObject?> LoadLines(string path)
    {
        using var fs = new FileStream(
            path, FileMode.Open, FileAccess.Read, FileShare.Read);
        var tr = new StreamReader(fs);

        while (true)
        {
            var line = tr.ReadLine();
            if (line == null)
            {
                break;
            }

            if (CoreUtilities.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var jr = new JsonTextReader(new StringReader(line));

            JObject? jo;
            try
            {
                jo = Utilities.JsonSerializer.Deserialize<JToken>(jr) as JObject;
            }
            catch (Exception ex)
            {
                throw new FormatException(line, ex);
            }
            yield return jo;
        }
    }

    public static async LoggerAwaitable<JObject?[]> LogTestBlockAsync(
        Func<ILogger, LoggerAwaitable> action,
        LogLevels maximumOutputLogLevel = LogLevels.Debug,
        long sizeToNextFile = 1 * 1024 * 1024,
        int maximumLogFiles = 0)
    {
        var (results, _) = await LogTestBlockAsync(
            (_, logger) => action(logger),
            maximumOutputLogLevel,
            sizeToNextFile,
            maximumLogFiles);
        return results;
    }

    public readonly struct LogTestBlockAsyncResult
    {
        public readonly JObject?[] results;
        public readonly int files;

        public LogTestBlockAsyncResult(JObject?[] results, int files)
        {
            this.results = results;
            this.files = files;
        }

        public void Deconstruct(out JObject?[] results, out int files)
        {
            results = this.results;
            files = this.files;
        }
    }

    public static async LoggerAwaitable<LogTestBlockAsyncResult> LogTestBlockAsync(
        Func<ILogController, ILogger, LoggerAwaitable> action,
        LogLevels maximumOutputLogLevel = LogLevels.Debug,
        long sizeToNextFile = 1 * 1024 * 1024,
        int maximumLogFiles = 0)
    {
        var basePath = Path.Combine(
            Path.GetDirectoryName(typeof(AsyncJsonLinesLoggerTests).Assembly.Location)!,
            $"logs_{Guid.NewGuid():N}");

        if (!Directory.Exists(basePath))
        {
            try
            {
                Directory.CreateDirectory(basePath);
            }
            catch
            {
            }
        }

        try
        {
            using (var logController = LogController.Factory.CreateJsonLines(
                basePath, maximumOutputLogLevel, sizeToNextFile, maximumLogFiles))
            {
                var logger = logController.CreateLogger();

                await action(logController, logger);
            }

            var paths = Directory.GetFiles(
                basePath, "log*.jsonl", SearchOption.AllDirectories);

            return new(paths.
                SelectMany(path => LoadLines(path)).
                ToArray(), paths.Length);
        }
        finally
        {
            if (Directory.Exists(basePath))
            {
                Directory.Delete(basePath, true);
            }
        }
    }

    //////////////////////////////////////////////////////////

    [TestCase(10, 1)]
    [TestCase(10, 2)]
    [TestCase(10, 5)]
    public async Task RotateLogFiles(long sizeToNextFile, int maximumLogFiles)
    {
        var (lines, files) = await LogTestBlockAsync(
            async (_, logger) =>
            {
                for (var index = 0; index < 100; index++)
                {
                    // This test needs to asynchronously output.
                    // Because will flush and rotate one entry each files.
                    await logger.DebugAsync($"AAA{index}BBB");
                }
            },
            LogLevels.Debug,
            sizeToNextFile,
            maximumLogFiles);

        Assert.AreEqual(maximumLogFiles, files, "files");
        Assert.AreEqual(maximumLogFiles, lines.Length, "lines");
    }

    //////////////////////////////////////////////////////////

    [TestCase(LogLevels.Debug)]
    [TestCase(LogLevels.Trace)]
    [TestCase(LogLevels.Information)]
    [TestCase(LogLevels.Warning)]
    [TestCase(LogLevels.Error)]
    [TestCase(LogLevels.Fatal)]
    public async Task LogSingleMessage(LogLevels logLevel)
    {
        var lines = await LogTestBlockAsync(async logger =>
        {
            var value = 123;
            await logger.LogAsync(logLevel, $"AAA{value}BBB");
        });

        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
        Assert.AreEqual(logLevel.ToString().ToLowerInvariant(), lines.Single()?["logLevel"]?.ToString());
    }

    [Test]
    public async Task LogIgnore1()
    {
        var lines = await LogTestBlockAsync(async logger =>
        {
            var value = 123;
            await logger.LogAsync(LogLevels.Ignore, $"AAA{value}BBB");
        });

        Assert.AreEqual(0, lines.Length);
    }

    [Test]
    public async Task LimitOutputLogLevel()
    {
        for (var targetLogLevel = LogLevels.Debug;
            targetLogLevel <= LogLevels.Fatal;
            targetLogLevel++)
        {
            for (var minimumOutputLogLevel = LogLevels.Debug;
                minimumOutputLogLevel <= LogLevels.Fatal;
                minimumOutputLogLevel++)
            {
                var lines = await LogTestBlockAsync(async logger =>
                {
                    var value = 123;
                    await logger.LogAsync(targetLogLevel, $"AAA{value}BBB");
                },
                minimumOutputLogLevel);

                if (targetLogLevel >= minimumOutputLogLevel)
                {
                    Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
                    Assert.AreEqual(targetLogLevel.ToString().ToLowerInvariant(), lines.Single()?["logLevel"]?.ToString());
                }
                else
                {
                    Assert.AreEqual(0, lines.Length);
                }
            }
        }
    }

    [Test]
    public async Task LogIgnore2()
    {
        var lines = await LogTestBlockAsync(async logger =>
        {
            var value = 123;
            await logger.LogAsync(LogLevels.Debug, $"AAA{value}BBB");
        }, LogLevels.Ignore);

        Assert.AreEqual(0, lines.Length);
    }

    //////////////////////////////////////////////////////////

    [TestCase(LogLevels.Debug)]
    [TestCase(LogLevels.Trace)]
    [TestCase(LogLevels.Information)]
    [TestCase(LogLevels.Warning)]
    [TestCase(LogLevels.Error)]
    [TestCase(LogLevels.Fatal)]
    public async Task ScopedLogMessage(LogLevels logLevel)
    {
        var lines = await LogTestBlockAsync(async logger =>
        {
            using var childLogger = await logger.ScopeAsync(logLevel);

            var value = 123;
            childLogger.Log(LogLevels.Warning, $"AAA{value}BBB");
        });

        Assert.AreEqual(3, lines.Length);

        Assert.AreEqual("Enter: Parent=2", lines[0]?["message"]?.ToString());
        Assert.AreEqual(logLevel.ToString().ToLowerInvariant(), lines[0]?["logLevel"]?.ToString());

        Assert.AreEqual("AAA123BBB", lines[1]?["message"]?.ToString());
        Assert.AreEqual(LogLevels.Warning.ToString().ToLowerInvariant(), lines[1]?["logLevel"]?.ToString());

        Assert.IsTrue(lines[2]?["message"]?.ToString().StartsWith("Leave: Elapsed="));
        Assert.AreEqual(logLevel.ToString().ToLowerInvariant(), lines[2]?["logLevel"]?.ToString());
    }
}
