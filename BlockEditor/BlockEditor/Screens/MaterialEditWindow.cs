#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockEditor.Models;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Screens
{
    public sealed class MaterialEditWindow : OverlayDialogBase
    {
        #region LightColorButton

        class LightColorButton : Button
        {
            public TextBlock NameTextBlock { get; private set; }

            public Canvas ColorCanvas { get; private set; }

            public LightColorButton(Screen screen)
                : base(screen)
            {
                var stackPanel = new StackPanel(screen)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Padding = new Thickness(16, 0, 16, 0)
                };
                Content = stackPanel;

                NameTextBlock = new TextBlock(screen)
                {
                    Width = 140,
                    ForegroundColor = Color.White,
                    BackgroundColor = Color.Black,
                    ShadowOffset = new Vector2(2),
                    TextHorizontalAlignment = HorizontalAlignment.Left
                };
                stackPanel.Children.Add(NameTextBlock);

                ColorCanvas = new Canvas(screen)
                {
                    Width = 28,
                    Height = 28,
                    Margin = new Thickness(4)
                };
                stackPanel.Children.Add(ColorCanvas);

                // ControlUtil では対応できないので固有で設定します。
                PopupFontOnGotFocus();
            }

            void PopupFontOnGotFocus()
            {
                var animation = new Vector2LerpAnimation
                {
                    Action = (current) => { NameTextBlock.FontStretch = current; },
                    From = Vector2.One,
                    To = new Vector2(1.2f),
                    Duration = TimeSpan.FromSeconds(0.1f),
                    AutoReversed = true
                };
                Animations.Add(animation);
                GotFocus += (Control s, ref RoutedEventContext c) =>
                {
                    animation.Enabled = true;
                };
                LostFocus += (Control s, ref RoutedEventContext c) =>
                {
                    animation.Enabled = false;
                };
            }
        }

        #endregion

        LightColorButton diffuseColorButton;

        LightColorButton emissiveColorButton;

        LightColorButton specularColorButton;

        PredefinedColorDialog predefinedColorDialog;

        public MaterialEdit MaterialEdit
        {
            get { return DataContext as MaterialEdit; }
        }

        public MaterialEditWindow(Screen screen)
            : base(screen)
        {
            DataContext = new MaterialEdit();

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

            diffuseColorButton = new LightColorButton(screen);
            diffuseColorButton.NameTextBlock.Text = "Diffuse color";
            diffuseColorButton.Click += OnDiffuseColorButtonClick;
            stackPanel.Children.Add(diffuseColorButton);

            specularColorButton = new LightColorButton(screen);
            specularColorButton.NameTextBlock.Text = "Specular color";
            specularColorButton.Click += OnSpecularColorButtonClick;
            stackPanel.Children.Add(specularColorButton);
        }

        void OnDiffuseColorButtonClick(Control sender, ref RoutedEventContext context)
        {
            ShowPredefinedColorDialog(PredefinedColorSelectedForDiffuse);
        }

        void OnSpecularColorButtonClick(Control sender, ref RoutedEventContext context)
        {
            ShowPredefinedColorDialog(PredefinedColorSelectedForSpecular);
        }

        void ShowPredefinedColorDialog(PredefinedColorSelected callback)
        {
            if (predefinedColorDialog == null)
            {
                predefinedColorDialog = new PredefinedColorDialog(Screen);
                // A 値をライト色で用いることはできないため、選択範囲から除外しておきます。
                predefinedColorDialog.PredefinedColors.RemoveAll((p) => p.Color.A != 255);
            }
            // コールバックを設定してから表示します。
            predefinedColorDialog.Selected = callback;
            predefinedColorDialog.Show();
        }

        void PredefinedColorSelectedForDiffuse(PredefinedColor predefinedColor)
        {
            var color = predefinedColor.Color;

            // Diffuse ボタンに反映します。
            diffuseColorButton.ColorCanvas.BackgroundColor = color;
            // モデルに反映します。
            MaterialEdit.DiffuseColor = color;
        }

        void PredefinedColorSelectedForSpecular(PredefinedColor predefinedColor)
        {
            var color = predefinedColor.Color;

            // Diffuse ボタンに反映します。
            specularColorButton.ColorCanvas.BackgroundColor = color;
            // モデルに反映します。
            MaterialEdit.SpecularColor = color;
        }
    }
}
