#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;
using Willcraftia.Xna.Blocks.BlockViewer.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class DirectionalLightWindow : Window
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
            }
        }

        #endregion

        TextBlock titleTextBlock;

        LightColorButton diffuseColorButton;

        LightColorButton specularColorButton;

        PredefinedColorDialog predefinedColorDialog;

        DirectionalLightViewModel ViewModel
        {
            get { return DataContext as DirectionalLightViewModel; }
        }

        public DirectionalLightWindow(Screen screen)
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

            titleTextBlock = new TextBlock(screen)
            {
                Padding = new Thickness(4),
                ForegroundColor = Color.Yellow,
                BackgroundColor = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextHorizontalAlignment = HorizontalAlignment.Left,
                ShadowOffset = new Vector2(2)
            };
            stackPanel.Children.Add(titleTextBlock);

            var separator = ControlUtil.CreateDefaultSeparator(screen);
            stackPanel.Children.Add(separator);

            diffuseColorButton = new LightColorButton(screen);
            diffuseColorButton.NameTextBlock.Text = Strings.DiffuseColorLabel;
            diffuseColorButton.Click += OnDiffuseColorButtonClick;
            stackPanel.Children.Add(diffuseColorButton);

            specularColorButton = new LightColorButton(screen);
            specularColorButton.NameTextBlock.Text = Strings.SpecularColorLabel;
            specularColorButton.Click += OnSpecularColorButtonClick;
            stackPanel.Children.Add(specularColorButton);
        }

        public override void Show()
        {
            switch (ViewModel.Index)
            {
                case 0:
                    titleTextBlock.Text = Strings.Light0Title;
                    break;
                case 1:
                    titleTextBlock.Text = Strings.Light1Title;
                    break;
                case 2:
                    titleTextBlock.Text = Strings.Light2Title;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            var diffuse = ViewModel.DiffuseColor;
            diffuseColorButton.ColorCanvas.BackgroundColor = new Color(diffuse.X, diffuse.Y, diffuse.Z);

            var specular = ViewModel.SpecularColor;
            specularColorButton.ColorCanvas.BackgroundColor = new Color(specular.X, specular.Y, specular.Z);

            diffuseColorButton.Focus();

            base.Show();
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
            ViewModel.DiffuseColor = color.ToVector3();
        }

        void PredefinedColorSelectedForSpecular(PredefinedColor predefinedColor)
        {
            var color = predefinedColor.Color;

            // Diffuse ボタンに反映します。
            specularColorButton.ColorCanvas.BackgroundColor = color;
            // モデルに反映します。
            ViewModel.SpecularColor = color.ToVector3();
        }
    }
}
