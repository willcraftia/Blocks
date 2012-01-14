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
        /// IBlockEffect のリストを取得します。
        /// </summary>
        public ReadOnlyCollection<IBlockEffect> Effects { get; private set; }

        /// <summary>
        /// BlockMeshPart のリストを取得します。
        /// </summary>
        public ReadOnlyCollection<BlockMeshPart> MeshParts { get; private set; }

        /// <summary>
        /// IBlockEffect のリストを取得します (内部処理用)。
        /// </summary>
        internal List<IBlockEffect> InternalEffects { get; private set; }

        /// <summary>
        /// BlockMeshPart のリストを取得します (内部処理用)。
        /// </summary>
        internal List<BlockMeshPart> InternalMeshParts { get; private set; }

        /// <summary>
        /// インスタンスを生成します (内部処理用)。
        /// </summary>
        internal BlockMesh()
        {
            InternalEffects = new List<IBlockEffect>();
            InternalMeshParts = new List<BlockMeshPart>();

            Effects = new ReadOnlyCollection<IBlockEffect>(InternalEffects);
            MeshParts = new ReadOnlyCollection<BlockMeshPart>(InternalMeshParts);
        }
    }
}
