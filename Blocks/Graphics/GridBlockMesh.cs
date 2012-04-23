#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    public sealed class GridBlockMesh : IDisposable
    {
        public GridPlane Up { get; private set; }
        public GridPlane Down { get; private set; }
        public GridPlane Forward { get; private set; }
        public GridPlane Backward { get; private set; }
        public GridPlane Left { get; private set; }
        public GridPlane Right { get; private set; }

        // MEMO
        //
        // BasicEffect.EnableDefaultLighting() が呼び出されると Normal0 が要求されるため、
        // 他とは共有しない専用の BasicEffect を使用します。
        //

        public BasicEffect Effect { get; private set; }

        public bool UpVisible { get; set; }
        public bool DownVisible { get; set; }
        public bool ForwardVisible { get; set; }
        public bool BackwardVisible { get; set; }
        public bool LeftVisible { get; set; }
        public bool RightVisible { get; set; }

        public GridBlockMesh(GraphicsDevice graphicsDevice, int gridSize, float cellSize, Color color)
        {
            UpVisible = true;
            DownVisible = true;
            ForwardVisible = true;
            BackwardVisible = true;
            LeftVisible = true;
            RightVisible = true;

            Effect = new BasicEffect(graphicsDevice);
            Effect.VertexColorEnabled = true;

            InitializePlanes(graphicsDevice, gridSize, cellSize, color);
        }

        public void SetVisibilities(Vector3 cameraPosition)
        {
            UpVisible = true;
            DownVisible = true;
            ForwardVisible = true;
            BackwardVisible = true;
            LeftVisible = true;
            RightVisible = true;

            if (0 < cameraPosition.Y) UpVisible = false;
            if (cameraPosition.Y < 0) DownVisible = false;

            if (0 < cameraPosition.Z) BackwardVisible = false;
            if (cameraPosition.Z < 0) ForwardVisible = false;

            if (0 < cameraPosition.X) RightVisible = false;
            if (cameraPosition.X < 0) LeftVisible = false;
        }

        public void Draw()
        {
            Draw(Effect);
        }

        public void Draw(Effect effect)
        {
            if (UpVisible) Up.Draw(effect);
            if (DownVisible) Down.Draw(effect);
            if (ForwardVisible) Forward.Draw(effect);
            if (BackwardVisible) Backward.Draw(effect);
            if (LeftVisible) Left.Draw(effect);
            if (RightVisible) Right.Draw(effect);
        }

        void InitializePlanes(GraphicsDevice graphicsDevice, int gridSize, float cellSize, Color color)
        {
            int halfGridSize = gridSize / 2;
            float offset = halfGridSize * cellSize;

            var upTransform = Matrix.CreateTranslation(new Vector3(0, offset, 0));
            Up = GridPlane.CreatePlaneXZ(graphicsDevice, gridSize, gridSize, cellSize, color, upTransform);

            var downTransform = Matrix.CreateTranslation(new Vector3(0, -offset, 0));
            Down = GridPlane.CreatePlaneXZ(graphicsDevice, gridSize, gridSize, cellSize, color, downTransform);

            var forwardTransform = Matrix.CreateTranslation(new Vector3(0, 0, -offset));
            Forward = GridPlane.CreatePlaneXY(graphicsDevice, gridSize, gridSize, cellSize, color, forwardTransform);

            var backwardTransform = Matrix.CreateTranslation(new Vector3(0, 0, offset));
            Backward = GridPlane.CreatePlaneXY(graphicsDevice, gridSize, gridSize, cellSize, color, backwardTransform);

            var leftTransform = Matrix.CreateTranslation(new Vector3(-offset, 0, 0));
            Left = GridPlane.CreatePlaneZY(graphicsDevice, gridSize, gridSize, cellSize, color, leftTransform);

            var rightTransform = Matrix.CreateTranslation(new Vector3(offset, 0, 0));
            Right = GridPlane.CreatePlaneZY(graphicsDevice, gridSize, gridSize, cellSize, color, rightTransform);
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool disposed;

        ~GridBlockMesh()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                Effect.Dispose();

                Up.Dispose();
                Down.Dispose();
                Forward.Dispose();
                Backward.Dispose();
                Left.Dispose();
                Right.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
