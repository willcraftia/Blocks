#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    /// <summary>
    /// Block の Mesh を表すクラスです。
    /// </summary>
    public sealed class BlockMesh : IDisposable
    {
        /// <summary>
        /// ロードが完了した時に発生します。
        /// </summary>
        public event EventHandler Loaded = delegate { };

        /// <summary>
        /// ロードが完了しているかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (ロードが完了している場合)、false (それ以外の場合)。
        /// </value>
        public bool IsLoaded { get; private set; }

        /// <summary>
        /// BlockMeshMaterial のリストを取得します。
        /// </summary>
        public ReadOnlyCollection<BlockMeshMaterial> MeshMaterials { get; private set; }

        /// <summary>
        /// LodBlockMesh のリストを取得します。
        /// </summary>
        public ReadOnlyCollection<BlockMeshLod> MeshLods { get; private set; }

        /// <summary>
        /// 利用できる LOD 数を取得します。
        /// </summary>
        public int LevelOfDetailCount
        {
            get { return MeshLods.Count; }
        }

        /// <summary>
        /// 利用する LOD を取得または設定します。
        /// </summary>
        public int LevelOfDetail { get; set; }

        /// <summary>
        /// LevelOfDetail プロパティが示す BlockMeshLod を取得します。
        /// LevelOfDetail プロパティの値が LevelOfDetailSize プロパティの値を越える場合、
        /// 最も荒い LOD の BlockMeshLod が返されます。
        /// </summary>
        public BlockMeshLod MeshLod
        {
            get
            {
                var targetLod = LevelOfDetail;
                if (LevelOfDetailCount <= LevelOfDetail) targetLod = MeshLods.Count - 1;

                return MeshLods[targetLod];
            }
        }

        /// <summary>
        /// BlockMeshLod プロパティの BlockMeshPart のリストを取得します。
        /// </summary>
        public ReadOnlyCollection<BlockMeshPart> MeshParts
        {
            get { return MeshLod.MeshParts; }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        internal BlockMesh() { }

        public void Draw(IBlockEffect effect)
        {
            MeshLod.Draw(effect);
        }

        internal void SetMeshMaterials(BlockMeshMaterial[] meshMaterials)
        {
            MeshMaterials = new ReadOnlyCollection<BlockMeshMaterial>(meshMaterials);
        }

        internal void AllocateMeshLods(int count)
        {
            var array = new BlockMeshLod[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = new BlockMeshLod(i);
                array[i].Loaded += OnMeshLodLoaded;
            }
            MeshLods = new ReadOnlyCollection<BlockMeshLod>(array);
        }

        void OnMeshLodLoaded(object sender, EventArgs e)
        {
            foreach (var meshLod in MeshLods)
            {
                if (!meshLod.IsLoaded) return;
            }

            IsLoaded = true;
            Loaded(this, EventArgs.Empty);
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool disposed;

        ~BlockMesh()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                if (MeshLods != null)
                {
                    foreach (var meshLod in MeshLods) meshLod.Dispose();
                }
            }

            disposed = true;
        }

        #endregion
    }
}
