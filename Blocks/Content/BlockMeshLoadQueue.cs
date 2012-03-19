#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Blocks.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// BlockMesh が管理する BlockMeshPart を分割してロードするクラスです。
    /// </summary>
    public sealed class BlockMeshLoadQueue
    {
        #region Item

        /// <summary>
        /// BlockMeshPart のロード要求を表す構造体です。
        /// </summary>
        struct Item
        {
            /// <summary>
            /// BlockMesh のロード元となる InterBlockMesh。
            /// </summary>
            public InterBlockMesh InterBlockMesh;

            /// <summary>
            /// ロードされる BlockMesh。
            /// </summary>
            public BlockMesh BlockMesh;

            /// <summary>
            /// このロード要求で対象とする
            /// </summary>
            public int LevelOfDetail;

            public int MeshPartIndex;

            public TimeSpan Duration;

            public bool Canceled;
        }

        #endregion

        /// <summary>
        /// Item のキュー。
        /// </summary>
        Queue<Item> queue;

        /// <summary>
        /// 次の Item を処理するまでの待機時間。
        /// </summary>
        TimeSpan delay = TimeSpan.Zero;

        /// <summary>
        /// 最後に Upadte(GameTime) メソッド内で Item を処理した時間。
        /// </summary>
        TimeSpan lastUpdateTime = TimeSpan.Zero;

        /// <summary>
        /// キューの最大サイズを取得します。
        /// </summary>
        public int MaxCapacity { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// キューの最大サイズは int.MaxValue に設定されます。
        /// </summary>
        /// <param name="initialCapacity">キューの初期サイズ。</param>
        public BlockMeshLoadQueue(int initialCapacity)
            : this(initialCapacity, int.MaxValue)
        {
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="initialCapacity">キューの初期サイズ。</param>
        /// <param name="maxCapacity">キューの最大サイズ。</param>
        public BlockMeshLoadQueue(int initialCapacity, int maxCapacity)
        {
            MaxCapacity = maxCapacity;
            queue = new Queue<Item>(initialCapacity);
        }

        /// <summary>
        /// BlockMesh の分割ロード要求を追加します。
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice。</param>
        /// <param name="interBlockMesh">BlockMesh のロード元となる InterBlockMesh。</param>
        /// <returns>分割ロード対応の BlockMesh。</returns>
        public BlockMesh Load(GraphicsDevice graphicsDevice, InterBlockMesh interBlockMesh)
        {
            // 分割ロード対応の BlockMesh を生成します。
            var blockMesh = CreatePhasedBlockMesh(graphicsDevice, interBlockMesh);

            // ロード要求をキューに追加します。
            Enqueue(interBlockMesh, blockMesh);
            //var coarsestLod = blockMesh.MeshLods.Count - 1;
            //for (int i = 0; i < blockMesh.MeshLods[coarsestLod].MeshParts.Count; i++)
            //{
            //    Enqueue(interBlockMesh, blockMesh, coarsestLod, i, TimeSpan.FromMilliseconds(0));
            //}

            // 分割ロード対応の BlockMesh を要求元へ返します。
            return blockMesh;
        }

        /// <summary>
        /// 分割ロード対応の BlockMesh を生成します。
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice。</param>
        /// <param name="interBlockMesh">BlockMesh のロード元となる InterBlockMesh。</param>
        /// <returns>分割ロード対応の BlockMesh。</returns>
        BlockMesh CreatePhasedBlockMesh(GraphicsDevice graphicsDevice, InterBlockMesh interBlockMesh)
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

            return blockMesh;
        }

        /// <summary>
        /// 分割ロード要求をキューへ追加します。
        /// </summary>
        /// <param name="interBlockMesh">BlockMesh のロード元となる InterBlockMesh。</param>
        /// <param name="blockMesh">分割ロード対応の BlockMesh。</param>
        void Enqueue(InterBlockMesh interBlockMesh, BlockMesh blockMesh)
        {
            // 最も粗い LOD から BlockMeshPart のロード要求をキューに入れます。
            for (int lod = blockMesh.MeshLods.Count - 1; 0 <= lod; lod--)
            {
                for (int i = 0; i < blockMesh.MeshLods[lod].MeshParts.Count; i++)
                {
                    Enqueue(interBlockMesh, blockMesh, lod, i, TimeSpan.FromMilliseconds(0));
                }
            }
        }

        void Enqueue(InterBlockMesh interBlockMesh, BlockMesh blockMesh, int levelOfDetail, int meshPartIndex, TimeSpan duration)
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
