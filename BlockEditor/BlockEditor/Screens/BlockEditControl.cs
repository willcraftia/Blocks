#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Blocks.BlockEditor.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Screens
{
    public sealed class BlockEditControl : Control
    {
        BlockEditViewModel ViewModel
        {
            get { return DataContext as BlockEditViewModel; }
        }

        public BlockEditControl(Screen screen)
            : base(screen)
        {
        }

        public override void Draw(GameTime gameTime, IDrawContext drawContext)
        {
            base.Draw(gameTime, drawContext);

            var bounds = new Rect(0, 0, RenderSize.Width, RenderSize.Height);
            using (var draw3d = drawContext.BeginDraw3D())
            {
                using (var localViewport = drawContext.BeginViewport(bounds))
                {
                    using (var localClipping = drawContext.BeginNewClip(bounds))
                    {
                        ViewModel.Draw();
                    }
                }
            }
        }
    }
}
