#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// 頂点バッファを作成するための一時データを管理するクラスです。
    /// </summary>
    public sealed class VertexSource : IDisposable
    {
        /// <summary>
        /// 頂点の位置のリスト。
        /// </summary>
        internal List<VertexPositionNormalColor> Vertices = new List<VertexPositionNormalColor>();

        /// <summary>
        /// 頂点のインデックスのリスト。
        /// </summary>
        internal List<ushort> Indices = new List<ushort>();

        /// <summary>
        /// 追加する頂点のインデックス。
        /// </summary>
        public int CurrentVertex
        {
            get { return Vertices.Count; }
        }

        // I/F
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 頂点を追加します。
        /// </summary>
        /// <param name="position">位置。</param>
        /// <param name="normal">法線。</param>
        /// <param name="color">色。</param>
        public void AddVertex(Vector3 position, Vector3 normal, Color color)
        {
            Vertices.Add(new VertexPositionNormalColor(position, normal, color));
        }

        /// <summary>
        /// インデックスを追加します。
        /// </summary>
        /// <param name="index">インデックス。</param>
        public void AddIndex(int index)
        {
            Indices.Add((ushort) index);
        }
    }
}
