#region Using

using System;
using System.Collections.Generic;
using Willcraftia.Xna.Blocks.Graphics;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    public delegate void BlockMeshLoadQueueCallback(string name, BlockMesh mesh);

    public sealed class BlockMeshLoadQueue
    {
        struct Task
        {
            public string Name;

            public Block Block;

            public BlockMeshLoadQueueCallback Callback;
        }

        BlockMeshFactory factory;

        Queue<Task> queue;

        public int Count
        {
            get
            {
                lock (this)
                {
                    return queue.Count;
                }
            }
        }

        public BlockMeshLoadQueue(BlockMeshFactory factory, int queueSize)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            if (queueSize < 1) throw new ArgumentOutOfRangeException("queueSize");
            this.factory = factory;

            queue = new Queue<Task>(queueSize);
        }

        public void Enqueue(string name, Block block, BlockMeshLoadQueueCallback callback)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (block == null) throw new ArgumentNullException("block");
            if (callback == null) throw new ArgumentNullException("callback");

            lock (this)
            {
                var task = new Task
                {
                    Name = name,
                    Block = block,
                    Callback = callback
                };
                queue.Enqueue(task);
            }
        }

        public void Update()
        {
            lock (this)
            {
                if (0 < queue.Count)
                {
                    var task = queue.Dequeue();

                    var mesh = factory.CreateBlockMesh(task.Block);
                    task.Callback(task.Name, mesh);
                }
            }
        }
    }
}
