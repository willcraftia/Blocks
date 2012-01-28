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
    public sealed class LanguageSettingDialog : Overlay
    {
        Window window;

        public LanguageSettingDialog(Screen screen)
            : base(screen)
        {
            window = new Window(screen)
            {
                SizeToContent = SizeToContent.WidthAndHeight
            };
            window.Owner = this;

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                Padding = new Thickness(8)
            };
            window.Children.Add(stackPanel);

            var setDefaultButton = new Button(screen)
            {
                Padding = new Thickness(8)
            };
            {
                stackPanel.Children.Add(setDefaultButton);

                var textBlock = new TextBlock(screen)
                {
                    Text = Strings.DefaultButtonText
                };
                setDefaultButton.Children.Add(textBlock);

                setDefaultButton.GotFocus += (s, e) => textBlock.ForegroundColor = Color.Yellow;
                setDefaultButton.LostFocus += (s, e) => textBlock.ForegroundColor = Color.White;
            }

            var setJaButton = new Button(screen)
            {
                Padding = new Thickness(8)
            };
            {
                stackPanel.Children.Add(setJaButton);

                var textBlock = new TextBlock(screen)
                {
                    Text = Strings.JaButtonText
                };
                setJaButton.Children.Add(textBlock);

                setJaButton.GotFocus += (s, e) => textBlock.ForegroundColor = Color.Yellow;
                setJaButton.LostFocus += (s, e) => textBlock.ForegroundColor = Color.White;
            }

            var setEnButton = new Button(screen)
            {
                Padding = new Thickness(8)
            };
            {
                stackPanel.Children.Add(setEnButton);

                var textBlock = new TextBlock(screen)
                {
                    Text = Strings.EnButtonText
                };
                setEnButton.Children.Add(textBlock);

                setEnButton.GotFocus += (s, e) => textBlock.ForegroundColor = Color.Yellow;
                setEnButton.LostFocus += (s, e) => textBlock.ForegroundColor = Color.White;
            }
        }

        public override void Show()
        {
            base.Show();

            window.Show();
        }

        protected override bool OnKeyDown()
        {
            if (Screen.KeyboardDevice.IsKeyPressed(Keys.Escape)) Close();

            return base.OnKeyDown();
        }
    }
}
