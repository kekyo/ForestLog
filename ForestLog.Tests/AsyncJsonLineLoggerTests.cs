﻿////////////////////////////////////////////////////////////////////////////
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
using System.Threading.Tasks;

namespace ForestLog;

public sealed class AsyncJsonLineLoggerTests
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

    private async ValueTask<JObject?[]> LogTestBlockAsync(Func<ILogger, ValueTask> action)
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

                await action(logger);
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
    public async Task DebugSingleMessage()
    {
        var lines = await LogTestBlockAsync(async logger =>
        {
            var value = 123;
            await logger.DebugAsync($"AAA{value}BBB");
        });

        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
    }

    [Test]
    public async Task DebugMultipleMessage()
    {
        var lines = await LogTestBlockAsync(async logger =>
        {
            var value1 = 123;
            await logger.DebugAsync($"AAA{value1}BBB");
            var value2 = 456;
            await logger.DebugAsync($"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("debug", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("debug", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public async Task TraceSingleMessage()
    {
        var lines = await LogTestBlockAsync(async logger =>
        {
            var value = 123;
            await logger.TraceAsync($"AAA{value}BBB");
        });

        Assert.AreEqual("trace", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
    }

    [Test]
    public async Task TraceMultipleMessage()
    {
        var lines = await LogTestBlockAsync(async logger =>
        {
            var value1 = 123;
            await logger.TraceAsync($"AAA{value1}BBB");
            var value2 = 456;
            await logger.TraceAsync($"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("trace", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("trace", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////
}
