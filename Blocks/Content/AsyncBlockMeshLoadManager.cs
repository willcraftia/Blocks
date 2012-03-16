#region Using

using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Blocks.Graphics;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    public sealed class AsyncBlockMeshLoadManager : IAsyncBlockMeshLoadService
    {
        struct Task
        {
            public BlockMeshFactory Factory;

            public string Name;

            public InterBlockMesh InterMesh;

            public AsyncBlockMeshLoadCallback Callback;
        }

        readonly object queueLock = new object();

        readonly object suspendLock = new object();

        Queue<Task> queue;

        ManualResetEvent beginEvent;

        ManualResetEvent suspendEvent;

        bool exit;

        Thread thread;

        bool enabled;

        bool suspended;

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled == value) return;

                enabled = value;

                if (enabled)
                {
                    StartThread();
                }
                else
                {
                    StopThread();
                }
            }
        }

        public AsyncBlockMeshLoadManager(Game game)
        {
            queue = new Queue<Task>();

            beginEvent = new ManualResetEvent(true);
            suspendEvent = new ManualResetEvent(true);

            game.Services.AddService(typeof(IAsyncBlockMeshLoadService), this);
        }

        public void Load(BlockMeshFactory factory, string name, InterBlockMesh interMesh, AsyncBlockMeshLoadCallback callback)
        {
            if (exit) return;

            var task = new Task
            {
                Factory = factory,
                Name = name,
                InterMesh = interMesh,
                Callback = callback
            };

            lock (queueLock)
            {
                queue.Enqueue(task);
            }
        }

        public void Resume()
        {
            lock (suspendLock)
            {
                suspended = false;
            }

            suspendEvent.Reset();
            beginEvent.Set();
        }

        public void Suspend()
        {
            lock (suspendLock)
            {
                suspended = true;
            }

            suspendEvent.WaitOne();
        }

        void StartThread()
        {
            exit = false;
            thread = new Thread(Run);
            thread.Start();
        }

        void StopThread()
        {
            exit = true;
            beginEvent.Set();
            if (thread != null) thread.Join();
        }

        void Run()
        {
            while (beginEvent.WaitOne() && !exit)
            {
                lock (suspendLock)
                {
                    if (suspended)
                    {
                        beginEvent.Reset();
                        suspendEvent.Set();
                        continue;
                    }
                }

                lock (queueLock)
                {
                    if (0 < queue.Count)
                    {
                        var task = queue.Dequeue();
                        var mesh = task.Factory.Create(task.InterMesh);
                        task.Callback(task.Name, mesh);
                    }
                    else
                    {
                        // キューが空の場合は自発的に Suspend 状態なります。
                        lock (suspendLock)
                        {
                            suspended = true;
                        }
                    }
                }
            }
        }
    }
}
