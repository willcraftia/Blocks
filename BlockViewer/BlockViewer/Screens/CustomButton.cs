#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    // MEMO
    //
    // ちょっと効率のために OO 的な隠蔽の観点は無視。
    //

    public class CustomButton : Button
    {
        public TextBlock TextBlock { get; private set; }

        public CustomButton(Screen screen)
            : base(screen)
        {
            TextBlock = new TextBlock(screen)
            {
                ForegroundColor = Color.White
            };
            Content = TextBlock;

            HorizontalAlignment = HorizontalAlignment.Left;
        }

        protected override void OnGotFocus(ref RoutedEventContext context)
        {
            base.OnGotFocus(ref context);
        }

        protected override void OnLostFocus(ref RoutedEventContext context)
        {
            base.OnLostFocus(ref context);
        }

        public override void Draw(GameTime gameTime, IDrawContext drawContext)
        {
            if (Focused)
            {
                var renderBounds = new Rect(RenderSize);
                drawContext.DrawRectangle(renderBounds, Color.Green * 0.5f);
            }

            base.Draw(gameTime, drawContext);
        }
    }
}
