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
        /// 頂点の配列。
        /// </summary>
        VertexPositionNormal[] vertices;

        /// <summary>
        /// インデックスの配列。
        /// </summary>
        ushort[] indices;

        /// <summary>
        /// VertexSource プロパティによる頂点とインデックスの編集が可能かどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (VertexSource プロパティによる頂点とインデックスの編集が不能である場合)、
        /// false (それ以外の場合)。
        /// </value>
        public bool Frozen { get; private set; }

        /// <summary>
        /// 参照する InterBlockEffect のインデックスを取得または設定します。
        /// </summary>
        public int EffectIndex { get; set; }

        /// <summary>
        /// 頂点の配列を取得します。
        /// このプロパティは、Frozen プロパティが true になるまで無効です。
        /// </summary>
        public VertexPositionNormal[] Vertices
        {
            get
            {
                if (!Frozen) throw new InvalidOperationException("This instance is not frozen.");
                return vertices;
            }
        }

        /// <summary>
        /// インデックスの配列を取得します。
        /// このプロパティは、Frozen プロパティが true になるまで無効です。
        /// </summary>
        public ushort[] Indices
        {
            get
            {
                if (!Frozen) throw new InvalidOperationException("This instance is not frozen.");
                return indices;
            }
        }

        /// <summary>
        /// 頂点とインデックスを管理する VertexSource を取得します。
        /// このプロパティは、Frozen プロパティが true になるまで有効です。
        /// </summary>
        /// <remarks>
        /// このプロパティへの参照を呼び出し元で保持して編集を続けたとしても、
        /// その編集による頂点とインデックスは Vertices および Indices プロパティに反映されません。
        /// </remarks>
        internal VertexSource<VertexPositionNormal, ushort> VertexSource
        {
            get
            {
                if (Frozen) throw new InvalidOperationException("This instance is frozen.");
                return vertexSource;
            }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public InterBlockMeshPart()
        {
            vertexSource = new VertexSource<VertexPositionNormal, ushort>();
        }

        /// <summary>
        /// 頂点とインデックスを変更不能に設定します。
        /// このメソッドの呼び出しにより、Frozen プロパティが true になります。
        /// </summary>
        internal void Freeze()
        {
            // 配列に変換します。
            vertices = vertexSource.Vertices.ToArray();
            indices = vertexSource.Indices.ToArray();

            // 外部で参照している可能性もあるので念のためクリアします。
            vertexSource.Vertices.Clear();
            vertexSource.Indices.Clear();
            
            // 参照を切り離します。
            vertexSource = null;

            Frozen = true;
        }
    }
}
