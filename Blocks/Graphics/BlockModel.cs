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
        /// BlockModelMaterial のリストを取得します。
        /// </summary>
        public ReadOnlyCollection<BlockModelMaterial> Materials { get; private set; }

        /// <summary>
        /// BlockModelMesh のリストを取得します。
        /// </summary>
        public ReadOnlyCollection<BlockModelMesh> Meshes { get; private set; }

        /// <summary>
        /// BlockModelMaterial のリストを取得します (内部処理用)。
        /// </summary>
        internal List<BlockModelMaterial> InternalMaterials { get; private set; }

        /// <summary>
        /// BlockModelMesh のリストを取得します (内部処理用)。
        /// </summary>
        internal List<BlockModelMesh> InternalMeshes { get; private set; }

        /// <summary>
        /// インスタンスを生成します (内部処理用)。
        /// </summary>
        internal BlockModel()
        {
            InternalMaterials = new List<BlockModelMaterial>();
            InternalMeshes = new List<BlockModelMesh>();

            Materials = new ReadOnlyCollection<BlockModelMaterial>(InternalMaterials);
            Meshes = new ReadOnlyCollection<BlockModelMesh>(InternalMeshes);
        }
    }
}
