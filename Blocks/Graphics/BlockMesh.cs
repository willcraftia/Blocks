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
        /// IBlockEffect のリスト。
        /// </summary>
        ReadOnlyCollection<IBlockEffect> effects;

        /// <summary>
        /// LOD ごとの BlockMeshPart のリスト。
        /// </summary>
        ReadOnlyCollection<BlockMeshPart>[] lodMeshParts;

        /// <summary>
        /// IBlockEffect のリストを取得します。
        /// </summary>
        public ReadOnlyCollection<IBlockEffect> Effects
        {
            get { return effects; }
        }

        /// <summary>
        /// BlockMeshPart のリストを取得します。
        /// LevelOfDetail プロパティの値が LevelOfDetailSize プロパティの値を越える場合、
        /// 最も荒い LOD の BlockMeshPart が返されます。
        /// </summary>
        public ReadOnlyCollection<BlockMeshPart> MeshParts
        {
            get
            {
                if (LevelOfDetail < lodMeshParts.Length)
                {
                    return lodMeshParts[LevelOfDetail];
                }
                else
                {
                    return lodMeshParts[lodMeshParts.Length - 1];
                }
            }
        }

        /// <summary>
        /// 利用できる LOD 数を取得します。
        /// </summary>
        public int LevelOfDetailSize
        {
            get { return lodMeshParts.Length; }
        }

        /// <summary>
        /// 利用する LOD を取得または設定します。
        /// </summary>
        public int LevelOfDetail { get; set; }

        /// <summary>
        /// インスタンスを生成します (内部処理用)。
        /// </summary>
        internal BlockMesh(int lodSize)
        {
            lodMeshParts = new ReadOnlyCollection<BlockMeshPart>[lodSize];
        }

        /// <summary>
        /// 描画します。
        /// </summary>
        public void Draw()
        {
            foreach (var meshPart in MeshParts) meshPart.Draw();
        }

        /// <summary>
        /// IBlockEffect 配列を設定します。
        /// </summary>
        /// <param name="effects">IBlockEffect 配列。</param>
        internal void SetEffectArray(IBlockEffect[] effects)
        {
            this.effects = new ReadOnlyCollection<IBlockEffect>(effects);
        }

        /// <summary>
        /// 指定の LOD の BlockMeshPart 配列を設定します。
        /// </summary>
        /// <param name="lod">LOD。</param>
        /// <param name="meshParts">BlockMeshPart 配列。</param>
        internal void SetLODMeshPartArray(int lod, BlockMeshPart[] meshParts)
        {
            lodMeshParts[lod] = new ReadOnlyCollection<BlockMeshPart>(meshParts);
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
                if (effects != null)
                {
                    foreach (var effect in effects) effect.Dispose();
                }
                if (lodMeshParts != null)
                {
                    foreach (var meshParts in lodMeshParts)
                    {
                        foreach (var meshPart in meshParts) meshPart.Dispose();
                    }
                }
            }

            disposed = true;
        }

        #endregion
    }
}
