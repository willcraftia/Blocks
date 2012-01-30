#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Input;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class LanguageSettingDialog : Window
    {
        public LanguageSettingDialog(Screen screen)
            : base(screen)
        {
            SizeToContent = SizeToContent.WidthAndHeight;

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                Padding = new Thickness(8)
            };
            Content = stackPanel;

            var setDefaultButton = new Button(screen)
            {
                Padding = new Thickness(8),
                Content = new TextBlock(screen)
                {
                    Text = Strings.DefaultButtonText
                }
            };
            stackPanel.Children.Add(setDefaultButton);

            var setJaButton = new Button(screen)
            {
                Padding = new Thickness(8),
                Content = new TextBlock(screen)
                {
                    Text = Strings.JaButtonText
                }
            };
            stackPanel.Children.Add(setJaButton);

            var setEnButton = new Button(screen)
            {
                Padding = new Thickness(8),
                Content = new TextBlock(screen)
                {
                    Text = Strings.EnButtonText
                }
            };
            stackPanel.Children.Add(setEnButton);
        }

        public override void Show()
        {
            base.Show();

            var overlay = new Overlay(Screen)
            {
                Opacity = 0.7f,
                BackgroundColor = Color.Black
            };
            overlay.Owner = this;
            overlay.Show();
        }

    }
}
