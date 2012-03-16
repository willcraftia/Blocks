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
        /// 頂点とインデックスを管理する VertexSource。
        /// </summary>
        VertexSource<VertexPositionNormal, ushort> vertexSource;

        /// <summary>
        /// VertexSource.Vertices を読み取り専用として公開するためのコレクション。
        /// </summary>
        ReadOnlyCollection<VertexPositionNormal> vertices;

        /// <summary>
        /// VertexSource.indices を読み取り専用として公開するためのコレクション。
        /// </summary>
        ReadOnlyCollection<ushort> indices;

        /// <summary>
        /// 参照する InterBlockEffect のインデックスを取得または設定します。
        /// </summary>
        public int EffectIndex { get; set; }

        /// <summary>
        /// 頂点リストを取得します。
        /// </summary>
        public ReadOnlyCollection<VertexPositionNormal> Vertices
        {
            get { return vertices; }
        }

        /// <summary>
        /// インデックス リストを取得します。
        /// </summary>
        public ReadOnlyCollection<ushort> Indices
        {
            get { return indices; }
        }

        /// <summary>
        /// 頂点とインデックスを管理する VertexSource を取得します。
        /// </summary>
        internal VertexSource<VertexPositionNormal, ushort> VertexSource
        {
            get { return vertexSource; }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public InterBlockMeshPart()
        {
            vertexSource = new VertexSource<VertexPositionNormal, ushort>();
            vertices = new ReadOnlyCollection<VertexPositionNormal>(VertexSource.Vertices);
            indices = new ReadOnlyCollection<ushort>(VertexSource.Indices);
        }
    }
}
