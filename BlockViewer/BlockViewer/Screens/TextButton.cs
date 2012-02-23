#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public class TextButton : Button
    {
        public TextBlock TextBlock { get; private set; }

        public TextButton(Screen screen)
            : base(screen)
        {
            TextBlock = new TextBlock(screen)
            {
                ForegroundColor = Color.White
            };
            Content = TextBlock;

            HorizontalAlignment = HorizontalAlignment.Left;
        }
    }
}
