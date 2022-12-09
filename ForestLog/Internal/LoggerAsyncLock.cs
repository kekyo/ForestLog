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

internal sealed class LoggerAsyncLock
{
    private readonly AsyncLockDisposer disposer;
    private readonly Queue<TaskCompletionSource<AsyncLockDisposer>> queue = new();
    private int count;

    public LoggerAsyncLock() =>
        disposer = new(this);

    public LoggerAwaitable<AsyncLockDisposer> LockAsync(CancellationToken ct)
    {
        var count = Interlocked.Increment(ref this.count);
        Debug.Assert(count >= 1);

        if (count == 1)
        {
            return disposer;
        }

        var tcs = new TaskCompletionSource<AsyncLockDisposer>();
        ct.Register(() => tcs.TrySetCanceled());

        lock (queue)
        {
            queue.Enqueue(tcs);
        }

        return tcs.Task;
    }

    public AsyncLockDisposer UnsafeLock()
    {
        var count = Interlocked.Increment(ref this.count);
        Debug.Assert(count >= 1);

        if (count == 1)
        {
            return disposer;
        }

        var tcs = new TaskCompletionSource<AsyncLockDisposer>();

        lock (queue)
        {
            queue.Enqueue(tcs);
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
                lock (queue)
                {
                    Debug.Assert(queue.Count >= 1);
                    var tcs = queue.Dequeue();
                    if (tcs.TrySetResult(disposer))
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
            parent.Unlock();

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        void IDisposable.Dispose() =>
            parent.Unlock();
    }
}
