#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    /// <summary>
    /// Block モデルを表すクラスです。
    /// </summary>
    public sealed class BlockModel
    {
        /// <summary>
        /// IBlockEffect のリストを取得します。
        /// </summary>
        public ReadOnlyCollection<IBlockEffect> Effects { get; private set; }

        /// <summary>
        /// BlockModelMesh のリストを取得します。
        /// </summary>
        public ReadOnlyCollection<BlockModelMesh> Meshes { get; private set; }

        /// <summary>
        /// IBlockEffect のリストを取得します (内部処理用)。
        /// </summary>
        internal List<IBlockEffect> InternalEffects { get; private set; }

        /// <summary>
        /// BlockModelMesh のリストを取得します (内部処理用)。
        /// </summary>
        internal List<BlockModelMesh> InternalMeshes { get; private set; }

        /// <summary>
        /// インスタンスを生成します (内部処理用)。
        /// </summary>
        internal BlockModel()
        {
            InternalEffects = new List<IBlockEffect>();
            InternalMeshes = new List<BlockModelMesh>();

            Effects = new ReadOnlyCollection<IBlockEffect>(InternalEffects);
            Meshes = new ReadOnlyCollection<BlockModelMesh>(InternalMeshes);
        }
    }
}
