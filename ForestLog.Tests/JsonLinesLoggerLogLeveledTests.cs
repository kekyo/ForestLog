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

    //////////////////////////////////////////////////////////

    [Test]
    public void InformationSingleMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            var value = 123;
            logger.Information($"AAA{value}BBB");
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
            logger.Information($"AAA{value1}BBB");
            var value2 = 456;
            logger.Information($"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("information", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("information", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    [Test]
    public void InformationException()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            try
            {
                throw new ApplicationException("AAA");
            }
            catch (Exception ex)
            {
                logger.Information(ex);
            }
        });

        Assert.AreEqual("information", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("System.ApplicationException: AAA", lines.Single()?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public void WarningSingleMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            var value = 123;
            logger.Warning($"AAA{value}BBB");
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
            logger.Warning($"AAA{value1}BBB");
            var value2 = 456;
            logger.Warning($"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("warning", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("warning", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    [Test]
    public void WarningException()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            try
            {
                throw new ApplicationException("AAA");
            }
            catch (Exception ex)
            {
                logger.Warning(ex);
            }
        });

        Assert.AreEqual("warning", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("System.ApplicationException: AAA", lines.Single()?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public void ErrorSingleMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            var value = 123;
            logger.Error($"AAA{value}BBB");
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
            logger.Error($"AAA{value1}BBB");
            var value2 = 456;
            logger.Error($"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("error", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("error", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    [Test]
    public void ErrorException()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            try
            {
                throw new ApplicationException("AAA");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        });

        Assert.AreEqual("error", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("System.ApplicationException: AAA", lines.Single()?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public void FatalSingleMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            var value = 123;
            logger.Fatal($"AAA{value}BBB");
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
            logger.Fatal($"AAA{value1}BBB");
            var value2 = 456;
            logger.Fatal($"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("fatal", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("fatal", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    [Test]
    public void FatalException()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            try
            {
                throw new ApplicationException("AAA");
            }
            catch (Exception ex)
            {
                logger.Fatal(ex);
            }
        });

        Assert.AreEqual("fatal", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("System.ApplicationException: AAA", lines.Single()?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public void DebugScopedMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            using var childLogger = logger.DebugScope();

            var value = 123;
            childLogger.Warning($"AAA{value}BBB");
        });

        Assert.AreEqual(3, lines.Length);

        Assert.AreEqual("Enter: Parent=1", lines[0]?["message"]?.ToString());
        Assert.AreEqual("debug", lines[0]?["logLevel"]?.ToString());

        Assert.AreEqual("AAA123BBB", lines[1]?["message"]?.ToString());
        Assert.AreEqual(LogLevels.Warning.ToString().ToLowerInvariant(), lines[1]?["logLevel"]?.ToString());

        Assert.IsTrue(lines[2]?["message"]?.ToString().StartsWith("Leave: Elapsed="));
        Assert.AreEqual("debug", lines[2]?["logLevel"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public void TraceScopedMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            using var childLogger = logger.TraceScope();

            var value = 123;
            childLogger.Warning($"AAA{value}BBB");
        });

        Assert.AreEqual(3, lines.Length);

        Assert.AreEqual("Enter: Parent=1", lines[0]?["message"]?.ToString());
        Assert.AreEqual("trace", lines[0]?["logLevel"]?.ToString());

        Assert.AreEqual("AAA123BBB", lines[1]?["message"]?.ToString());
        Assert.AreEqual(LogLevels.Warning.ToString().ToLowerInvariant(), lines[1]?["logLevel"]?.ToString());

        Assert.IsTrue(lines[2]?["message"]?.ToString().StartsWith("Leave: Elapsed="));
        Assert.AreEqual("trace", lines[2]?["logLevel"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public void InformationScopedMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            using var childLogger = logger.InformationScope();

            var value = 123;
            childLogger.Warning($"AAA{value}BBB");
        });

        Assert.AreEqual(3, lines.Length);

        Assert.AreEqual("Enter: Parent=1", lines[0]?["message"]?.ToString());
        Assert.AreEqual("information", lines[0]?["logLevel"]?.ToString());

        Assert.AreEqual("AAA123BBB", lines[1]?["message"]?.ToString());
        Assert.AreEqual(LogLevels.Warning.ToString().ToLowerInvariant(), lines[1]?["logLevel"]?.ToString());

        Assert.IsTrue(lines[2]?["message"]?.ToString().StartsWith("Leave: Elapsed="));
        Assert.AreEqual("information", lines[2]?["logLevel"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public void WarningScopedMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            using var childLogger = logger.WarningScope();

            var value = 123;
            childLogger.Warning($"AAA{value}BBB");
        });

        Assert.AreEqual(3, lines.Length);

        Assert.AreEqual("Enter: Parent=1", lines[0]?["message"]?.ToString());
        Assert.AreEqual("warning", lines[0]?["logLevel"]?.ToString());

        Assert.AreEqual("AAA123BBB", lines[1]?["message"]?.ToString());
        Assert.AreEqual(LogLevels.Warning.ToString().ToLowerInvariant(), lines[1]?["logLevel"]?.ToString());

        Assert.IsTrue(lines[2]?["message"]?.ToString().StartsWith("Leave: Elapsed="));
        Assert.AreEqual("warning", lines[2]?["logLevel"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public void ErrorScopedMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            using var childLogger = logger.ErrorScope();

            var value = 123;
            childLogger.Warning($"AAA{value}BBB");
        });

        Assert.AreEqual(3, lines.Length);

        Assert.AreEqual("Enter: Parent=1", lines[0]?["message"]?.ToString());
        Assert.AreEqual("error", lines[0]?["logLevel"]?.ToString());

        Assert.AreEqual("AAA123BBB", lines[1]?["message"]?.ToString());
        Assert.AreEqual(LogLevels.Warning.ToString().ToLowerInvariant(), lines[1]?["logLevel"]?.ToString());

        Assert.IsTrue(lines[2]?["message"]?.ToString().StartsWith("Leave: Elapsed="));
        Assert.AreEqual("error", lines[2]?["logLevel"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public void FatalScopedMessage()
    {
        var lines = JsonLinesLoggerTests.LogTestBlock(logger =>
        {
            using var childLogger = logger.FatalScope();

            var value = 123;
            childLogger.Warning($"AAA{value}BBB");
        });

        Assert.AreEqual(3, lines.Length);

        Assert.AreEqual("Enter: Parent=1", lines[0]?["message"]?.ToString());
        Assert.AreEqual("fatal", lines[0]?["logLevel"]?.ToString());

        Assert.AreEqual("AAA123BBB", lines[1]?["message"]?.ToString());
        Assert.AreEqual(LogLevels.Warning.ToString().ToLowerInvariant(), lines[1]?["logLevel"]?.ToString());

        Assert.IsTrue(lines[2]?["message"]?.ToString().StartsWith("Leave: Elapsed="));
        Assert.AreEqual("fatal", lines[2]?["logLevel"]?.ToString());
    }
}
