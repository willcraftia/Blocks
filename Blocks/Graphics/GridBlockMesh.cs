#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    public sealed class GridBlockMesh
    {
        public GridPlane Top { get; private set; }
        public GridPlane Bottom { get; private set; }
        public GridPlane North { get; private set; }
        public GridPlane South { get; private set; }
        public GridPlane West { get; private set; }
        public GridPlane East { get; private set; }

        public bool TopVisible { get; set; }
        public bool BottomVisible { get; set; }
        public bool NorthVisible { get; set; }
        public bool SouthVisible { get; set; }
        public bool WestVisible { get; set; }
        public bool EastVisible { get; set; }

        public GridBlockMesh(GraphicsDevice graphicsDevice, int gridSize, float cellSize, Color color)
        {
            TopVisible = true;
            BottomVisible = true;
            NorthVisible = true;
            SouthVisible = true;
            WestVisible = true;
            EastVisible = true;

            InitializePlanes(graphicsDevice, gridSize, cellSize, color);
        }

        public void SetVisibilities(Vector3 cameraPosition)
        {
            TopVisible = true;
            BottomVisible = true;
            NorthVisible = true;
            SouthVisible = true;
            WestVisible = true;
            EastVisible = true;

            if (0 < cameraPosition.Y) TopVisible = false;
            if (cameraPosition.Y < 0) BottomVisible = false;

            if (0 < cameraPosition.Z) SouthVisible = false;
            if (cameraPosition.Z < 0) NorthVisible = false;

            if (0 < cameraPosition.X) EastVisible = false;
            if (cameraPosition.X < 0) WestVisible = false;
        }

        public void Draw(Effect effect)
        {
            if (TopVisible) Top.Draw(effect);
            if (BottomVisible) Bottom.Draw(effect);
            if (NorthVisible) North.Draw(effect);
            if (SouthVisible) South.Draw(effect);
            if (WestVisible) West.Draw(effect);
            if (EastVisible) East.Draw(effect);
        }

        void InitializePlanes(GraphicsDevice graphicsDevice, int gridSize, float cellSize, Color color)
        {
            int halfGridSize = gridSize / 2;
            float offset = halfGridSize * cellSize;

            var topTransform = Matrix.CreateTranslation(new Vector3(0, offset, 0));
            Top = GridPlane.CreatePlaneXZ(graphicsDevice, gridSize, gridSize, cellSize, color, topTransform);

            var bottomTransform = Matrix.CreateTranslation(new Vector3(0, -offset, 0));
            Bottom = GridPlane.CreatePlaneXZ(graphicsDevice, gridSize, gridSize, cellSize, color, bottomTransform);

            var northTransform = Matrix.CreateTranslation(new Vector3(0, 0, -offset));
            North = GridPlane.CreatePlaneXY(graphicsDevice, gridSize, gridSize, cellSize, color, northTransform);

            var southTransform = Matrix.CreateTranslation(new Vector3(0, 0, offset));
            South = GridPlane.CreatePlaneXY(graphicsDevice, gridSize, gridSize, cellSize, color, southTransform);

            var westTransform = Matrix.CreateTranslation(new Vector3(-offset, 0, 0));
            West = GridPlane.CreatePlaneZY(graphicsDevice, gridSize, gridSize, cellSize, color, westTransform);

            var eastTransform = Matrix.CreateTranslation(new Vector3(offset, 0, 0));
            East = GridPlane.CreatePlaneZY(graphicsDevice, gridSize, gridSize, cellSize, color, eastTransform);
        }
    }
}
