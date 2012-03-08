#region Using

using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class SelectLanguageDialog : OverlayDialogBase
    {
        static CultureInfo cultureJa = new CultureInfo("ja");

        static CultureInfo cultureEn = new CultureInfo("en");

        FloatLerpAnimation openAnimation;

        public SelectLanguageDialog(Screen screen)
            : base(screen)
        {
            // 開く際に openAnimation で Width を設定するので 0 で初期化します。
            Width = 0;
            ShadowOffset = new Vector2(4);
            Padding = new Thickness(16);

            Overlay.Opacity = 0.5f;

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            Content = stackPanel;

            var title = new TextBlock(screen)
            {
                Text = Strings.SelectLanguageTitle,
                Padding = new Thickness(4),
                ForegroundColor = Color.Yellow,
                BackgroundColor = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextHorizontalAlignment = HorizontalAlignment.Left,
                ShadowOffset = new Vector2(2)
            };
            stackPanel.Children.Add(title);

            var separator = ControlUtil.CreateDefaultSeparator(screen);
            stackPanel.Children.Add(separator);

            var jaButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.JaButton);
            jaButton.Click += OnJaButtonClick;
            stackPanel.Children.Add(jaButton);

            var enButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.EnButton);
            enButton.Click += OnEnButtonClick;
            stackPanel.Children.Add(enButton);

            var defaultButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.DefaultButton);
            defaultButton.Click += OnDefaultButtonClick;
            stackPanel.Children.Add(defaultButton);

            var cancelButon = ControlUtil.CreateDefaultMenuButton(screen, Strings.CancelButton);
            cancelButon.Click += (Control s, ref RoutedEventContext c) => Close();
            stackPanel.Children.Add(cancelButon);

            openAnimation = new FloatLerpAnimation
            {
                Action = (current) => { Width = current; },
                From = 0,
                To = 240,
                Duration = TimeSpan.FromSeconds(0.1f)
            };
            Animations.Add(openAnimation);

            cancelButon.Focus();
        }

        public override void Show()
        {
            openAnimation.Enabled = true;
            base.Show();
        }

        void OnDefaultButtonClick(Control sender, ref RoutedEventContext context)
        {
            Complete(CultureInfo.CurrentCulture);
        }

        void OnJaButtonClick(Control sender, ref RoutedEventContext context)
        {
            Complete(cultureJa);
        }

        void OnEnButtonClick(Control sender, ref RoutedEventContext context)
        {
            Complete(cultureEn);
        }

        void Complete(CultureInfo culture)
        {
            Strings.Culture = culture;
            Screen.ShowScreen(ScreenNames.Start);
        }
    }
}
