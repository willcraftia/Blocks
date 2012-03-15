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

            public Block Block;

            public AsyncBlockMeshLoadCallback Callback;
        }

        readonly object queueLock = new object();

        Queue<Task> queue;

        ManualResetEvent beginEvent;

        ManualResetEvent suspendEvent;

        bool exit;

        Thread thread;

        bool enabled;

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

        public void Load(BlockMeshFactory factory, string name, Block block, AsyncBlockMeshLoadCallback callback)
        {
            if (exit) return;

            var task = new Task
            {
                Factory = factory,
                Name = name,
                Block = block,
                Callback = callback
            };

            lock (queueLock)
            {
                queue.Enqueue(task);
            }

            //beginEvent.Set();
        }

        public void Resume()
        {
            beginEvent.Set();
        }

        public void Suspend()
        {
            beginEvent.Reset();

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
                suspendEvent.Reset();

                lock (queueLock)
                {
                    if (0 < queue.Count)
                    {
                        var task = queue.Dequeue();
                        var mesh = task.Factory.CreateBlockMesh(task.Block);
                        task.Callback(task.Name, mesh);
                    }
                    else
                    {
                        beginEvent.Reset();
                    }
                }

                suspendEvent.Set();

                Thread.Sleep(1000);
            }
        }
    }
}
