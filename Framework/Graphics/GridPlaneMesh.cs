#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    public sealed class GridPlaneMesh
    {
        /// <summary>
        /// GraphicsDevice。
        /// </summary>
        GraphicsDevice graphicsDevice;

        public Color Color { get; private set; }

        public int Column { get; private set; }

        public int Row { get; private set; }

        public float CellWidth { get; private set; }

        public float CellHeight { get; private set; }

        /// <summary>
        /// 頂点バッファを取得します。
        /// </summary>
        public VertexBuffer VertexBuffer { get; private set; }

        /// <summary>
        /// プリミティブの数を取得します。
        /// </summary>
        public int PrimitiveCount { get; private set; }

        public GridPlaneMesh(GraphicsDevice graphicsDevice, int column, int row, float cellWidth, float cellHeight, Color color)
        {
            if (graphicsDevice == null) throw new ArgumentNullException("graphicsDevice");
            if (column <= 0) throw new ArgumentOutOfRangeException("column");
            if (row <= 0) throw new ArgumentOutOfRangeException("row");
            if (cellWidth <= 0) throw new ArgumentOutOfRangeException("cellWidth");
            if (cellHeight <= 0) throw new ArgumentOutOfRangeException("cellHeight");

            this.graphicsDevice = graphicsDevice;
            Column = column;
            Row = row;
            CellWidth = cellWidth;
            CellHeight = cellHeight;
            Color = color;

            InitializeVertexBuffer();
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

        void InitializeVertexBuffer()
        {
            PrimitiveCount = (Column + 1) + (Row + 1);
            var vertices = new VertexPositionColor[PrimitiveCount * 2];

            int index = 0;

            // Y 軸に平行な線分
            float h = Row * CellHeight;
            vertices[index++] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Green);
            vertices[index++] = new VertexPositionColor(new Vector3(0, h, 0), Color.Green);
            for (int i = 1; i < Column + 1; i++)
            {
                float x = i * CellWidth;
                vertices[index++] = new VertexPositionColor(new Vector3(x, 0, 0), Color);
                vertices[index++] = new VertexPositionColor(new Vector3(x, h, 0), Color);
            }

            // X 軸に平行な線分
            float w = Column * CellWidth;
            vertices[index++] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Red);
            vertices[index++] = new VertexPositionColor(new Vector3(w, 0, 0), Color.Red);
            for (int i = 1; i < Row + 1; i++)
            {
                float y = i * CellHeight;
                vertices[index++] = new VertexPositionColor(new Vector3(0, y, 0), Color);
                vertices[index++] = new VertexPositionColor(new Vector3(w, y, 0), Color);
            }

            VertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.None);
            VertexBuffer.SetData<VertexPositionColor>(vertices);
        }
    }
}
