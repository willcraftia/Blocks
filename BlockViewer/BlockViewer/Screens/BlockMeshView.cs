#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class BlockMeshView : Control
    {
        Vector3 cameraPosition = new Vector3(0, 0, 2.5f);

        public BlockMeshView(Screen screen)
            : base(screen)
        {
        }

        public override void Draw(GameTime gameTime, IDrawContext drawContext)
        {
            base.Draw(gameTime, drawContext);

            drawContext.Flush();

            var bounds = new Rect(0, 0, RenderSize.Width, RenderSize.Height);

            using (var setViewport = drawContext.SetViewport(bounds))
            {
                using (var setClip = drawContext.SetNewClip(bounds))
                {
                    var effect = Screen.BasicEffect;

                    var aspect = ((float) RenderSize.Width / (float) RenderSize.Height);

                    //effect.World = Orientation * Matrix.CreateScale(Scale);
                    //effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                    //effect.Projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 0.1f, 10);
                    //effect.DiffuseColor = ForegroundColor.ToVector3();
                    //// どうもデフォルトで Specular が設定されているようだ。
                    //effect.SpecularColor = Color.Black.ToVector3();
                    //effect.Alpha = 1;
                    //effect.EnableDefaultLighting();
                    //effect.VertexColorEnabled = true;

                    //CubePrimitive.Draw(effect);
                }
            }
        }
    }
}
