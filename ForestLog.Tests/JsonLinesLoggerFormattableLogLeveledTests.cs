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

namespace ForestLog;

public sealed class JsonLinesLoggerFormattableLogLeveledTests
{
    [Test]
    public void DebugSingleMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            var value = 123;
            logger.Debug((IFormattable)$"AAA{value}BBB");
        });

        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
    }

    [Test]
    public void DebugMultipleMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            var value1 = 123;
            logger.Debug((IFormattable)$"AAA{value1}BBB");
            var value2 = 456;
            logger.Debug((IFormattable)$"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("debug", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("debug", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public void TraceSingleMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            var value = 123;
            logger.Trace((IFormattable)$"AAA{value}BBB");
        });

        Assert.AreEqual("trace", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
    }

    [Test]
    public void TraceMultipleMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            var value1 = 123;
            logger.Trace((IFormattable)$"AAA{value1}BBB");
            var value2 = 456;
            logger.Trace((IFormattable)$"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("trace", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("trace", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public void InformationSingleMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            var value = 123;
            logger.Information((IFormattable)$"AAA{value}BBB");
        });

        Assert.AreEqual("information", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
    }

    [Test]
    public void InformationMultipleMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            var value1 = 123;
            logger.Information((IFormattable)$"AAA{value1}BBB");
            var value2 = 456;
            logger.Information((IFormattable)$"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("information", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("information", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public void WarningSingleMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            var value = 123;
            logger.Warning((IFormattable)$"AAA{value}BBB");
        });

        Assert.AreEqual("warning", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
    }

    [Test]
    public void WarningMultipleMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            var value1 = 123;
            logger.Warning((IFormattable)$"AAA{value1}BBB");
            var value2 = 456;
            logger.Warning((IFormattable)$"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("warning", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("warning", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public void ErrorSingleMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            var value = 123;
            logger.Error((IFormattable)$"AAA{value}BBB");
        });

        Assert.AreEqual("error", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
    }

    [Test]
    public void ErrorMultipleMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            var value1 = 123;
            logger.Error((IFormattable)$"AAA{value1}BBB");
            var value2 = 456;
            logger.Error((IFormattable)$"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("error", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("error", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public void FatalSingleMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            var value = 123;
            logger.Fatal((IFormattable)$"AAA{value}BBB");
        });

        Assert.AreEqual("fatal", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
    }

    [Test]
    public void FatalMultipleMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            var value1 = 123;
            logger.Fatal((IFormattable)$"AAA{value1}BBB");
            var value2 = 456;
            logger.Fatal((IFormattable)$"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("fatal", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("fatal", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }
}
