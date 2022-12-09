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

public sealed class LoggerAwaitableMethodBuilderTests
{
    private static async LoggerAwaitable Return(TimeSpan delay = default)
    {
        if (delay == default)
        {
        }
        else
        {
            await Task.Delay(delay);
        }
    }

    [Test]
    public void ReturnImmediately()
    {
        static async Task RunnerAsync()
        {
            var tid = Thread.CurrentThread.ManagedThreadId;
            var awaitable = Return();

            await awaitable;

            Assert.AreEqual(tid, Thread.CurrentThread.ManagedThreadId);
        }

        var sc = new Application();
        sc.Run(RunnerAsync());
    }

    [Test]
    public void ReturnImmediatelyAsTask()
    {
        static async Task RunnerAsync()
        {
            var tid = Thread.CurrentThread.ManagedThreadId;
            var awaitable = (Task)Return();

            await awaitable;

            Assert.AreEqual(tid, Thread.CurrentThread.ManagedThreadId);
        }

        var sc = new Application();
        sc.Run(RunnerAsync());
    }

#if NETCOREAPP
    [Test]
    public void ReturnImmediatelyAsValueTask()
    {
        async Task RunnerAsync()
        {
            var tid = Thread.CurrentThread.ManagedThreadId;
            var awaitable = (ValueTask)Return();

            await awaitable;

            Assert.AreEqual(tid, Thread.CurrentThread.ManagedThreadId);
        }

        var sc = new Application();
        sc.Run(RunnerAsync());
    }
#endif

    //////////////////////////////////////////////////////////////////

    [Test]
    public void ReturnWithDelay1()
    {
        static async Task RunnerAsync()
        {
            var tid = Thread.CurrentThread.ManagedThreadId;

            var awaitable = Return(TimeSpan.FromMilliseconds(500));

            await awaitable;

            Assert.AreEqual(tid, Thread.CurrentThread.ManagedThreadId);
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
                var awaitable = Return(TimeSpan.FromMilliseconds((index / 3) * 100));

                await awaitable;
            }

            Assert.AreEqual(tid, Thread.CurrentThread.ManagedThreadId);
        }

        var sc = new Application();
        sc.Run(RunnerAsync());
    }

    [Test]
    public void ReturnWithDelayAsTask()
    {
        static async Task RunnerAsync()
        {
            var tid = Thread.CurrentThread.ManagedThreadId;

            var awaitable = (Task)Return(TimeSpan.FromMilliseconds(500));

            await awaitable;

            Assert.AreEqual(tid, Thread.CurrentThread.ManagedThreadId);
        }

        var sc = new Application();
        sc.Run(RunnerAsync());
    }

#if NETCOREAPP
    [Test]
    public void ReturnWithDelayAsValueTask()
    {
        static async Task RunnerAsync()
        {
            var tid = Thread.CurrentThread.ManagedThreadId;

            var awaitable = (ValueTask)Return(TimeSpan.FromMilliseconds(500));

            await awaitable;

            Assert.AreEqual(tid, Thread.CurrentThread.ManagedThreadId);
        }

        var sc = new Application();
        sc.Run(RunnerAsync());
    }
#endif

    //////////////////////////////////////////////////////////////////

    private static async LoggerAwaitable RaiseException(TimeSpan delay = default)
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
            var awaitable = RaiseException();

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

            var awaitable = RaiseException(TimeSpan.FromMilliseconds(500));

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
                var awaitable = RaiseException(TimeSpan.FromMilliseconds((index / 3) * 100));

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
        }

        var sc = new Application();

        Exception? cex = null;
        sc.UnhandledException += (s, e) =>
            cex = e.Exception;

        sc.Run(RunnerAsync());
    }

    //////////////////////////////////////////////////////////////////

    [Test]
    public async Task ThrowImmediatelyAsTask()
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        static async LoggerAwaitable ThrowAsync()
        {
            throw new ApplicationException();
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        var awaitable = (Task)ThrowAsync();

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
    public async Task ThrowImmediatelyAsValueTask()
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        static async LoggerAwaitable ThrowAsync()
        {
            throw new ApplicationException();
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        var awaitable = (ValueTask)ThrowAsync();

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

    [Test]
    public async Task ThrowDelayedAsTask()
    {
        static async LoggerAwaitable ThrowAsync()
        {
            await Task.Delay(500);
            throw new ApplicationException();
        }

        var awaitable = (Task)ThrowAsync();

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
    public async Task ThrowDelayedAsValueTask()
    {
        static async LoggerAwaitable ThrowAsync()
        {
            await Task.Delay(500);
            throw new ApplicationException();
        }

        var awaitable = (ValueTask)ThrowAsync();

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
