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

public sealed class LoggerAwaitableTests
{
    [Test]
    public async Task WrappedFromDefaulted()
    {
        var awaitable = default(LoggerAwaitable);

        await awaitable;
    }

    [Test]
    public async Task WrappedFromTaskValue()
    {
        var awaitable = LoggerAwaitable.FromTask(Task.CompletedTask);

        await awaitable;
    }

    [Test]
    public async Task WrappedFromDelayedTask()
    {
        var expected = TimeSpan.FromMilliseconds(500);

        var sw = new Stopwatch();
        sw.Start();

        await LoggerAwaitable.FromTask(Task.Delay(expected));

        var actual = sw.Elapsed;
        Assert.IsTrue(actual >= expected);
    }

    //////////////////////////////////////////////////////////////////

    [Test]
    public async Task SequentialAwaitings1()
    {
        await LoggerAwaitable.FromTask(Task.Delay(100));

        var expected1 = 123;
        var actual1 = await LoggerAwaitable.FromResult(expected1);

        Assert.AreEqual(expected1, actual1);

        await LoggerAwaitable.FromTask(Task.Delay(100));

        var expected2 = 234;
        var actual2 = await LoggerAwaitable.FromTask(Task.FromResult(expected2));

        Assert.AreEqual(expected2, actual2);
    }

    [Test]
    public async Task SequentialAwaitings2()
    {
        for (var index = 0; index < 10; index++)
        {
            await LoggerAwaitable.FromTask(Task.Delay(100));
        }
    }

#if NETCOREAPP
    [Test]
    public async Task SequentialAwaitings3()
    {
        await LoggerAwaitable.FromTask(new ValueTask(Task.Delay(100)));

        var expected1 = 123;
        var actual1 = await LoggerAwaitable.FromResult(expected1);

        Assert.AreEqual(expected1, actual1);

        await LoggerAwaitable.FromTask(new ValueTask(Task.Delay(100)));

        var expected2 = 234;
        var actual2 = await LoggerAwaitable.FromTask(new ValueTask<int>(Task.FromResult(expected2)));

        Assert.AreEqual(expected2, actual2);
    }

    [Test]
    public async Task SequentialAwaitings4()
    {
        for (var index = 0; index < 10; index++)
        {
            await LoggerAwaitable.FromTask(new ValueTask(Task.Delay(100)));
        }
    }
#endif

    //////////////////////////////////////////////////////////////////

    [Test]
    public async Task ExceptionThrowed()
    {
        var awaitable = LoggerAwaitable.FromTask(Task.FromException(new ApplicationException()));

        try
        {
            await awaitable;
            Assert.Fail();
        }
        catch (ApplicationException)
        {
        }
    }

    [Test]
    public async Task ExceptionThrowedAfterAwaitedOnTask()
    {
        static async Task ThrowAfterDelay()
        {
            await Task.Delay(500);
            throw new ApplicationException();
        }

        var awaitable = LoggerAwaitable.FromTask(ThrowAfterDelay());

        try
        {
            await awaitable;
            Assert.Fail();
        }
        catch (ApplicationException)
        {
        }
    }

#if NETCOREAPP
    [Test]
    public async Task ExceptionThrowedAfterAwaitedOnValueTask()
    {
        static async ValueTask ThrowAfterDelay()
        {
            await Task.Delay(500);
            throw new ApplicationException();
        }

        var awaitable = LoggerAwaitable.FromTask(ThrowAfterDelay());

        try
        {
            await awaitable;
            Assert.Fail();
        }
        catch (ApplicationException)
        {
        }
    }
#endif
}
