////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using ForestLog.Internal;
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

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var jr = new JsonTextReader(new StringReader(line));
            yield return Utilities.JsonSerializer.Deserialize<JObject>(jr);
        }
    }

    public static async ValueTask<JObject?[]> LogTestBlockAsync(
        Func<ILogger, ValueTask> action,
        LogLevels maximumOutputLogLevel = LogLevels.Debug)
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
                basePath, maximumOutputLogLevel))
            {
                var logger = logController.CreateLogger();

                await action(logger);
            }

            return Directory.EnumerateFiles(
                basePath, "log*.jsonl", SearchOption.AllDirectories).
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
}
