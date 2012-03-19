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
            /// このロード要求で対象とする LOD。
            /// </summary>
            public int LevelOfDetail;

            /// <summary>
            /// このロード要求で対象とする BlockMeshPart のインデックス。
            /// </summary>
            public int MeshPartIndex;
        }

        #endregion

        /// <summary>
        /// Item のキュー。
        /// </summary>
        List<Item> queue;

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
            queue = new List<Item>(initialCapacity);
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
            var coarsestLod = blockMesh.MeshLods.Count - 1;
            Enqueue(interBlockMesh, blockMesh, coarsestLod);

            // 分割ロード対応の BlockMesh を要求元へ返します。
            return blockMesh;
        }

        public void Cancel(BlockMesh blockMesh)
        {
            queue.RemoveAll((i) => i.BlockMesh == blockMesh);
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
        /// 指定の LOD についての分割ロード要求をキューへ追加します。
        /// </summary>
        /// <param name="interBlockMesh">BlockMesh のロード元となる InterBlockMesh。</param>
        /// <param name="blockMesh">分割ロード対応の BlockMesh。</param>
        /// <param name="levelOfDetail">対象とする LOD。</param>
        void Enqueue(InterBlockMesh interBlockMesh, BlockMesh blockMesh, int levelOfDetail)
        {
            for (int i = 0; i < blockMesh.MeshLods[levelOfDetail].MeshParts.Count; i++)
            {
                Enqueue(interBlockMesh, blockMesh, levelOfDetail, i);
            }
        }

        /// <summary>
        /// 指定の BlockMeshPart のロード要求をキューへ追加します。
        /// </summary>
        /// <param name="interBlockMesh">BlockMesh のロード元となる InterBlockMesh。</param>
        /// <param name="blockMesh">分割ロード対応の BlockMesh。</param>
        /// <param name="levelOfDetail">対象とする LOD。</param>
        /// <param name="meshPartIndex">対象とする BlockMeshPart のインデックス。</param>
        void Enqueue(InterBlockMesh interBlockMesh, BlockMesh blockMesh, int levelOfDetail, int meshPartIndex)
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
                    MeshPartIndex = meshPartIndex
                };

                queue.Add(item);
            }
        }

        /// <summary>
        /// キューに追加されたロード要求を処理します。
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            var time = gameTime.TotalGameTime;
            if (time < lastUpdateTime + delay) return;

            // 更新遅延が発生しているならば、待機時間を延長させます。
            if (gameTime.IsRunningSlowly)
            {
                delay += TimeSpan.FromMilliseconds(100);
                return;
            }

            // キューからロード要求を取り出します。
            Item item;
            lock (this)
            {
                if (queue.Count == 0) return;

                item = queue[0];
                queue.RemoveAt(0);
            }

            // ロード要求を処理します。
            Load(ref item);

            delay = TimeSpan.Zero;
            lastUpdateTime = time;
        }

        /// <summary>
        /// BlockMeshPart をロードします。
        /// </summary>
        /// <param name="item">ロード要求。</param>
        void Load(ref Item item)
        {
            var meshPart = item.BlockMesh.MeshLods[item.LevelOfDetail].MeshParts[item.MeshPartIndex];
            var interMeshPart = item.InterBlockMesh.MeshLods[item.LevelOfDetail].MeshParts[item.MeshPartIndex];

            meshPart.PopulateVertices(interMeshPart.Vertices);
            meshPart.PopulateIndices(interMeshPart.Indices);

            // 最大 LOD ではないならば、上位 LOD のロード要求をキューへ追加します。
            if (0 < item.LevelOfDetail && item.BlockMesh.MeshLods[item.LevelOfDetail].IsLoaded)
            {
                var finerLod = item.LevelOfDetail - 1;
                Enqueue(item.InterBlockMesh, item.BlockMesh, finerLod);
            }
        }
    }
}
