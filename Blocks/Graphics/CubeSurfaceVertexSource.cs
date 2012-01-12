#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    using SurfaceVertexSource = VertexSource<VertexPositionNormal, ushort>;

    /// <summary>
    /// 立方体の頂点データを面ごとに管理するクラスです。
    /// </summary>
    public sealed class CubeSurfaceVertexSource : IDisposable
    {
        /// <summary>
        /// 辺のサイズを取得します。
        /// </summary>
        public float Size { get; private set; }

        /// <summary>
        /// 面の頂点データの配列を取得します。
        /// インデックスは CubeSurfaces の列挙値に従います。
        /// </summary>
        public SurfaceVertexSource[] Surfaces { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="size">辺のサイズ。</param>
        public CubeSurfaceVertexSource(float size)
        {
            Size = size;

            Surfaces = new SurfaceVertexSource[6];

            Vector3[] normals =
            {
                new Vector3( 1,  0,  0),
                new Vector3(-1,  0,  0),
                new Vector3( 0,  1,  0),
                new Vector3( 0, -1,  0),
                new Vector3( 0,  0,  1),
                new Vector3( 0,  0, -1)
            };
            
            for (int i = 0; i < 6; i++)
            {
                Surfaces[i] = new SurfaceVertexSource();
                InitializeSurface(Surfaces[i], normals[i]);
            }
        }

        /// <summary>
        /// 指定の法線の面の頂点データを作成します。
        /// </summary>
        /// <param name="source">対応する面の VertexSource。</param>
        /// <param name="normal">面の法線。</param>
        void InitializeSurface(SurfaceVertexSource source, Vector3 normal)
        {
            var maker = new QuadrangleMaker();
            float s = Size * 0.5f;

            var side1 = new Vector3(normal.Y, normal.Z, normal.X);
            var side2 = Vector3.Cross(normal, side1);

            maker.Position0 = (normal - side1 - side2) * s;
            maker.Position1 = (normal - side1 + side2) * s;
            maker.Position2 = (normal + side1 + side2) * s;
            maker.Position3 = (normal + side1 - side2) * s;
            maker.Normal = normal;
            maker.Make(source);
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        bool disposed;

        ~CubeSurfaceVertexSource()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                foreach (var surface in Surfaces) surface.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
