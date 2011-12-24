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

        Matrix view;

        Matrix projection;

        public CubeButton()
        {
            Scale = 1;
            Orientation = Matrix.Identity;
        }

        public override void Draw(GameTime gameTime)
        {
            var graphicsDevice = Screen.UIContext.GraphicsDevice;
            var previousViewport = graphicsDevice.Viewport;

            var effect = Screen.UIContext.BasicEffect;

            var bounds = RenderBounds;
            var viewport = graphicsDevice.Viewport;
            viewport.X = bounds.X;
            viewport.Y = bounds.Y;
            viewport.Width = bounds.Width;
            viewport.Height = bounds.Height;
            graphicsDevice.Viewport = viewport;

            effect.World = Orientation * Matrix.CreateScale(Scale);
            effect.View = view;
            effect.Projection = projection;
            effect.DiffuseColor = ForegroundColor.ToVector3();
            effect.Alpha = 1;
            effect.EnableDefaultLighting();

            //graphicsDevice.DepthStencilState = DepthStencilState.Default;
            //graphicsDevice.BlendState = BlendState.Opaque;

            CubePrimitive.Draw(effect);

            graphicsDevice.Viewport = previousViewport;

            base.Draw(gameTime);
        }

        protected override void OnRenderBoundsChanged()
        {
            var bounds = RenderBounds;
            var aspect = ((float) bounds.Width / (float) bounds.Height);

            var cameraPosition = new Vector3(0, 0, 2.5f);
            view = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 0.1f, 10);

            base.OnRenderBoundsChanged();
        }
    }
}
