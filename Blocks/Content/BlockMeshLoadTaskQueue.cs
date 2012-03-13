#region Using

using System;
using System.Collections.Generic;
using System.Threading;
using Willcraftia.Xna.Blocks.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    public sealed class BlockMeshLoadTaskQueue
    {
        readonly object syncRoot = new object();

        bool busy;

        Queue<BlockMeshLoadTask> queue;

        public BlockMeshLoadTaskQueue(int queueSize)
        {
            queue = new Queue<BlockMeshLoadTask>(queueSize);
        }

        public void Enqueue(BlockMeshLoadTask task)
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

        public void Enqueue(IBlockMeshLoader loader, string name, BlockMeshLoadTaskCallback callback)
        {
            if (loader == null) throw new ArgumentNullException("loader");
            if (name == null) throw new ArgumentNullException("name");

            var task = new BlockMeshLoadTask
            {
                Loader = loader,
                Name = name,
                Callback = callback
            };

            Enqueue(task);
        }

        void WaitCallback(object state)
        {
            var task = (BlockMeshLoadTask) state;
            task.LoadBlockMesh();

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
