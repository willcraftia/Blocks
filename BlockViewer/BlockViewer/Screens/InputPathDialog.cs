#region Using

using System;
using System.IO;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class InputPathDialog : Window
    {
        TextBox pathTextBox;

        public InputPathDialog(Screen screen)
            : base(screen)
        {
            Width = BlockViewerGame.SpriteSize * 10;

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                Padding = new Thickness(16)
            };
            Content = stackPanel;

            pathTextBox = new TextBox(screen)
            {
                FontStretch = new Vector2(0.5f),
                Text = Directory.GetCurrentDirectory()
            };
            stackPanel.Children.Add(pathTextBox);

            var buttonsPanel = new StackPanel(screen);
            stackPanel.Children.Add(buttonsPanel);

            var okButton = new CustomButton(screen)
            {
                Margin = new Thickness(0, 0, 4, 0)
            };
            okButton.TextBlock.Text = "OK";
            buttonsPanel.Children.Add(okButton);

            var cancelButton = new CustomButton(screen)
            {
                Margin = new Thickness(4, 0, 0, 0)
            };
            cancelButton.TextBlock.Text = "Cancel";
            buttonsPanel.Children.Add(cancelButton);

            cancelButton.Focus();
        }
    }
}
