#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class LightWindow : Window
    {
        public LightWindow(Screen screen)
            : base(screen)
        {
            ShadowOffset = new Vector2(4);
            Padding = new Thickness(16);
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            Content = stackPanel;

            var lightPanel = new StackPanel(screen);
            stackPanel.Children.Add(lightPanel);

            var light0Button = ControlUtil.CreateDefaultMenuButton(screen, "Light #0");
            lightPanel.Children.Add(light0Button);

            var separator = ControlUtil.CreateDefaultSeparator(screen);
            stackPanel.Children.Add(separator);

            var okButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.OKButton);
            okButton.Click += new RoutedEventHandler(OnOkButtonClick);
            stackPanel.Children.Add(okButton);

            okButton.Focus();
        }

        void OnOkButtonClick(Control sender, ref RoutedEventContext context)
        {
            Close();
        }
    }
}
