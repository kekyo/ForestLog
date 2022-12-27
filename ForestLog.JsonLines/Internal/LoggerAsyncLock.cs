////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ForestLog.Tasks;

namespace ForestLog.Internal;

[DebuggerStepThrough]
internal sealed class LoggerAsyncLock
{
    [DebuggerStepThrough]
    private sealed class Waiter : IDisposable
    {
        private readonly TaskCompletionSource<AsyncLockDisposer> tcs = new();
        private readonly CancellationTokenRegistration ctr;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public Waiter()
        {
        }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public Waiter(CancellationToken ct) =>
            this.ctr = ct.Register(() => tcs.TrySetCanceled());

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Dispose() =>
            this.ctr.Dispose();

        public Task<AsyncLockDisposer> Task =>
            this.tcs.Task;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public bool TrySetResult(AsyncLockDisposer disposer) =>
            this.tcs.TrySetResult(disposer);
    }

    private readonly AsyncLockDisposer disposer;
    private readonly Queue<Waiter> queue = new();
    private bool isRunning;

    public LoggerAsyncLock() =>
        this.disposer = new(this);

    public LoggerAwaitable<AsyncLockDisposer> LockAsync(CancellationToken ct)
    {
        lock (this.queue)
        {
            if (!this.isRunning)
            {
                this.isRunning = true;
                return this.disposer;
            }
        }

        var waiter = new Waiter(ct);

        lock (this.queue)
        {
            if (!this.isRunning)
            {
                this.isRunning = true;
                waiter.Dispose();
                return this.disposer;
            }

            this.queue.Enqueue(waiter);
        }

        return waiter.Task;
    }

    public AsyncLockDisposer UnsafeLock()
    {
        lock (this.queue)
        {
            if (!this.isRunning)
            {
                this.isRunning = true;
                return this.disposer;
            }
        }

        var waiter = new Waiter();

        lock (this.queue)
        {
            if (!this.isRunning)
            {
                this.isRunning = true;
                return this.disposer;
            }

            this.queue.Enqueue(waiter);
        }

        return waiter.Task.GetAwaiter().GetResult();
    }

    private void Unlock()
    {
        while (true)
        {
            lock (this.queue)
            {
                if (this.queue.Count == 0)
                {
                    this.isRunning = false;
                    break;
                }

                using var waiter = this.queue.Dequeue();
                if (waiter.TrySetResult(this.disposer))
                {
                    break;
                }
            }
        }
    }

    [DebuggerStepThrough]
    public readonly struct AsyncLockDisposer : IDisposable
    {
        private readonly LoggerAsyncLock parent;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        internal AsyncLockDisposer(LoggerAsyncLock parent) =>
            this.parent = parent;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Dispose() =>
            this.parent.Unlock();

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        void IDisposable.Dispose() =>
            this.parent.Unlock();
    }
}
