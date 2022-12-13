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

    [Test]
    public async Task DebugException()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            try
            {
                throw new ApplicationException("AAA");
            }
            catch (Exception ex)
            {
                await logger.DebugAsync(ex);
            }
        });

        Assert.AreEqual("debug", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("System.ApplicationException: AAA", lines.Single()?["message"]?.ToString());
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

    [Test]
    public async Task TraceException()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            try
            {
                throw new ApplicationException("AAA");
            }
            catch (Exception ex)
            {
                await logger.TraceAsync(ex);
            }
        });

        Assert.AreEqual("trace", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("System.ApplicationException: AAA", lines.Single()?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public async Task InformationSingleMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            var value = 123;
            await logger.InformationAsync($"AAA{value}BBB");
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
            await logger.InformationAsync($"AAA{value1}BBB");
            var value2 = 456;
            await logger.InformationAsync($"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("information", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("information", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    [Test]
    public async Task InformationException()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            try
            {
                throw new ApplicationException("AAA");
            }
            catch (Exception ex)
            {
                await logger.InformationAsync(ex);
            }
        });

        Assert.AreEqual("information", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("System.ApplicationException: AAA", lines.Single()?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public async Task WarningSingleMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            var value = 123;
            await logger.WarningAsync($"AAA{value}BBB");
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
            await logger.WarningAsync($"AAA{value1}BBB");
            var value2 = 456;
            await logger.WarningAsync($"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("warning", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("warning", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    [Test]
    public async Task WarningException()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            try
            {
                throw new ApplicationException("AAA");
            }
            catch (Exception ex)
            {
                await logger.WarningAsync(ex);
            }
        });

        Assert.AreEqual("warning", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("System.ApplicationException: AAA", lines.Single()?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public async Task ErrorSingleMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            var value = 123;
            await logger.ErrorAsync($"AAA{value}BBB");
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
            await logger.ErrorAsync($"AAA{value1}BBB");
            var value2 = 456;
            await logger.ErrorAsync($"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("error", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("error", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    [Test]
    public async Task ErrorException()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            try
            {
                throw new ApplicationException("AAA");
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync(ex);
            }
        });

        Assert.AreEqual("error", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("System.ApplicationException: AAA", lines.Single()?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public async Task FatalSingleMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            var value = 123;
            await logger.FatalAsync($"AAA{value}BBB");
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
            await logger.FatalAsync($"AAA{value1}BBB");
            var value2 = 456;
            await logger.FatalAsync($"CCC{value2}DDD");
        });

        Assert.AreEqual(2, lines.Length);
        Assert.AreEqual("fatal", lines[0]?["logLevel"]?.ToString());
        Assert.AreEqual("AAA123BBB", lines[0]?["message"]?.ToString());
        Assert.AreEqual("fatal", lines[1]?["logLevel"]?.ToString());
        Assert.AreEqual("CCC456DDD", lines[1]?["message"]?.ToString());
    }

    [Test]
    public async Task FatalException()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            try
            {
                throw new ApplicationException("AAA");
            }
            catch (Exception ex)
            {
                await logger.FatalAsync(ex);
            }
        });

        Assert.AreEqual("fatal", lines.Single()?["logLevel"]?.ToString());
        Assert.AreEqual("System.ApplicationException: AAA", lines.Single()?["message"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public async Task DebugScopedMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            using var childLogger = await logger.DebugScopeAsync();

            var value = 123;
            await logger.WarningAsync($"AAA{value}BBB");
        });

        Assert.AreEqual(3, lines.Length);

        Assert.AreEqual("Enter: Parent=2", lines[0]?["message"]?.ToString());
        Assert.AreEqual("debug", lines[0]?["logLevel"]?.ToString());

        Assert.AreEqual("AAA123BBB", lines[1]?["message"]?.ToString());
        Assert.AreEqual(LogLevels.Warning.ToString().ToLowerInvariant(), lines[1]?["logLevel"]?.ToString());

        Assert.IsTrue(lines[2]?["message"]?.ToString().StartsWith("Leave: Elapsed="));
        Assert.AreEqual("debug", lines[2]?["logLevel"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public async Task TraceScopedMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            using var childLogger = await logger.TraceScopeAsync();

            var value = 123;
            await logger.WarningAsync($"AAA{value}BBB");
        });

        Assert.AreEqual(3, lines.Length);

        Assert.AreEqual("Enter: Parent=2", lines[0]?["message"]?.ToString());
        Assert.AreEqual("trace", lines[0]?["logLevel"]?.ToString());

        Assert.AreEqual("AAA123BBB", lines[1]?["message"]?.ToString());
        Assert.AreEqual(LogLevels.Warning.ToString().ToLowerInvariant(), lines[1]?["logLevel"]?.ToString());

        Assert.IsTrue(lines[2]?["message"]?.ToString().StartsWith("Leave: Elapsed="));
        Assert.AreEqual("trace", lines[2]?["logLevel"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public async Task InformationScopedMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            using var childLogger = await logger.InformationScopeAsync();

            var value = 123;
            await logger.WarningAsync($"AAA{value}BBB");
        });

        Assert.AreEqual(3, lines.Length);

        Assert.AreEqual("Enter: Parent=2", lines[0]?["message"]?.ToString());
        Assert.AreEqual("information", lines[0]?["logLevel"]?.ToString());

        Assert.AreEqual("AAA123BBB", lines[1]?["message"]?.ToString());
        Assert.AreEqual(LogLevels.Warning.ToString().ToLowerInvariant(), lines[1]?["logLevel"]?.ToString());

        Assert.IsTrue(lines[2]?["message"]?.ToString().StartsWith("Leave: Elapsed="));
        Assert.AreEqual("information", lines[2]?["logLevel"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public async Task WarningScopedMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            using var childLogger = await logger.WarningScopeAsync();

            var value = 123;
            await logger.WarningAsync($"AAA{value}BBB");
        });

        Assert.AreEqual(3, lines.Length);

        Assert.AreEqual("Enter: Parent=2", lines[0]?["message"]?.ToString());
        Assert.AreEqual("warning", lines[0]?["logLevel"]?.ToString());

        Assert.AreEqual("AAA123BBB", lines[1]?["message"]?.ToString());
        Assert.AreEqual(LogLevels.Warning.ToString().ToLowerInvariant(), lines[1]?["logLevel"]?.ToString());

        Assert.IsTrue(lines[2]?["message"]?.ToString().StartsWith("Leave: Elapsed="));
        Assert.AreEqual("warning", lines[2]?["logLevel"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public async Task ErrorScopedMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            using var childLogger = await logger.ErrorScopeAsync();

            var value = 123;
            await logger.WarningAsync($"AAA{value}BBB");
        });

        Assert.AreEqual(3, lines.Length);

        Assert.AreEqual("Enter: Parent=2", lines[0]?["message"]?.ToString());
        Assert.AreEqual("error", lines[0]?["logLevel"]?.ToString());

        Assert.AreEqual("AAA123BBB", lines[1]?["message"]?.ToString());
        Assert.AreEqual(LogLevels.Warning.ToString().ToLowerInvariant(), lines[1]?["logLevel"]?.ToString());

        Assert.IsTrue(lines[2]?["message"]?.ToString().StartsWith("Leave: Elapsed="));
        Assert.AreEqual("error", lines[2]?["logLevel"]?.ToString());
    }

    //////////////////////////////////////////////////////////

    [Test]
    public async Task FatalScopedMessage()
    {
        var lines = await AsyncJsonLinesLoggerTests.LogTestBlockAsync(async logger =>
        {
            using var childLogger = await logger.FatalScopeAsync();

            var value = 123;
            await logger.WarningAsync($"AAA{value}BBB");
        });

        Assert.AreEqual(3, lines.Length);

        Assert.AreEqual("Enter: Parent=2", lines[0]?["message"]?.ToString());
        Assert.AreEqual("fatal", lines[0]?["logLevel"]?.ToString());

        Assert.AreEqual("AAA123BBB", lines[1]?["message"]?.ToString());
        Assert.AreEqual(LogLevels.Warning.ToString().ToLowerInvariant(), lines[1]?["logLevel"]?.ToString());

        Assert.IsTrue(lines[2]?["message"]?.ToString().StartsWith("Leave: Elapsed="));
        Assert.AreEqual("fatal", lines[2]?["logLevel"]?.ToString());
    }
}
