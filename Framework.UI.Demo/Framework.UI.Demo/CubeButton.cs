#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo
{
    public sealed class CubeButton : Controls.Button
    {
        public Graphics.GeometricPrimitive CubePrimitive { get; set; }

        public float Scale { get; set; }

        public Matrix Orientation { get; set; }

        public CubeButton()
        {
            Scale = 1;
            Orientation = Matrix.Identity;
        }

        public override void Draw(GameTime gameTime, Rectangle renderBounds)
        {
            var graphicsDevice = Screen.UIContext.GraphicsDevice;
            var previousViewport = graphicsDevice.Viewport;

            var effect = Screen.UIContext.BasicEffect;

            var viewport = graphicsDevice.Viewport;
            viewport.X = renderBounds.X;
            viewport.Y = renderBounds.Y;
            viewport.Width = renderBounds.Width;
            viewport.Height = renderBounds.Height;
            graphicsDevice.Viewport = viewport;

            var cameraPosition = new Vector3(0, 0, 2.5f);
            var aspect = ((float) renderBounds.Width / (float) renderBounds.Height);

            effect.World = Orientation * Matrix.CreateScale(Scale);
            effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 0.1f, 10);
            effect.DiffuseColor = ForegroundColor.ToVector3();
            effect.Alpha = 1;
            effect.EnableDefaultLighting();

            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            //graphicsDevice.BlendState = BlendState.Opaque;
            //graphicsDevice.BlendState = BlendState.AlphaBlend;

            CubePrimitive.Draw(effect);

            graphicsDevice.Viewport = previousViewport;

            base.Draw(gameTime, renderBounds);
        }
    }
}
