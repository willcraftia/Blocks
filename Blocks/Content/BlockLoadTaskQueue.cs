#region Using

using System;
using System.Collections.Generic;
using System.Threading;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    public sealed class BlockLoadTaskQueue
    {
        readonly object syncRoot = new object();

        bool busy;

        Queue<BlockLoadTask> queue;

        public BlockLoadTaskQueue(int queueSize)
        {
            queue = new Queue<BlockLoadTask>(queueSize);
        }

        public void Enqueue(BlockLoadTask task)
        {
            lock (syncRoot)
            {
                if (busy)
                {
                    queue.Enqueue(task);
                }
                else
                {
                    busy = true;
                    ThreadPool.QueueUserWorkItem(WaitCallback, task);
                }
            }
        }

        public void Enqueue(IBlockLoader loader, string name, BlockLoadTaskCallback callback)
        {
            if (loader == null) throw new ArgumentNullException("loader");
            if (name == null) throw new ArgumentNullException("name");

            var task = new BlockLoadTask
            {
                Loader = loader,
                Name = name,
                Callback = callback
            };

            Enqueue(task);
        }

        void WaitCallback(object state)
        {
            var task = (BlockLoadTask) state;
            task.LoadBlock();

            NextAction();
        }

        void NextAction()
        {
            lock (syncRoot)
            {
                if (0 < queue.Count)
                {
                    var task = queue.Dequeue();
                    ThreadPool.QueueUserWorkItem(WaitCallback, task);
                }
                else
                {
                    busy = false;
                }
            }
        }
    }
}
