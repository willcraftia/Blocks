﻿#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// 幾何プリミティブ (Geometric Primitive) の頂点データを管理するクラスです。
    /// </summary>
    public sealed class GeometricPrimitive : IDisposable
    {
        /// <summary>
        /// GraphicsDevice。
        /// </summary>
        GraphicsDevice graphicsDevice;

        /// <summary>
        /// 頂点バッファを取得します。
        /// </summary>
        public VertexBuffer VertexBuffer { get; private set; }

        /// <summary>
        /// インデックス バッファを取得します。
        /// </summary>
        public IndexBuffer IndexBuffer { get; private set; }

        /// <summary>
        /// 頂点バッファの最上部からのオフセットを取得します。
        /// </summary>
        public int VertexOffset { get; private set; }

        /// <summary>
        /// 頂点の数を取得します。
        /// </summary>
        public int NumVertices { get; private set; }

        /// <summary>
        /// 頂点の読み取りを開始するインデックスの位置を取得します。
        /// </summary>
        public int StartIndex { get; private set; }

        /// <summary>
        /// プリミティブの数を取得します。
        /// </summary>
        public int PrimitiveCount { get; private set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice。</param>
        GeometricPrimitive(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        /// <summary>
        /// 指定された VertexSource から GeometricPrimitive を生成します。
        /// </summary>
        /// <typeparam name="TVertex">頂点構造体の型。</typeparam>
        /// <typeparam name="TIndex">インデックスの型。</typeparam>
        /// <param name="graphicsDevice">GraphicsDevice。</param>
        /// <param name="source">VertexSource。</param>
        /// <returns>生成された GeometricPrimitive。</returns>
        public static GeometricPrimitive Create<TVertex, TIndex>(GraphicsDevice graphicsDevice, VertexSource<TVertex, TIndex> source)
            where TVertex : struct
            where TIndex : struct
        {
            var vertices = source.Vertices.ToArray();
            var indices = source.Indices.ToArray();
            return Create(graphicsDevice, vertices, indices);
        }

        /// <summary>
        /// 指定された頂点とインデックスのデータから GeometricPrimitive を生成します。
        /// </summary>
        /// <typeparam name="TVertex">頂点構造体の型。</typeparam>
        /// <typeparam name="TIndex">インデックスの型。</typeparam>
        /// <param name="graphicsDevice">GraphicsDevice。</param>
        /// <param name="vertices">頂点データの配列。</param>
        /// <param name="indices">インデックス データの配列。</param>
        /// <returns>生成された GeometricPrimitive。</returns>
        public static GeometricPrimitive Create<TVertex, TIndex>(GraphicsDevice graphicsDevice, TVertex[] vertices, TIndex[] indices)
            where TVertex : struct
            where TIndex : struct
        {
            if (graphicsDevice == null) throw new ArgumentNullException("graphicsDevice");

            var instance = new GeometricPrimitive(graphicsDevice);
            instance.Initialize(vertices, indices);
            return instance;
        }

        /// <summary>
        /// 指定の Effect で描画します。
        /// </summary>
        /// <param name="effect">Effect。</param>
        public void Draw(Effect effect)
        {
            GraphicsDevice graphicsDevice = effect.GraphicsDevice;

            graphicsDevice.SetVertexBuffer(VertexBuffer);
            graphicsDevice.Indices = IndexBuffer;
            
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, VertexOffset, 0, NumVertices, StartIndex, PrimitiveCount);
            }
        }

        /// <summary>
        /// 指定された頂点とインデックスのデータで GeometricPrimitive を初期化します。
        /// </summary>
        /// <typeparam name="TVertex">頂点構造体の型。</typeparam>
        /// <typeparam name="TIndex">インデックスの型。</typeparam>
        /// <param name="vertices">頂点データの配列。</param>
        /// <param name="indices">インデックス データの配列。</param>
        void Initialize<TVertex, TIndex>(TVertex[] vertices, TIndex[] indices)
            where TVertex : struct
            where TIndex : struct
        {
            // 頂点バッファを初期化します。
            VertexBuffer = new VertexBuffer(graphicsDevice, typeof(TVertex), vertices.Length, BufferUsage.None);
            VertexBuffer.SetData(vertices);

            // インデックス バッファを初期化します。
            IndexBuffer = new IndexBuffer(graphicsDevice, typeof(TIndex), indices.Length, BufferUsage.None);
            IndexBuffer.SetData(indices);

            VertexOffset = 0;
            NumVertices = vertices.Length;
            StartIndex = 0;
            PrimitiveCount = indices.Length / 3;
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        bool disposed;

        ~GeometricPrimitive()
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
