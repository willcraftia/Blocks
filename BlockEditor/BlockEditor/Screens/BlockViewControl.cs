#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Blocks.BlockEditor.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Screens
{
    public sealed class BlockViewControl : Control
    {
        ViewerViewModel ViewModel
        {
            get { return DataContext as ViewerViewModel; }
        }

        public BlockViewControl(Screen screen)
            : base(screen)
        {
            Focusable = true;
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
