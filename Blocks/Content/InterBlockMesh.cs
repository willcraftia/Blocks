#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// BlockMesh の頂点情報とエフェクト情報を管理するクラスです。
    /// このクラスは、CPU メモリ上で頂点情報とエフェクト情報を管理します。
    /// </summary>
    public sealed class InterBlockMesh
    {
        /// <summary>
        /// InterBlockEffect の配列。
        /// </summary>
        public InterBlockEffect[] Effects;

        /// <summary>
        /// InterBlockMeshPart の配列。
        /// </summary>
        public InterBlockMeshPart[][] MeshParts;
    }
}
