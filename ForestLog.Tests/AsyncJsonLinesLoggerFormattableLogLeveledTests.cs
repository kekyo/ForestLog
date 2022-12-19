////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ForestLog;

public sealed class AsyncJsonLinesLoggerFormattableLogLeveledTests
{
    [Test]
    public async Task DebugSingleMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            var value = 123;
            await logger.DebugAsync((IFormattable)$"AAA{value}BBB");
        });

        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
    }

    [Test]
    public async Task DebugMultipleMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            var value1 = 123;
            await logger.DebugAsync((IFormattable)$"AAA{value1}BBB");
            var value2 = 456;
            await logger.DebugAsync((IFormattable)$"CCC{value2}DDD");
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
            await logger.TraceAsync((IFormattable)$"AAA{value}BBB");
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
            await logger.TraceAsync((IFormattable)$"AAA{value1}BBB");
            var value2 = 456;
            await logger.TraceAsync((IFormattable)$"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("trace", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("trace", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public async Task InformationSingleMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            var value = 123;
            await logger.InformationAsync((IFormattable)$"AAA{value}BBB");
        });

        Assert.AreEqual("information", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
    }

    [Test]
    public async Task InformationMultipleMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            var value1 = 123;
            await logger.InformationAsync((IFormattable)$"AAA{value1}BBB");
            var value2 = 456;
            await logger.InformationAsync((IFormattable)$"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("information", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("information", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public async Task WarningSingleMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            var value = 123;
            await logger.WarningAsync((IFormattable)$"AAA{value}BBB");
        });

        Assert.AreEqual("warning", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
    }

    [Test]
    public async Task WarningMultipleMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            var value1 = 123;
            await logger.WarningAsync((IFormattable)$"AAA{value1}BBB");
            var value2 = 456;
            await logger.WarningAsync((IFormattable)$"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("warning", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("warning", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public async Task ErrorSingleMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            var value = 123;
            await logger.ErrorAsync((IFormattable)$"AAA{value}BBB");
        });

        Assert.AreEqual("error", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
    }

    [Test]
    public async Task ErrorMultipleMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            var value1 = 123;
            await logger.ErrorAsync((IFormattable)$"AAA{value1}BBB");
            var value2 = 456;
            await logger.ErrorAsync((IFormattable)$"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("error", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("error", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public async Task FatalSingleMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            var value = 123;
            await logger.FatalAsync((IFormattable)$"AAA{value}BBB");
        });

        Assert.AreEqual("fatal", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
    }

    [Test]
    public async Task FatalMultipleMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            var value1 = 123;
            await logger.FatalAsync((IFormattable)$"AAA{value1}BBB");
            var value2 = 456;
            await logger.FatalAsync((IFormattable)$"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("fatal", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("fatal", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }
}
