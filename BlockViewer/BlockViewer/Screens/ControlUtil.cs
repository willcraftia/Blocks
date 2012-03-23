#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public static class ControlUtil
    {
        const float menuButtonHeight = 32;

        const float dialogButtonHeight = 32;

        public static Button CreateDefaultMenuButton(Screen screen, String text)
        {
            var button = new Button(screen)
            {
                Height = menuButtonHeight,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Padding = new Thickness(4),

                Content = new TextBlock(screen)
                {
                    Text = text,
                    ForegroundColor = Color.White,
                    BackgroundColor = Color.Black,
                    ShadowOffset = new Vector2(2)
                }
            };

            SetDefaultBehavior(button);

            return button;
        }

        public static Button CreateDefaultDialogButton(Screen screen, String text)
        {
            var button = new Button(screen)
            {
                Height = menuButtonHeight,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Padding = new Thickness(4),

                Content = new TextBlock(screen)
                {
                    Text = text,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    ForegroundColor = Color.White,
                    BackgroundColor = Color.Black,
                    ShadowOffset = new Vector2(2)
                }
            };

            SetDefaultBehavior(button);

            return button;
        }

        public static Image CreateDefaultSeparator(Screen screen)
        {
            return new Image(screen)
            {
                Texture = screen.Content.Load<Texture2D>("UI/Separator"),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 8, 0, 4)
            };
        }

        public static void SetDefaultBehavior(Button button)
        {
            float focusedTextOpacity = 1;
            float defocusedTextOpacity = 1;

            button.Content.Opacity = defocusedTextOpacity;

            button.GotFocus += (Control s, ref RoutedEventContext c) =>
            {
                button.Content.Opacity = focusedTextOpacity;
            };
            button.LostFocus += (Control s, ref RoutedEventContext c) =>
            {
                button.Content.Opacity = defocusedTextOpacity;
            };

            button.EnabledChanged += (s, e) =>
            {
                if (button.Enabled)
                {
                    button.Content.ForegroundColor = Color.White;
                }
                else
                {
                    button.Content.ForegroundColor = Color.Gray;
                }
            };

            ControlUtil.PopContentFontOnGotFocus(button);
        }

        static void PopContentFontOnGotFocus(Button button)
        {
            var animation = new Vector2LerpAnimation
            {
                Action = (current) => { button.Content.FontStretch = current; },
                From = Vector2.One,
                To = new Vector2(1.2f),
                Duration = TimeSpan.FromSeconds(0.1f),
                AutoReversed = true
            };
            button.Animations.Add(animation);
            button.GotFocus += (Control s, ref RoutedEventContext c) =>
            {
                animation.Enabled = true;
            };
            button.LostFocus += (Control s, ref RoutedEventContext c) =>
            {
                animation.Enabled = false;
            };
        }
    }
}
