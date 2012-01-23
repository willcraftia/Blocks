#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo.Screens
{
    public sealed class WindowDemoScreen : Screen
    {
        #region FirstWindow

        class FirstWindow : Window
        {
            public FirstWindow(Screen screen)
                : base(screen)
            {
                Width = unit * 10;
                Height = unit * 10;
                HorizontalAlignment = HorizontalAlignment.Left;
                VerticalAlignment = VerticalAlignment.Top;
                Margin = new Thickness(unit, unit, 0, 0);
                BackgroundColor = Color.Red;

                var firstWindow_widthAnimation = new PropertyLerpAnimation
                {
                    Target = this,
                    PropertyName = "Width",
                    From = 0,
                    To = Width,
                    Repeat = Repeat.Forever,
                    BeginTime = TimeSpan.Zero,
                    Duration = TimeSpan.FromSeconds(2),
                    Enabled = true
                };
                screen.Animations.Add(firstWindow_widthAnimation);

                var firstWindow_HeightAnimation = new PropertyLerpAnimation
                {
                    Target = this,
                    PropertyName = "Height",
                    From = 0,
                    To = Height,
                    Repeat = Repeat.Forever,
                    BeginTime = TimeSpan.Zero,
                    Duration = TimeSpan.FromSeconds(2),
                    Enabled = true
                };
                screen.Animations.Add(firstWindow_HeightAnimation);
            }
        }

        #endregion

        #region SecondWindow

        class SecondWindow : Window
        {
            public SecondWindow(Screen screen)
                : base(screen)
            {
                Width = unit * 10;
                Height = unit * 10;
                HorizontalAlignment = HorizontalAlignment.Right;
                VerticalAlignment = VerticalAlignment.Bottom;
                Margin = new Thickness(0, 0, unit, unit);
                BackgroundColor = Color.Green;

                var opacityAnimation = new PropertyLerpAnimation
                {
                    Target = this,
                    PropertyName = "Opacity",
                    From = 0,
                    To = 1,
                    Repeat = Repeat.Forever,
                    BeginTime = TimeSpan.Zero,
                    Duration = TimeSpan.FromSeconds(2),
                    AutoReversed = true,
                    Enabled = true
                };
                screen.Animations.Add(opacityAnimation);
            }
        }

        #endregion

        #region ThirdWindow

        class ThirdWindow : Window
        {
            public ThirdWindow(Screen screen)
                : base(screen)
            {
                Width = unit * 15;
                Height = unit * 10;
                BackgroundColor = Color.Blue;

                var stackPanel = new StackPanel(screen)
                {
                    Margin = new Thickness(8),
                    Orientation = Orientation.Horizontal
                };
                Children.Add(stackPanel);

                var openNewDialogButton = new Button(screen)
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(8)
                };
                stackPanel.Children.Add(openNewDialogButton);
                openNewDialogButton.Click += (s, e) =>
                {
                    var firstDialog = new FirstDialog(screen);
                    firstDialog.Show();
                };
                var openNewDialogTextBlock = new TextBlock(screen)
                {
                    Text = "Open new dialog",
                    FontStretch = new Vector2(1.0f, 2.0f),
                    Padding = new Thickness(8)
                };
                openNewDialogButton.Children.Add(openNewDialogTextBlock);

                var cubeControl = new CubeControl(screen)
                {
                    CubePrimitive = CreateCubePrimitive(),
                    Clipped = false,
                    ForegroundColor = Color.White,
                    Width = unit * 7
                };
                stackPanel.Children.Add(cubeControl);

                var rotateCubeTimelineAnimation = new RotateCubeAnimation
                {
                    CubeButton = cubeControl,
                    From = 0,
                    To = MathHelper.TwoPi,
                    BeginTime = TimeSpan.Zero,
                    Duration = TimeSpan.FromSeconds(4),
                    Repeat = Repeat.Forever
                };
                screen.Animations.Add(rotateCubeTimelineAnimation);

                cubeControl.MouseEnter += (s, e) =>
                {
                    cubeControl.Scale = 1.5f;
                    rotateCubeTimelineAnimation.Enabled = true;
                };
                cubeControl.MouseLeave += (s, e) =>
                {
                    cubeControl.Scale = 1;
                    rotateCubeTimelineAnimation.Enabled = false;
                };

                var thirdWindow_widthAnimation = new PropertyLerpAnimation
                {
                    Target = this,
                    PropertyName = "Width",
                    From = 0,
                    To = Width,
                    BeginTime = TimeSpan.Zero,
                    Duration = TimeSpan.FromSeconds(1),
                    Enabled = true
                };
                screen.Animations.Add(thirdWindow_widthAnimation);

                var thirdWindow_heightAnimation = new PropertyLerpAnimation
                {
                    Target = this,
                    PropertyName = "Height",
                    From = 0,
                    To = Height,
                    BeginTime = TimeSpan.Zero,
                    Duration = TimeSpan.FromSeconds(1),
                    Enabled = true
                };
                screen.Animations.Add(thirdWindow_heightAnimation);
            }

            /// <summary>
            /// 立方体の GeometricPrimitive を生成します。
            /// </summary>
            /// <returns>生成された立方体の GeometricPrimitive。</returns>
            Graphics.GeometricPrimitive CreateCubePrimitive()
            {
                //var cubeVertexSourceFactory = new Graphics.CubeVertexSourceFactory();
                var cubeVertexSourceFactory = new Graphics.ColoredCubeVertexSourceFactory();
                cubeVertexSourceFactory.TopSurfaceColor = Color.Green;
                cubeVertexSourceFactory.BottomSurfaceColor = Color.GreenYellow;
                cubeVertexSourceFactory.NorthSurfaceColor = Color.Blue;
                cubeVertexSourceFactory.SouthSurfaceColor = Color.BlueViolet;
                cubeVertexSourceFactory.EastSurfaceColor = Color.Red;
                cubeVertexSourceFactory.WestSurfaceColor = Color.OrangeRed;

                var source = cubeVertexSourceFactory.CreateVertexSource();
                return Graphics.GeometricPrimitive.Create(Screen.GraphicsDevice, source);
            }
        }

        #endregion

        #region FirstDialog

        class FirstDialog : Overlay
        {
            Window window;

            public FirstDialog(Screen screen)
                : base(screen)
            {
                Opacity = 0.5f;
                BackgroundColor = Color.Black;

                window = new Window(screen)
                {
                    Width = unit * 7,
                    SizeToContent = SizeToContent.Height,
                    BackgroundColor = Color.Green
                };
                // Owner を関連付けると Owner が閉じると自動的に閉じます。
                window.Owner = this;

                var stackPanel = new StackPanel(screen)
                {
                    Margin = new Thickness(8),
                    Orientation = Orientation.Vertical
                };
                window.Children.Add(stackPanel);

                var openNewDialogButton = new Button(screen)
                {
                    Margin = new Thickness(8),
                    Padding = new Thickness(8),
                    Height = unit,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                openNewDialogButton.Click += (s, e) =>
                {
                    var secondDialog = new SecondDialog(screen);
                    secondDialog.Show();
                };
                stackPanel.Children.Add(openNewDialogButton);
                var openNewDialogTextBlock = new TextBlock(screen)
                {
                    Text = "Open new dialog",
                    TextHorizontalAlignment = HorizontalAlignment.Left
                };
                openNewDialogButton.Children.Add(openNewDialogTextBlock);

                var closeButton = new Button(screen)
                {
                    Margin = new Thickness(8),
                    Padding = new Thickness(8),
                    Height = unit,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                closeButton.Click += (s, e) => Close();
                stackPanel.Children.Add(closeButton);
                var closeTextBlock = new TextBlock(screen)
                {
                    Text = "Close",
                    TextHorizontalAlignment = HorizontalAlignment.Left
                };
                closeButton.Children.Add(closeTextBlock);

                var switchScreenButton = new Button(screen)
                {
                    Margin = new Thickness(8),
                    Padding = new Thickness(8),
                    Height = unit,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                switchScreenButton.Click += new EventHandler(OnSwitchScreenButtonClick);
                stackPanel.Children.Add(switchScreenButton);
                var switchScreenTextBlock = new TextBlock(screen)
                {
                    Text = "Switch Screen",
                    TextHorizontalAlignment = HorizontalAlignment.Left
                };
                switchScreenButton.Children.Add(switchScreenTextBlock);

                var exitButton = new Button(screen)
                {
                    Margin = new Thickness(8),
                    Padding = new Thickness(8),
                    Height = unit,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                exitButton.Click += new EventHandler(OnExitButtonClick);
                stackPanel.Children.Add(exitButton);
                var exitTextBlock = new TextBlock(screen)
                {
                    Text = "Exit",
                    TextHorizontalAlignment = HorizontalAlignment.Left
                };
                exitButton.Children.Add(exitTextBlock);
            }

            void OnExitButtonClick(object sender, EventArgs e)
            {
                var overlay = new Overlay(Screen)
                {
                    Opacity = 0,
                    BackgroundColor = Color.Black
                };
                overlay.Show();

                var opacityAnimation = new PropertyLerpAnimation
                {
                    Target = overlay,
                    PropertyName = "Opacity",
                    From = 0,
                    To = 1,
                    BeginTime = TimeSpan.Zero,
                    Duration = TimeSpan.FromSeconds(0.5d),
                    Enabled = true
                };
                opacityAnimation.Completed += (s, evt) => Screen.Game.Exit();
                Screen.Animations.Add(opacityAnimation);
            }

            void OnSwitchScreenButtonClick(object sender, EventArgs e)
            {
                var overlay = new Overlay(Screen)
                {
                    Opacity = 0,
                    BackgroundColor = Color.Black
                };
                overlay.Show();

                var opacityAnimation = new PropertyLerpAnimation
                {
                    Target = overlay,
                    PropertyName = "Opacity",
                    From = 0,
                    To = 1,
                    BeginTime = TimeSpan.Zero,
                    Duration = TimeSpan.FromSeconds(0.5d),
                    Enabled = true
                };
                opacityAnimation.Completed += (s, evt) =>
                {
                    var uiService = Screen.Game.Services.GetRequiredService<IUIService>();
                    uiService.Show("MainMenuDemoScreen");
                };
                Screen.Animations.Add(opacityAnimation);
            }

            public override void Show()
            {
                base.Show();

                window.Show();
            }
        }

        #endregion

        #region SecondDialog

        class SecondDialog : Overlay
        {
            Window window;

            public SecondDialog(Screen screen)
                : base(screen)
            {
                Opacity = 0.5f;
                BackgroundColor = Color.Black;

                window = new Window(screen)
                {
                    SizeToContent = SizeToContent.WidthAndHeight,
                    BackgroundColor = Color.Brown
                };
                // Owner を関連付けると Owner が閉じると自動的に閉じます。
                window.Owner = this;

                var closeButton = new Button(screen)
                {
                    Width = unit * 2,
                    Height = unit,
                    Margin = new Thickness(unit)
                };
                closeButton.Click += (s, e) => Close();
                window.Children.Add(closeButton);
                var closeTextBlock = new TextBlock(screen)
                {
                    Text = "Close",
                    TextHorizontalAlignment = HorizontalAlignment.Left
                };
                closeButton.Children.Add(closeTextBlock);
            }

            public override void Show()
            {
                base.Show();

                window.Show();
            }
        }

        #endregion

        const int unit = 32;

        public WindowDemoScreen(Game game)
            : base(game)
        {
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            // 重いロードのテスト用にスリープさせてます。
            System.Threading.Thread.Sleep(2000);

            var viewportBounds = GraphicsDevice.Viewport.TitleSafeArea;
            Desktop.BackgroundColor = Color.CornflowerBlue;
            Desktop.Margin = new Thickness(viewportBounds.Left, viewportBounds.Top, 0, 0);
            Desktop.Width = viewportBounds.Width;
            Desktop.Height = viewportBounds.Height;

            var firstWindow = new FirstWindow(this);
            firstWindow.Show();

            var secondWindow = new SecondWindow(this);
            secondWindow.Show();

            var thirdWindow = new ThirdWindow(this);
            thirdWindow.Show();

            var startEffectOverlay = new Overlay(this)
            {
                Opacity = 1,
                BackgroundColor = Color.Black
            };
            startEffectOverlay.Show();

            var startEffectOverlay_opacityAnimation = new PropertyLerpAnimation
            {
                Target = startEffectOverlay,
                PropertyName = "Opacity",
                From = 1,
                To = 0,
                BeginTime = TimeSpan.Zero,
                Duration = TimeSpan.FromSeconds(0.5d),
                Enabled = true
            };
            startEffectOverlay_opacityAnimation.Completed += (s, e) =>
            {
                startEffectOverlay.Close();
                thirdWindow.Activate();
            };
            Animations.Add(startEffectOverlay_opacityAnimation);

            base.LoadContent();
        }
    }
}
