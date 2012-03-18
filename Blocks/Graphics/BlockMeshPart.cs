#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    /// <summary>
    /// Material ごとに頂点データが分けられた BlockMesh のパーツを表すクラスです。
    /// </summary>
    public sealed class BlockMeshPart : IDisposable
    {
        /// <summary>
        /// VertexBuffer および IndexBuffer のロードが完了した時に発生します。
        /// </summary>
        public event EventHandler Loaded = delegate { };

        /// <summary>
        /// GraphicsDevice。
        /// </summary>
        GraphicsDevice graphicsDevice;

        /// <summary>
        /// VertexBuffer のロードが完了しているかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (VertexBuffer の設定が完了している場合)、false (それ以外の場合)。
        /// </value>
        public bool VertexBufferLoaded { get; private set; }

        /// <summary>
        /// IndexBuffer のロードが完了しているかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (IndexBuffer の設定が完了している場合)、false (それ以外の場合)。
        /// </value>
        public bool IndexBufferLoaded { get; private set; }

        /// <summary>
        /// VertexBuffer を取得します。
        /// VertexBufferLoaded プロパティが false の場合、
        /// このプロパティは null です。
        /// </summary>
        public VertexBuffer VertexBuffer { get; private set; }

        /// <summary>
        /// IndexBuffer を取得します。
        /// IndexBufferLoaded プロパティが false の場合、
        /// このプロパティは null です。
        /// </summary>
        public IndexBuffer IndexBuffer { get; private set; }

        /// <summary>
        /// VertexBuffer の最上部からのオフセットを取得します。
        /// </summary>
        public int VertexOffset { get; private set; }

        /// <summary>
        /// 頂点数を取得します。
        /// </summary>
        public int NumVertices { get; private set; }

        /// <summary>
        /// 頂点の読み取りを開始するインデックスの位置を取得します。
        /// </summary>
        public int StartIndex { get; private set; }

        /// <summary>
        /// プリミティブ数を取得します。
        /// </summary>
        public int PrimitiveCount { get; private set; }

        /// <summary>
        /// 参照する BlockMeshEffect を取得します。
        /// </summary>
        public BlockMeshEffect MeshEffect { get; internal set; }

        /// <summary>
        /// VertexBuffer および IndexBuffer のロードが完了しているかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (VertexBuffer および IndexBuffer のロードが完了している場合)、false (それ以外の場合)。
        /// </value>
        public bool IsLoaded
        {
            get { return VertexBufferLoaded && IndexBufferLoaded; }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        internal BlockMeshPart(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            VertexOffset = 0;
            StartIndex = 0;
        }

        internal void PopulateVertices<TVertex>(TVertex[] vertices) where TVertex : struct
        {
            VertexBuffer = new VertexBuffer(graphicsDevice, typeof(TVertex), vertices.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(vertices);

            NumVertices = vertices.Length;

            VertexBufferLoaded = true;

            if (IsLoaded) Loaded(this, EventArgs.Empty);
        }

        internal void PopulateIndices<TIndex>(TIndex[] indices) where TIndex : struct
        {
            // インデックス バッファを初期化します。
            IndexBuffer = new IndexBuffer(graphicsDevice, typeof(TIndex), indices.Length, BufferUsage.WriteOnly);
            IndexBuffer.SetData(indices);

            PrimitiveCount = indices.Length / 3;

            IndexBufferLoaded = true;

            if (IsLoaded) Loaded(this, EventArgs.Empty);
        }

        /// <summary>
        /// この BlockMeshPart が参照する BlockMeshEffect で描画します。
        /// </summary>
        public void Draw()
        {
            if (!IsLoaded) throw new InvalidOperationException("BlockMeshPart is not loaded.");
            if (!MeshEffect.IsLoaded) throw new InvalidOperationException("The effect referenced is not loaded.");

            graphicsDevice.SetVertexBuffer(VertexBuffer, VertexOffset);
            graphicsDevice.Indices = IndexBuffer;

            MeshEffect.Effect.Pass.Apply();
            graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, NumVertices, StartIndex, PrimitiveCount);
        }

        /// <summary>
        /// 指定された Effect で描画します。
        /// </summary>
        /// <param name="effect">Effect。</param>
        public void Draw(Effect effect)
        {
            if (!IsLoaded) throw new InvalidOperationException("BlockMeshPart is not loaded.");

            graphicsDevice.SetVertexBuffer(VertexBuffer, VertexOffset);
            graphicsDevice.Indices = IndexBuffer;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, NumVertices, StartIndex, PrimitiveCount);
            }
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        bool disposed;

        ~BlockMeshPart()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                if (VertexBuffer != null) VertexBuffer.Dispose();
                if (IndexBuffer != null) IndexBuffer.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
