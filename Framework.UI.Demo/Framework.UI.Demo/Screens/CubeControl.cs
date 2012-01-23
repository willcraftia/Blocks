#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo.Screens
{
    public sealed class CubeControl : Control
    {
        public Graphics.GeometricPrimitive CubePrimitive { get; set; }

        public float Scale { get; set; }

        public Matrix Orientation { get; set; }

        public CubeControl(Screen screen)
            : base(screen)
        {
            Scale = 1;
            Orientation = Matrix.Identity;
        }

        public override void Draw(GameTime gameTime, IDrawContext drawContext)
        {
            // todo: 先に LaF を描画。
            base.Draw(gameTime, drawContext);

            drawContext.Flush();

            var bounds = drawContext.Bounds;

            using (var setViewport = drawContext.SetViewport(drawContext.Bounds))
            {
                var effect = Screen.BasicEffect;

                var cameraPosition = new Vector3(0, 0, 2.5f);
                var aspect = ((float) bounds.Width / (float) bounds.Height);

                effect.World = Orientation * Matrix.CreateScale(Scale);
                effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                effect.Projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 0.1f, 10);
                effect.DiffuseColor = ForegroundColor.ToVector3();
                // どうもデフォルトで Specular が設定されているようだ。
                effect.SpecularColor = Color.Black.ToVector3();
                effect.Alpha = 1;
                effect.EnableDefaultLighting();
                effect.VertexColorEnabled = true;

                CubePrimitive.Draw(effect);
            }
        }
    }
}
