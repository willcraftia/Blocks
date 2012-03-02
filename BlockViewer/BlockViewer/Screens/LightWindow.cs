#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;
using Willcraftia.Xna.Blocks.BlockViewer.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class LightWindow : Window
    {
        Button light0Button;

        Button light1Button;
        
        Button light2Button;

        ColorButton diffuse0Button;
        
        ColorButton diffuse1Button;
        
        ColorButton diffuse2Button;

        ColorButton specular0Button;

        ColorButton specular1Button;
        
        ColorButton specular2Button;

        BlockMeshViewModel ViewModel
        {
            get { return DataContext as BlockMeshViewModel; }
        }

        public LightWindow(Screen screen)
            : base(screen)
        {
            Width = 200;
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

            var title = new TextBlock(screen)
            {
                Text = Strings.LightModeTitle,
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

            var light0Panel = new StackPanel(screen)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            stackPanel.Children.Add(light0Panel);
            {
                light0Button = ControlUtil.CreateDefaultMenuButton(screen, Strings.Light0Button);
                light0Button.GotFocus += (Control s, ref RoutedEventContext c) =>
                {
                    ViewModel.ViewMode = ViewMode.DirectionalLight0;
                };
                light0Panel.Children.Add(light0Button);

                diffuse0Button = new ColorButton(screen)
                {
                    Width = 32,
                    Height = 32,
                    Margin = new Thickness(2)
                };
                light0Panel.Children.Add(diffuse0Button);

                specular0Button = new ColorButton(screen)
                {
                    Width = 32,
                    Height = 32,
                    Margin = new Thickness(2)
                };
                light0Panel.Children.Add(specular0Button);
            }

            var light1Panel = new StackPanel(screen)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            stackPanel.Children.Add(light1Panel);
            {
                light1Button = ControlUtil.CreateDefaultMenuButton(screen, Strings.Light1Button);
                light1Button.GotFocus += (Control s, ref RoutedEventContext c) =>
                {
                    ViewModel.ViewMode = ViewMode.DirectionalLight1;
                };
                light1Panel.Children.Add(light1Button);

                diffuse1Button = new ColorButton(screen)
                {
                    Width = 32,
                    Height = 32,
                    Margin = new Thickness(2)
                };
                light1Panel.Children.Add(diffuse1Button);

                specular1Button = new ColorButton(screen)
                {
                    Width = 32,
                    Height = 32,
                    Margin = new Thickness(2)
                };
                light1Panel.Children.Add(specular1Button);
            }

            var light2Panel = new StackPanel(screen)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            stackPanel.Children.Add(light2Panel);
            {
                light2Button = ControlUtil.CreateDefaultMenuButton(screen, Strings.Light2Button);
                light2Button.GotFocus += (Control s, ref RoutedEventContext c) =>
                {
                    ViewModel.ViewMode = ViewMode.DirectionalLight2;
                };
                light2Panel.Children.Add(light2Button);

                diffuse2Button = new ColorButton(screen)
                {
                    Width = 32,
                    Height = 32,
                    Margin = new Thickness(2)
                };
                light2Panel.Children.Add(diffuse2Button);

                specular2Button = new ColorButton(screen)
                {
                    Width = 32,
                    Height = 32,
                    Margin = new Thickness(2)
                };
                light2Panel.Children.Add(specular2Button);
            }
        }

        public override void Update(GameTime gameTime)
        {
            UpdateLightColorButtons(ViewModel.Light0ViewModel, diffuse0Button, specular0Button);
            UpdateLightColorButtons(ViewModel.Light1ViewModel, diffuse1Button, specular1Button);
            UpdateLightColorButtons(ViewModel.Light2ViewModel, diffuse2Button, specular2Button);

            base.Update(gameTime);
        }

        void UpdateLightColorButtons(LightViewModel lightViewModel, ColorButton diffuseButton, ColorButton specularButton)
        {
            var diffuse = lightViewModel.DiffuseColor;
            diffuseButton.ForegroundColor = new Color(diffuse.X, diffuse.Y, diffuse.Z);

            var specular = lightViewModel.SpecularColor;
            specularButton.ForegroundColor = new Color(specular.X, specular.Y, specular.Z);
        }

        public override void Show()
        {
            light0Button.Focus();

            base.Show();
        }

        public override void Close()
        {
            ViewModel.ViewMode = ViewMode.Camera;

            base.Close();
        }
    }
}
