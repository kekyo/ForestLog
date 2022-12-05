////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using Lepracaun;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ForestLog.Tasks;

public sealed class LoggerAwaitableMethodBuilderTTests
{
    private static async LoggerAwaitable<T> Return<T>(T value, TimeSpan delay = default)
    {
        if (delay == default)
        {
            return value;
        }
        else
        {
            await Task.Delay(delay);
            return value;
        }
    }

    [Test]
    public void ReturnImmediately()
    {
        static async Task RunnerAsync()
        {
            var tid = Thread.CurrentThread.ManagedThreadId;
            var expected = 123;
            var awaitable = Return(expected);

            var actual = await awaitable;

            Assert.AreEqual(tid, Thread.CurrentThread.ManagedThreadId);
            Assert.AreEqual(expected, actual);
        }

        var sc = new Application();
        sc.Run(RunnerAsync());
    }

    [Test]
    public void ReturnWithDelay1()
    {
        static async Task RunnerAsync()
        {
            var tid = Thread.CurrentThread.ManagedThreadId;

            var expected = 123;
            var awaitable = Return(expected, TimeSpan.FromMilliseconds(500));

            var actual = await awaitable;

            Assert.AreEqual(tid, Thread.CurrentThread.ManagedThreadId);
            Assert.AreEqual(expected, actual);
        }

        var sc = new Application();
        sc.Run(RunnerAsync());
    }

    [Test]
    public void ReturnWithDelay2()
    {
        static async Task RunnerAsync()
        {
            var tid = Thread.CurrentThread.ManagedThreadId;

            for (var index = 0; index < 10; index++)
            {
                var expected = 123 + index;
                var awaitable = Return(expected, TimeSpan.FromMilliseconds((index / 3) * 100));

                var actual = await awaitable;

                Assert.AreEqual(expected, actual);
            }

            Assert.AreEqual(tid, Thread.CurrentThread.ManagedThreadId);
        }

        var sc = new Application();
        sc.Run(RunnerAsync());
    }

    //////////////////////////////////////////////////////////////////

    private static async LoggerAwaitable<T> RaiseException<T>(TimeSpan delay = default)
    {
        if (delay == default)
        {
        }
        else
        {
            await Task.Delay(delay);
        }

        throw new ApplicationException();
    }

    [Test]
    public void ExceptionImmediately()
    {
        static async Task RunnerAsync()
        {
            var tid = Thread.CurrentThread.ManagedThreadId;
            var awaitable = RaiseException<int>();

            try
            {
                await awaitable;
                Assert.Fail();
            }
            catch (ApplicationException)
            {
                Assert.AreEqual(tid, Thread.CurrentThread.ManagedThreadId);
            }
        }

        var sc = new Application();
        sc.Run(RunnerAsync());
    }

    [Test]
    public void RaiseExceptionWithDelay1()
    {
        static async Task RunnerAsync()
        {
            var tid = Thread.CurrentThread.ManagedThreadId;

            var awaitable = RaiseException<int>(TimeSpan.FromMilliseconds(500));

            try
            {
                await awaitable;
                Assert.Fail();
            }
            catch (ApplicationException)
            {
                Assert.AreEqual(tid, Thread.CurrentThread.ManagedThreadId);
            }
        }

        var sc = new Application();
        sc.Run(RunnerAsync());
    }

    [Test]
    public void RaiseExceptionWithDelay2()
    {
        static async Task RunnerAsync()
        {
            var tid = Thread.CurrentThread.ManagedThreadId;

            for (var index = 0; index < 10; index++)
            {
                var awaitable = RaiseException<int>(TimeSpan.FromMilliseconds((index / 3) * 100));

                try
                {
                    Assert.Fail();
                    await awaitable;
                }
                catch (ApplicationException)
                {
                    Assert.AreEqual(tid, Thread.CurrentThread.ManagedThreadId);
                }
            }
        }

        var sc = new Application();

        Exception? cex = null;
        sc.UnhandledException += (s, e) =>
            cex = e.Exception;

        sc.Run(RunnerAsync());
    }
}
