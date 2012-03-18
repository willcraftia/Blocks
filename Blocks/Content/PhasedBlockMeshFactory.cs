#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Blocks.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    public sealed class PhasedBlockMeshFactory
    {
        enum LoadType
        {
            MeshEffect,
            MeshPart
        }

        struct Item
        {
            public LoadType LoadType;

            public InterBlockMesh InterBlockMesh;

            public BlockMesh BlockMesh;

            public int MeshEffectIndex;

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

        public PhasedBlockMeshFactory(int initialCapacity)
            : this(initialCapacity, int.MaxValue)
        {
        }

        public PhasedBlockMeshFactory(int initialCapacity, int maxCapacity)
        {
            MaxCapacity = maxCapacity;
            queue = new Queue<Item>(initialCapacity);
        }

        public BlockMesh LoadBlockMesh(GraphicsDevice graphicsDevice, InterBlockMesh interBlockMesh, IBlockEffectFactory blockEffectFactory)
        {
            var effectCount = interBlockMesh.Effects.Length;
            var lodCount = interBlockMesh.MeshLods.Length;

            var blockMesh = new BlockMesh();
            blockMesh.AllocateMeshEffects(effectCount);
            blockMesh.AllocateMeshLods(lodCount);

            for (int lod = 0; lod < lodCount; lod++)
            {
                var interMeshLod = interBlockMesh.MeshLods[lod];
                var meshPartCount = interMeshLod.MeshParts.Length;
                var meshLod = blockMesh.MeshLods[lod];

                meshLod.AllocateMeshParts(graphicsDevice, meshPartCount);

                for (int i = 0; i < meshPartCount; i++)
                {
                    var interMeshPart = interMeshLod.MeshParts[i];
                    var meshPart = meshLod.MeshParts[i];

                    meshPart.MeshEffect = blockMesh.MeshEffects[interMeshPart.EffectIndex];
                }
            }

            Enqueue(interBlockMesh, blockMesh, blockEffectFactory);

            return blockMesh;
        }

        void Enqueue(InterBlockMesh interBlockMesh, BlockMesh blockMesh, IBlockEffectFactory blockEffectFactory)
        {
            for (int i = 0; i < blockMesh.MeshEffects.Count; i++)
            {
                EnqueueLoadMeshEffect(interBlockMesh, blockMesh, i, blockEffectFactory, TimeSpan.FromMilliseconds(50));
            }
            for (int lod = 0; lod < blockMesh.MeshLods.Count; lod++)
            {
                for (int i = 0; i < blockMesh.MeshLods[lod].MeshParts.Count; i++)
                {
                    EnqueueLoadMeshPart(interBlockMesh, blockMesh, lod, i, TimeSpan.FromMilliseconds(50));
                }
            }
        }

        void EnqueueLoadMeshEffect(InterBlockMesh interBlockMesh, BlockMesh blockMesh, int effectIndex, IBlockEffectFactory blockEffectFactory, TimeSpan duration)
        {
            lock (this)
            {
                if (queue.Count == MaxCapacity)
                    throw new InvalidOperationException("This queue is full.");

                var item = new Item
                {
                    LoadType = LoadType.MeshEffect,
                    InterBlockMesh = interBlockMesh,
                    BlockMesh = blockMesh,
                    MeshEffectIndex = effectIndex,
                    BlockEffectFactory = blockEffectFactory,
                    Duration = duration
                };

                queue.Enqueue(item);
            }
        }

        void EnqueueLoadMeshPart(InterBlockMesh interBlockMesh, BlockMesh blockMesh, int levelOfDetail, int meshPartIndex, TimeSpan duration)
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

            //if (gameTime.IsRunningSlowly)
            //{
            //    delay += TimeSpan.FromMilliseconds(100);
            //    return;
            //}

            Item item;

            lock (this)
            {
                if (queue.Count == 0) return;

                item = queue.Dequeue();
            }

            Load(ref item);

            delay = item.Duration;
            lastUpdateTime = time;
        }

        void Load(ref Item item)
        {
            switch (item.LoadType)
            {
                case LoadType.MeshEffect:
                    LoadMeshEffect(ref item);
                    break;
                case LoadType.MeshPart:
                    LoadMeshPart(ref item);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        void LoadMeshEffect(ref Item item)
        {
            var interEffect = item.InterBlockMesh.Effects[item.MeshEffectIndex];
            var effect = item.BlockEffectFactory.CreateBlockEffect();

            effect.DiffuseColor = interEffect.DiffuseColor;
            effect.EmissiveColor = interEffect.EmissiveColor;
            effect.SpecularColor = interEffect.SpecularColor;
            effect.SpecularPower = interEffect.SpecularPower;

            var meshEffect = item.BlockMesh.MeshEffects[item.MeshEffectIndex];
            meshEffect.PopulateEffect(effect);
        }

        void LoadMeshPart(ref Item item)
        {
            var meshPart = item.BlockMesh.MeshLods[item.LevelOfDetail].MeshParts[item.MeshPartIndex];
            var interMeshPart = item.InterBlockMesh.MeshLods[item.LevelOfDetail].MeshParts[item.MeshPartIndex];

            meshPart.PopulateVertices(interMeshPart.Vertices);
            meshPart.PopulateIndices(interMeshPart.Indices);
        }
    }
}
