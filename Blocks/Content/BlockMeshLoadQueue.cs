#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Blocks.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    public sealed class BlockMeshLoadQueue
    {
        enum LoadType
        {
            Effect,
            MeshPart
        }

        struct Item
        {
            public LoadType LoadType;

            public InterBlockMesh InterBlockMesh;

            public BlockMesh BlockMesh;

            public int EffectIndex;

            public IBlockEffectFactory BlockEffectFactory;

            public int LevelOfDetail;

            public int MeshPartIndex;

            public TimeSpan Duration;

            public bool Canceled;
        }

        Queue<Item> queue;

        TimeSpan delay = TimeSpan.Zero;

        TimeSpan lastUpdateTime = TimeSpan.Zero;

        public int MaxCapacity { get; private set; }

        public BlockMeshLoadQueue(int initialCapacity)
            : this(initialCapacity, int.MaxValue)
        {
        }

        public BlockMeshLoadQueue(int initialCapacity, int maxCapacity)
        {
            MaxCapacity = maxCapacity;
            queue = new Queue<Item>(initialCapacity);
        }

        public void EnqueueLoadEffect(InterBlockMesh interBlockMesh, BlockMesh blockMesh, int effectIndex, IBlockEffectFactory blockEffectFactory, TimeSpan duration)
        {
            lock (this)
            {
                if (queue.Count == MaxCapacity)
                    throw new InvalidOperationException("This queue is full.");

                var item = new Item
                {
                    LoadType = LoadType.Effect,
                    InterBlockMesh = interBlockMesh,
                    BlockMesh = blockMesh,
                    EffectIndex = effectIndex,
                    BlockEffectFactory = blockEffectFactory,
                    Duration = duration
                };

                queue.Enqueue(item);
            }
        }

        public void EnqueueLoadMeshPart(InterBlockMesh interBlockMesh, BlockMesh blockMesh, int levelOfDetail, int meshPartIndex, TimeSpan duration)
        {
            lock (this)
            {
                if (queue.Count == MaxCapacity)
                    throw new InvalidOperationException("This queue is full.");

                var item = new Item
                {
                    LoadType = LoadType.MeshPart,
                    InterBlockMesh = interBlockMesh,
                    BlockMesh = blockMesh,
                    LevelOfDetail = levelOfDetail,
                    MeshPartIndex = meshPartIndex,
                    Duration = duration
                };

                queue.Enqueue(item);
            }
        }

        public void Update(GameTime gameTime)
        {
            var time = gameTime.TotalGameTime;
            if (time < lastUpdateTime + delay) return;

            if (gameTime.IsRunningSlowly)
            {
                delay += TimeSpan.FromMilliseconds(1000);
                return;
            }

            Item item;

            lock (this)
            {
                if (queue.Count == 0) return;

                item = queue.Dequeue();
            }

            //item.Update(gameTime);

            delay = item.Duration;
            lastUpdateTime = time;
        }

        void LoadEffect(ref Item item)
        {
            var effect = item.BlockEffectFactory.CreateBlockEffect();

            // TODO
        }

        void LoadMeshPart(ref Item item)
        {
            var meshPart = item.BlockMesh.MeshLods[item.LevelOfDetail].MeshParts[item.MeshPartIndex];

            // TODO
        }
    }
}
