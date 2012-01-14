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
    public sealed class BlockMesh
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
        /// </summary>
        public ReadOnlyCollection<BlockMeshPart> MeshParts
        {
            get { return lodMeshParts[LevelOfDetail]; }
        }

        /// <summary>
        /// 利用する LOD レベルを取得または設定します。
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
    }
}
