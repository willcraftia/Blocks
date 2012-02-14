#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    public sealed class GridPlane
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
        /// プリミティブの数を取得します。
        /// </summary>
        public int PrimitiveCount { get; private set; }

        GridPlane(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null) throw new ArgumentNullException("graphicsDevice");

            this.graphicsDevice = graphicsDevice;
        }

        public static GridPlane CreatePlaneXY(GraphicsDevice graphicsDevice,
            int xSize, int ySize, float cellSize, Color color, Matrix transform)
        {
            var instance = new GridPlane(graphicsDevice);
            instance.InitializePlaneXY(xSize, ySize, cellSize, ref color, ref transform);
            return instance;
        }

        public static GridPlane CreatePlaneXZ(GraphicsDevice graphicsDevice,
            int xSize, int zSize, float cellSize, Color color, Matrix transform)
        {
            var instance = new GridPlane(graphicsDevice);
            instance.InitializePlaneXZ(xSize, zSize, cellSize, ref color, ref transform);
            return instance;
        }

        public static GridPlane CreatePlaneZY(GraphicsDevice graphicsDevice,
            int zSize, int ySize, float cellSize, Color color, Matrix transform)
        {
            var instance = new GridPlane(graphicsDevice);
            instance.InitializePlaneZY(zSize, ySize, cellSize, ref color, ref transform);
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

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, PrimitiveCount);
            }
        }

        void InitializePlaneXY(int xSize, int ySize, float cellSize, ref Color color, ref Matrix transform)
        {
            if (xSize <= 0) throw new ArgumentOutOfRangeException("xSize");
            if (ySize <= 0) throw new ArgumentOutOfRangeException("ySize");
            if (cellSize <= 0) throw new ArgumentOutOfRangeException("cellSize");

            PrimitiveCount = (xSize + 1) + (ySize + 1);
            var vertices = new VertexPositionColor[PrimitiveCount * 2];

            int index = 0;

            float halfX = xSize / 2 * cellSize;
            float halfY = ySize / 2 * cellSize;

            // Y 軸に平行な線分
            for (int i = 0; i < xSize + 1; i++)
            {
                float x = i * cellSize - halfX;

                var s = Vector3.Transform(new Vector3(x, -halfY, 0), transform);
                var e = Vector3.Transform(new Vector3(x, halfY, 0), transform);
                var c = (i == xSize / 2) ? Color.Green : color;

                vertices[index++] = new VertexPositionColor(s, c);
                vertices[index++] = new VertexPositionColor(e, c);
            }

            // X 軸に平行な線分
            for (int i = 0; i < ySize + 1; i++)
            {
                float y = i * cellSize - halfY;

                var s = Vector3.Transform(new Vector3(-halfX, y, 0), transform);
                var e = Vector3.Transform(new Vector3(halfX, y, 0), transform);
                var c = (i == ySize / 2) ? Color.Red : color;

                vertices[index++] = new VertexPositionColor(s, c);
                vertices[index++] = new VertexPositionColor(e, c);
            }

            VertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.None);
            VertexBuffer.SetData<VertexPositionColor>(vertices);
        }

        void InitializePlaneXZ(int xSize, int zSize, float cellSize, ref Color color, ref Matrix transform)
        {
            if (xSize <= 0) throw new ArgumentOutOfRangeException("xSize");
            if (zSize <= 0) throw new ArgumentOutOfRangeException("zSize");
            if (cellSize <= 0) throw new ArgumentOutOfRangeException("cellSize");

            PrimitiveCount = (xSize + 1) + (zSize + 1);
            var vertices = new VertexPositionColor[PrimitiveCount * 2];

            int index = 0;

            float halfX = xSize / 2 * cellSize;
            float halfZ = zSize / 2 * cellSize;

            // Z 軸に平行な線分
            for (int i = 0; i < xSize + 1; i++)
            {
                float x = i * cellSize - halfX;

                var s = Vector3.Transform(new Vector3(x, 0, -halfZ), transform);
                var e = Vector3.Transform(new Vector3(x, 0, halfZ), transform);
                var c = (i == xSize / 2) ? Color.Blue : color;

                vertices[index++] = new VertexPositionColor(s, c);
                vertices[index++] = new VertexPositionColor(e, c);
            }

            // X 軸に平行な線分
            for (int i = 0; i < zSize + 1; i++)
            {
                float z = i * cellSize - halfZ;

                var s = Vector3.Transform(new Vector3(-halfX, 0, z), transform);
                var e = Vector3.Transform(new Vector3(halfX, 0, z), transform);
                var c = (i == zSize / 2) ? Color.Red : color;

                vertices[index++] = new VertexPositionColor(s, c);
                vertices[index++] = new VertexPositionColor(e, c);
            }

            VertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.None);
            VertexBuffer.SetData<VertexPositionColor>(vertices);
        }

        void InitializePlaneZY(int zSize, int ySize, float cellSize, ref Color color, ref Matrix transform)
        {
            if (zSize <= 0) throw new ArgumentOutOfRangeException("zSize");
            if (ySize <= 0) throw new ArgumentOutOfRangeException("ySize");
            if (cellSize <= 0) throw new ArgumentOutOfRangeException("cellSize");

            PrimitiveCount = (zSize + 1) + (ySize + 1);
            var vertices = new VertexPositionColor[PrimitiveCount * 2];

            int index = 0;

            float halfZ = zSize / 2 * cellSize;
            float halfY = ySize / 2 * cellSize;

            // Y 軸に平行な線分
            for (int i = 0; i < zSize + 1; i++)
            {
                float z = i * cellSize - halfZ;

                var s = Vector3.Transform(new Vector3(0, -halfY, z), transform);
                var e = Vector3.Transform(new Vector3(0, halfY, z), transform);
                var c = (i == zSize / 2) ? Color.Green : color;

                vertices[index++] = new VertexPositionColor(s, c);
                vertices[index++] = new VertexPositionColor(e, c);
            }

            // Z 軸に平行な線分
            for (int i = 0; i < ySize + 1; i++)
            {
                float y = i * cellSize - halfY;

                var s = Vector3.Transform(new Vector3(0, y, -halfZ), transform);
                var e = Vector3.Transform(new Vector3(0, y, halfZ), transform);
                var c = (i == ySize / 2) ? Color.Blue : color;

                vertices[index++] = new VertexPositionColor(s, c);
                vertices[index++] = new VertexPositionColor(e, c);
            }

            VertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.None);
            VertexBuffer.SetData<VertexPositionColor>(vertices);
        }
    }
}
