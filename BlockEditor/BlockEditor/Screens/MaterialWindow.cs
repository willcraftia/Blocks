#region Using

using System;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Screens
{
    public sealed class MaterialWindow : Window
    {
        public MaterialWindow(Screen screen)
            : base(screen)
        {
            var stackPanel = new StackPanel(screen)
            {
            };
            Content = stackPanel;


        }
    }
}
