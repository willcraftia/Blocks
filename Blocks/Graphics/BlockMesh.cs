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
        ReadOnlyCollection<IBlockEffect> effects;

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

        internal void InitializeEffects(IBlockEffect[] effects)
        {
            this.effects = new ReadOnlyCollection<IBlockEffect>(effects);
        }

        internal void InitializeLODMeshParts(int lod, BlockMeshPart[] meshParts)
        {
            lodMeshParts[lod] = new ReadOnlyCollection<BlockMeshPart>(meshParts);
        }
    }
}
