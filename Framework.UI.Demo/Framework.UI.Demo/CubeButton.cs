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

        public override void Draw(GameTime gameTime)
        {
            var graphicsDevice = Screen.UIContext.GraphicsDevice;
            var previousViewport = graphicsDevice.Viewport;

            var effect = Screen.UIContext.BasicEffect;

            float time = (float) gameTime.TotalGameTime.TotalSeconds;

            float yaw = time * 0.4f;
            float pitch = time * 0.7f;
            float roll = time * 1.1f;

            var cameraPosition = new Vector3(0, 0, 2.5f);

            var bounds = GetAbsoluteBounds();
            var viewport = graphicsDevice.Viewport;
            viewport.X = bounds.X;
            viewport.Y = bounds.Y;
            viewport.Width = bounds.Width;
            viewport.Height = bounds.Height;
            graphicsDevice.Viewport = viewport;

            var aspect = ((float) viewport.Width / (float) viewport.Height);

            effect.World = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);
            effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 10);
            effect.DiffuseColor = Color.White.ToVector3();
            effect.Alpha = 1;
            effect.EnableDefaultLighting();

            //graphicsDevice.DepthStencilState = DepthStencilState.Default;
            //graphicsDevice.BlendState = BlendState.Opaque;

            CubePrimitive.Draw(effect);

            graphicsDevice.Viewport = previousViewport;

            base.Draw(gameTime);
        }
    }
}
