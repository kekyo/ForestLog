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
using System.Threading.Tasks;

namespace ForestLog;

public sealed class JsonLinesLoggerTests
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

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var jr = new JsonTextReader(new StringReader(line));
            yield return Utilities.JsonSerializer.Deserialize<JObject>(jr);
        }
    }

    public static JObject?[] LogTestBlock(
        Action<ILogger> action,
        LogLevels maximumOutputLogLevel = LogLevels.Debug,
        long sizeToNextFile = 1 * 1024 * 1024,
        int maximumLogFiles = 0) =>
        LogTestBlock(
            out var _,
            (_, logger) => action(logger),
            maximumOutputLogLevel,
            sizeToNextFile,
            maximumLogFiles);

    public static JObject?[] LogTestBlock(
        out int files,
        Action<ILogController, ILogger> action,
        LogLevels maximumOutputLogLevel = LogLevels.Debug,
        long sizeToNextFile = 1 * 1024 * 1024,
        int maximumLogFiles = 0)
    {
        var basePath = Path.Combine(
            Path.GetDirectoryName(typeof(JsonLinesLoggerTests).Assembly.Location)!,
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

                action(logController, logger);
            }

            var paths = Directory.GetFiles(
                basePath, "log*.jsonl", SearchOption.AllDirectories);
            files = paths.Length;

            return paths.
                SelectMany(path => LoadLines(path)).
                ToArray();
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

    [TestCase(LogLevels.Debug)]
    [TestCase(LogLevels.Trace)]
    [TestCase(LogLevels.Information)]
    [TestCase(LogLevels.Warning)]
    [TestCase(LogLevels.Error)]
    [TestCase(LogLevels.Fatal)]
    public void LogSingleMessage(LogLevels logLevel)
    {
        var lines = LogTestBlock(logger =>
        {
            var value = 123;
            logger.Log(logLevel, $"AAA{value}BBB");
        });

        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
        Assert.AreEqual(logLevel.ToString().ToLowerInvariant(), lines.Single()?["logLevel"]?.ToString());
    }

    [Test]
    public void LogIgnore1()
    {
        var lines = LogTestBlock(logger =>
        {
            var value = 123;
            logger.Log(LogLevels.Ignore, $"AAA{value}BBB");
        });

        Assert.AreEqual(0, lines.Length);
    }

    [Test]
    public void LimitOutputLogLevel()
    {
        for (var targetLogLevel = LogLevels.Debug;
            targetLogLevel <= LogLevels.Fatal;
            targetLogLevel++)
        {
            for (var minimumOutputLogLevel = LogLevels.Debug;
                minimumOutputLogLevel <= LogLevels.Fatal;
                minimumOutputLogLevel++)
            {
                var lines = LogTestBlock(logger =>
                {
                    var value = 123;
                    logger.Log(targetLogLevel, $"AAA{value}BBB");
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
    public void LogIgnore2()
    {
        var lines = LogTestBlock(logger =>
        {
            var value = 123;
            logger.Log(LogLevels.Debug, $"AAA{value}BBB");
        },
        LogLevels.Ignore);

        Assert.AreEqual(0, lines.Length);
    }

    //////////////////////////////////////////////////////////

    private async LoggerAwaitable<LogEntry[]> QueryTestBlockAsync(
        Action<ILogger> action, int maximumLogEntries, Func<LogEntry, bool> predicate)
    {
        var basePath = Path.Combine(
            Path.GetDirectoryName(this.GetType().Assembly.Location)!,
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
                basePath, LogLevels.Debug))
            {
                var logger = logController.CreateLogger();

                action(logger);

                // Wait for flushing.
                await Task.Delay(500);

                return await logController.QueryLogEntriesAsync(
                    maximumLogEntries, predicate, default);
            }
        }
        finally
        {
            if (Directory.Exists(basePath))
            {
                Directory.Delete(basePath, true);
            }
        }
    }

    [Test]
    public async Task ReadMultipleMessage1()
    {
        var entries = await QueryTestBlockAsync(logger =>
        {
            var value1 = 123;
            logger.Debug($"AAA{value1}BBB");
            var value2 = 456;
            logger.Trace($"CCC{value2}DDD");
        },
        10,
        entry => entry.LogLevel == LogLevels.Debug);

        Assert.AreEqual("AAA123BBB", entries.Single().Message);
    }

    [Test]
    public async Task ReadMultipleMessage2()
    {
        var entries = await QueryTestBlockAsync(logger =>
        {
            var value1 = 123;
            logger.Debug($"AAA{value1}BBB");
            var value2 = 456;
            logger.Trace($"CCC{value2}DDD");
        },
        10,
        entry => entry.LogLevel == LogLevels.Trace);

        Assert.AreEqual("CCC456DDD", entries.Single().Message);
    }

    [Test]
    public async Task ReadMultipleMessage3()
    {
        var entries = await QueryTestBlockAsync(logger =>
        {
            var value1 = 123;
            logger.Debug($"AAA{value1}BBB");
            var value2 = 456;
            logger.Trace($"CCC{value2}DDD");
        },
        10,
        entry => true);

        Assert.AreEqual(2, entries.Length);
        Assert.AreEqual("AAA123BBB", entries[0].Message);
        Assert.AreEqual("CCC456DDD", entries[1].Message);
    }

    [Test]
    public async Task ReadMultipleMessage4()
    {
        var entries = await QueryTestBlockAsync(logger =>
        {
            var value1 = 123;
            logger.Debug($"AAA{value1}BBB");
            var value2 = 456;
            logger.Trace($"CCC{value2}DDD");
        },
        10,
        entry => false);

        Assert.AreEqual(0, entries.Length);
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(9)]
    [TestCase(10)]
    [TestCase(11)]
    public async Task ReadMultipleMessageWithLimit(int max)
    {
        var entries = await QueryTestBlockAsync(logger =>
        {
            for (var index = 0; index < 10; index++)
            {
                logger.Debug($"AAA{index}BBB");
                logger.Trace($"CCC{index}DDD");
            }
        },
        max,
        entry => entry.LogLevel == LogLevels.Trace);

        Assert.AreEqual(Math.Min(max, 10), entries.Length);
        Assert.IsTrue(entries.All(e => e.Message.StartsWith("CCC")));
    }

    //////////////////////////////////////////////////////////

    [TestCase(10, 1)]
    [TestCase(10, 2)]
    [TestCase(10, 5)]
    public void RotateLogFiles(long sizeToNextFile, int maximumLogFiles)
    {
        var lines = LogTestBlock(
            out var files,
            (_, logger) =>
            {
                for (var index = 0; index < 100; index++)
                {
                    logger.Debug($"AAA{index}BBB");
                }
            },
            LogLevels.Debug,
            sizeToNextFile,
            maximumLogFiles);

        Assert.AreEqual(maximumLogFiles, files);
        Assert.AreEqual(maximumLogFiles, lines.Length);
    }

    //////////////////////////////////////////////////////////

    [Test]
    public void SuspendAndResume()
    {
        var lines = LogTestBlock(
            out var files,
            (logController, logger) =>
            {
                for (var index = 0; index < 10000; index++)
                {
                    logger.Debug($"AAA{index}BBB");
                }
                Assert.AreNotEqual(0, logController.CurrentQueuedEntries);

                logController.Suspend();
                Assert.AreEqual(0, logController.CurrentQueuedEntries);

                for (var index = 0; index < 10000; index++)
                {
                    logger.Debug($"AAA{index}BBB");
                }
                Assert.AreEqual(0, logController.CurrentQueuedEntries);

                logController.Resume();

                for (var index = 0; index < 10000; index++)
                {
                    logger.Debug($"AAA{index}BBB");
                }
            });

        Assert.AreEqual(20000, lines.Length);
    }
}
