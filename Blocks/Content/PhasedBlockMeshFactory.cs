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
        struct Item
        {
            public InterBlockMesh InterBlockMesh;

            public BlockMesh BlockMesh;

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

        public BlockMesh LoadBlockMesh(GraphicsDevice graphicsDevice, InterBlockMesh interBlockMesh)
        {
            var lodCount = interBlockMesh.MeshLods.Length;

            var blockMesh = new BlockMesh();
            blockMesh.SetMeshMaterials(interBlockMesh.MeshMaterials);
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

                    meshPart.MeshMaterial = blockMesh.MeshMaterials[interMeshPart.MeshMaterialIndex];
                }
            }

            Enqueue(interBlockMesh, blockMesh);

            return blockMesh;
        }

        void Enqueue(InterBlockMesh interBlockMesh, BlockMesh blockMesh)
        {
            // 最も荒い LOD から BlockMeshPart のロード要求をキューに入れます。
            for (int lod = blockMesh.MeshLods.Count - 1; 0 <= lod; lod--)
            {
                for (int i = 0; i < blockMesh.MeshLods[lod].MeshParts.Count; i++)
                {
                    EnqueueLoadMeshPart(interBlockMesh, blockMesh, lod, i, TimeSpan.FromMilliseconds(0));
                }
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
                delay += TimeSpan.FromMilliseconds(100);
                return;
            }

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
            var meshPart = item.BlockMesh.MeshLods[item.LevelOfDetail].MeshParts[item.MeshPartIndex];
            var interMeshPart = item.InterBlockMesh.MeshLods[item.LevelOfDetail].MeshParts[item.MeshPartIndex];

            meshPart.PopulateVertices(interMeshPart.Vertices);
            meshPart.PopulateIndices(interMeshPart.Indices);
        }
    }
}
