#region Using

using System;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Models
{
    public sealed class CubeMesh : IDisposable
    {
        GraphicsDevice graphicsDevice;

        GeometricPrimitive mesh;

        public float Size { get; private set; }

        public BasicEffect Effect { get; private set; }

        public CubeMesh(GraphicsDevice graphicsDevice, float size)
        {
            if (graphicsDevice == null) throw new ArgumentNullException("graphicsDevice");
            if (size < 0) throw new ArgumentOutOfRangeException("size");
            this.graphicsDevice = graphicsDevice;
            Size = size;

            Effect = new BasicEffect(graphicsDevice);
            //Effect.EnableDefaultLighting();
            mesh = CreateCubePrimitive(size);
        }

        public void Draw()
        {
            mesh.Draw(Effect);
        }

        /// <summary>
        /// 立方体の GeometricPrimitive を生成します。
        /// </summary>
        /// <param name="size"></param>
        /// <returns>生成された立方体の GeometricPrimitive。</returns>
        GeometricPrimitive CreateCubePrimitive(float size)
        {
            var cube = new Cube { Size = size };
            var source = new VertexSource<VertexPositionNormal, ushort>();
            cube.Make(source);
            return GeometricPrimitive.Create(graphicsDevice, source);
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool disposed;

        ~CubeMesh()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                Effect.Dispose();
                mesh.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
