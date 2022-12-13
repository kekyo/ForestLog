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

public sealed class AsyncJsonLinesLoggerLogLeveledTests
{
    [Test]
    public async Task DebugSingleMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            var value = 123;
            await logger.DebugAsync($"AAA{value}BBB");
        });

        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
    }

    [Test]
    public async Task DebugMultipleMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
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
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
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
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
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
}
