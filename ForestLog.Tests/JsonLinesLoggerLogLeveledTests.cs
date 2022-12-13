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

public sealed class JsonLinesLoggerLogLeveledTests
{
    [Test]
    public void DebugSingleMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            var value = 123;
            logger.Debug($"AAA{value}BBB");
        });

        Assert.AreEqual("AAA123BBB", lines.Single()?["message"]?.ToString());
    }

    [Test]
    public void DebugMultipleMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
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
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
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
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
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
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
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
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
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
}
