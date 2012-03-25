#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens.Box
{
    public sealed class BoxProgressDialog : OverlayDialogBase
    {
        TextBlock textBlock;

        public string Message
        {
            get { return textBlock.Text; }
            set { textBlock.Text = value; }
        }

        public BoxProgressDialog(Screen scree)
            : base(scree)
        {
            Height = 96;
            Overlay.Opacity = 0.5f;

            textBlock = new TextBlock(Screen)
            {
                ForegroundColor = Color.White,
                BackgroundColor = Color.Black,
                ShadowOffset = new Vector2(2),
                Padding = new Thickness(16)
            };
            Content = textBlock;
        }
    }
}
