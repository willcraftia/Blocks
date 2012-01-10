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
    /// <typeparam name="T">頂点構造体の型。</typeparam>
    public sealed class VertexSource<T> : IDisposable where T : struct
    {
        /// <summary>
        /// 頂点の位置のリスト。
        /// </summary>
        public List<T> Vertices { get; private set; }

        /// <summary>
        /// 頂点のインデックスのリスト。
        /// </summary>
        public List<ushort> Indices { get; private set; }

        /// <summary>
        /// 追加する頂点のインデックス。
        /// </summary>
        public int CurrentVertex
        {
            get { return Vertices.Count; }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public VertexSource()
        {
            Vertices = new List<T>();
            Indices = new List<ushort>();
        }

        // I/F
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 頂点を追加します。
        /// </summary>
        /// <param name="vertex">頂点構造体。</param>
        public void AddVertex(T vertex)
        {
            Vertices.Add(vertex);
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
