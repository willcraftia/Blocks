#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Willcraftia.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// Material ごとに頂点情報が分けられた InterBlockMesh のパーツを表すクラスです。
    /// このクラスは、CPU 上で頂点情報を管理します。
    /// </summary>
    public sealed class InterBlockMeshPart
    {
        /// <summary>
        /// 参照する InterBlockEffect のインデックス。
        /// </summary>
        public int EffectIndex;

        /// <summary>
        /// 頂点の配列。
        /// </summary>
        public VertexPositionNormal[] Vertices;

        /// <summary>
        /// インデックスの配列。
        /// </summary>
        public ushort[] Indices;
    }
}
