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
        Button setDefaultButton;

        Button setJaButton;

        Button setEnButton;

        FloatLerpAnimation openAnimation;

        public SelectLanguageDialog(Screen screen)
            : base(screen)
        {
            // 開く際に openAnimation で Width を設定するので 0 で初期化します。
            Width = 0;
            ShadowOffset = new Vector2(4);
            Padding = new Thickness(16);

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

            var separator = new Image(screen)
            {
                Texture = screen.Content.Load<Texture2D>("UI/Separator"),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 4)
            };
            stackPanel.Children.Add(separator);

            setDefaultButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.DefaultButton);
            setDefaultButton.Click += new RoutedEventHandler(OnButtonClick);
            stackPanel.Children.Add(setDefaultButton);

            setJaButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.JaButton);
            setJaButton.Click += new RoutedEventHandler(OnButtonClick);
            stackPanel.Children.Add(setJaButton);

            setEnButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.EnButton);
            setEnButton.Click += new RoutedEventHandler(OnButtonClick);
            stackPanel.Children.Add(setEnButton);

            openAnimation = new FloatLerpAnimation
            {
                Action = (current) => { Width = current; },
                From = 0,
                To = 240,
                Duration = TimeSpan.FromSeconds(0.1f)
            };
            Animations.Add(openAnimation);

            // デフォルト フォーカス。
            setDefaultButton.Focus();

            Overlay.Opacity = 0.5f;
        }

        protected override void OnVisibleChanged()
        {
            // 表示されたら openAnimation を実行します。
            if (Visible) openAnimation.Enabled = true;

            base.OnVisibleChanged();
        }

        void OnButtonClick(Control sender, ref RoutedEventContext context)
        {
            var button = sender as Button;

            CultureInfo culture = null;

            if (button == setDefaultButton)
            {
                culture = CultureInfo.CurrentCulture;
            }
            else if (button == setJaButton)
            {
                culture = new CultureInfo("ja");
            }
            else if (button == setEnButton)
            {
                culture = new CultureInfo("en");
            }

            if (culture != null) Strings.Culture  = culture;

            var uiService = Screen.Game.Services.GetRequiredService<IUIService>();
            uiService.Show(ScreenNames.Start);
        }
    }
}
