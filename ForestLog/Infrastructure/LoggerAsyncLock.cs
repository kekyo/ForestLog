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

namespace ForestLog.Infrastructure;

internal sealed class LoggerAsyncLock
{
    private readonly AsyncLockDisposer disposer;
    private readonly Queue<TaskCompletionSource<AsyncLockDisposer>> queue = new();
    private int count;

    public LoggerAsyncLock() =>
        this.disposer = new(this);

    public LoggerAwaitable<AsyncLockDisposer> LockAsync(CancellationToken ct)
    {
        var count = Interlocked.Increment(ref this.count);
        Debug.Assert(count >= 1);

        if (count == 1)
        {
            return this.disposer;
        }

        var tcs = new TaskCompletionSource<AsyncLockDisposer>();
        ct.Register(() => tcs.TrySetCanceled());

        lock (this.queue)
        {
            this.queue.Enqueue(tcs);
        }

        return tcs.Task;
    }

    public AsyncLockDisposer UnsafeLock()
    {
        var count = Interlocked.Increment(ref this.count);
        Debug.Assert(count >= 1);

        if (count == 1)
        {
            return this.disposer;
        }

        var tcs = new TaskCompletionSource<AsyncLockDisposer>();

        lock (this.queue)
        {
            this.queue.Enqueue(tcs);
        }

        return tcs.Task.Result;
    }

    private void Unlock()
    {
        while (true)
        {
            var count = Interlocked.Decrement(ref this.count);
            Debug.Assert(count >= 0);

            if (count == 0)
            {
                break;
            }
            else if (count >= 1)
            {
                lock (this.queue)
                {
                    Debug.Assert(this.queue.Count >= 1);
                    var tcs = this.queue.Dequeue();
                    if (tcs.TrySetResult(this.disposer))
                    {
                        break;
                    }
                }
            }
        }
    }

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
