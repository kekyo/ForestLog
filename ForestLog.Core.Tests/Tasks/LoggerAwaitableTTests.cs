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
using System.Diagnostics;
using System.Threading.Tasks;

namespace ForestLog.Tasks;

public sealed class LoggerAwaitableTTests
{
    [Test]
    public async Task WrappedFromValue()
    {
        var expected = 123;
        var awaitable = LoggerAwaitable.FromResult(expected);

        var actual = await awaitable;

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public async Task WrappedFromTaskValue()
    {
        var expected = 123;
        var awaitable = LoggerAwaitable.FromTask(Task.FromResult(expected));

        var actual = await awaitable;

        Assert.AreEqual(expected, actual);
    }

    private static async Task<T> DelayByTask<T>(T value)
    {
        await Task.Delay(100);
        return value;
    }

#if NETCOREAPP
    private static async ValueTask<T> DelayByValueTask<T>(T value)
    {
        await Task.Delay(100);
        return value;
    }
#endif
    [Test]
    public async Task SequentialAwaitings1()
    {
        var expected1 = 123;
        var actual1 = await LoggerAwaitable.FromTask(DelayByTask(expected1));

        Assert.AreEqual(expected1, actual1);

        var expected2 = 234;
        var actual2 = await LoggerAwaitable.FromResult(expected2);

        Assert.AreEqual(expected2, actual2);

        var expected3 = 345;
        var actual3 = await LoggerAwaitable.FromTask(DelayByTask(expected3));

        Assert.AreEqual(expected3, actual3);

        var expected4 = 456;
        var actual4 = await LoggerAwaitable.FromTask(Task.FromResult(expected4));

        Assert.AreEqual(expected4, actual4);
    }

    [Test]
    public async Task SequentialAwaitings2()
    {
        for (var index = 0; index < 10; index++)
        {
            var expected = 123 + index;
            var actual = await LoggerAwaitable.FromTask(DelayByTask(expected));

            Assert.AreEqual(expected, actual);
        }
    }

#if NETCOREAPP
    [Test]
    public async Task SequentialAwaitings3()
    {
        var expected1 = 123;
        var actual1 = await LoggerAwaitable.FromValueTask(DelayByValueTask(expected1));

        Assert.AreEqual(expected1, actual1);

        var expected2 = 234;
        var actual2 = await LoggerAwaitable.FromResult(expected2);

        Assert.AreEqual(expected2, actual2);

        var expected3 = 345;
        var actual3 = await LoggerAwaitable.FromValueTask(DelayByValueTask(expected3));

        Assert.AreEqual(expected3, actual3);

        var expected4 = 456;
        var actual4 = await LoggerAwaitable.FromValueTask(new ValueTask<int>(expected4));

        Assert.AreEqual(expected4, actual4);
    }

    [Test]
    public async Task SequentialAwaitings4()
    {
        for (var index = 0; index < 10; index++)
        {
            var expected = 123 + index;
            var actual = await LoggerAwaitable.FromValueTask(DelayByValueTask(expected));

            Assert.AreEqual(expected, actual);
        }
    }
#endif
}
