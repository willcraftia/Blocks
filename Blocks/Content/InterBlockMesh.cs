#region Using

using System;
using Willcraftia.Xna.Blocks.Graphics;

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
        /// BlockMeshMaterial の配列。
        /// </summary>
        public BlockMeshMaterial[] MeshMaterials;

        /// <summary>
        /// InterBlockMeshLod の配列。
        /// </summary>
        public InterBlockMeshLod[] MeshLods;
    }
}
