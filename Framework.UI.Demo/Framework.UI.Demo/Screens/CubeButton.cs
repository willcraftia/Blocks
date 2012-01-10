﻿#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo.Screens
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

        public override void Draw(GameTime gameTime, IDrawContext drawContext)
        {
            drawContext.Flush();

            var bounds = drawContext.Bounds;

            var graphicsDevice = Screen.GraphicsDevice;
            var previousViewport = graphicsDevice.Viewport;
            var newBounds = Rectangle.Intersect(previousViewport.Bounds, bounds);
            graphicsDevice.Viewport = new Viewport(newBounds);

            var effect = Screen.BasicEffect;

            var cameraPosition = new Vector3(0, 0, 2.5f);
            var aspect = ((float) bounds.Width / (float) bounds.Height);

            effect.World = Orientation * Matrix.CreateScale(Scale);
            effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 0.1f, 10);
            effect.VertexColorEnabled = true;
            //effect.DiffuseColor = ForegroundColor.ToVector3();
            effect.Alpha = 1;
            effect.EnableDefaultLighting();

            CubePrimitive.Draw(effect);

            graphicsDevice.Viewport = previousViewport;

            base.Draw(gameTime, drawContext);
        }
    }
}
