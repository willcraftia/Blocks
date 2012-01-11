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
    /// <typeparam name="TVertex">頂点構造体の型。</typeparam>
    /// <typeparam name="TIndex">インデックスの型。</typeparam>
    public sealed class VertexSource<TVertex, TIndex> : IDisposable
        where TVertex : struct
        where TIndex : struct
    {
        /// <summary>
        /// 頂点の位置のリスト。
        /// </summary>
        public List<TVertex> Vertices { get; private set; }

        /// <summary>
        /// 頂点のインデックスのリスト。
        /// </summary>
        public List<TIndex> Indices { get; private set; }

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
            Vertices = new List<TVertex>();
            Indices = new List<TIndex>();
        }

        /// <summary>
        /// 頂点を追加します。
        /// </summary>
        /// <param name="vertex">頂点構造体。</param>
        public void AddVertex(TVertex vertex)
        {
            Vertices.Add(vertex);
        }

        /// <summary>
        /// インデックスを追加します。
        /// </summary>
        /// <param name="index">インデックス。</param>
        public void AddIndex(TIndex index)
        {
            Indices.Add(index);
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        bool disposed;

        ~VertexSource()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                Vertices.Clear();
                Indices.Clear();
            }

            disposed = true;
        }

        #endregion
    }
}
