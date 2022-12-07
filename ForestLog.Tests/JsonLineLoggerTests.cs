﻿////////////////////////////////////////////////////////////////////////////
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

public sealed class JsonLineLoggerTests
{
    private IEnumerable<JObject?> LoadLines(string path)
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

    private JObject?[] LogTestBlock(Action<ILogger> action)
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
            using (var logController = LoggerFactory.CreateJsonLineLogController(
                basePath, LogLevels.Debug))
            {
                var logger = logController.CreateLogger();

                action(logger);
            }

            return Directory.EnumerateFiles(
                basePath, "log.jsonl", SearchOption.AllDirectories).
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

    [Test]
    public void DebugSingleMessage()
    {
        var lines = LogTestBlock(logger =>
        {
            var value = 123;
            logger.Debug($"AAA{value}BBB");
        });

        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
    }

    [Test]
    public void DebugMultipleMessage()
    {
        var lines = LogTestBlock(logger =>
        {
            var value1 = 123;
            logger.Debug($"AAA{value1}BBB");
            var value2 = 456;
            logger.Debug($"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("debug", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("debug", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    [Test]
    public void DebugException()
    {
        var lines = LogTestBlock(logger =>
        {
            try
            {
                throw new ApplicationException("AAA");
            }
            catch (Exception ex)
            {
                logger.Debug(ex);
            }
        });

        Assert.AreEqual("debug", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("System.ApplicationException: AAA", lines.Single()?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public void TraceSingleMessage()
    {
        var lines = LogTestBlock(logger =>
        {
            var value = 123;
            logger.Trace($"AAA{value}BBB");
        });

        Assert.AreEqual("trace", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
    }

    [Test]
    public void TraceMultipleMessage()
    {
        var lines = LogTestBlock(logger =>
        {
            var value1 = 123;
            logger.Trace($"AAA{value1}BBB");
            var value2 = 456;
            logger.Trace($"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("trace", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("trace", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    [Test]
    public void TraceException()
    {
        var lines = LogTestBlock(logger =>
        {
            try
            {
                throw new ApplicationException("AAA");
            }
            catch (Exception ex)
            {
                logger.Trace(ex);
            }
        });

        Assert.AreEqual("trace", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("System.ApplicationException: AAA", lines.Single()?["message"]?.ToString());
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
            using (var logController = LoggerFactory.CreateJsonLineLogController(
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
}
