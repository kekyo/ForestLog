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

internal sealed class AsyncLock
{
    private readonly Disposer disposer;
    private readonly Queue<TaskCompletionSource<Disposer>> queue = new();
    private int count;

    public AsyncLock() =>
        this.disposer = new(this);

    public LoggerAwaitable<Disposer> LockAsync(CancellationToken ct)
    {
        var count = Interlocked.Increment(ref this.count);
        Debug.Assert(count >= 1);

        if (count == 1)
        {
            return this.disposer;
        }

        var tcs = new TaskCompletionSource<Disposer>();
        ct.Register(() => tcs.TrySetCanceled());

        lock (this.queue)
        {
            this.queue.Enqueue(tcs);
        }

        return tcs.Task;
    }

    public IDisposable UnsafeLock()
    {
        var count = Interlocked.Increment(ref this.count);
        Debug.Assert(count >= 1);

        if (count == 1)
        {
            return this.disposer;
        }

        var tcs = new TaskCompletionSource<Disposer>();

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

    public sealed class Disposer : IDisposable
    {
        private readonly AsyncLock parent;

        internal Disposer(AsyncLock parent) =>
            this.parent = parent;

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Dispose() =>
            this.parent.Unlock();
    }
}
