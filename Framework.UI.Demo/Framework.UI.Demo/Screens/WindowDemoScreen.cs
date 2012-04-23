#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Lafs;
using Willcraftia.Xna.Framework.UI.Lafs.Debug;

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
                Padding = new Thickness(16);

                Content = new TextBlock(screen)
                {
                    Text = "1st Window",
                    ForegroundColor = Color.White,
                    BackgroundColor = Color.Black,
                    ShadowOffset = new Vector2(2),
                    TextOutlineWidth = 1,
                    FontStretch = new Vector2(1.5f),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextHorizontalAlignment = HorizontalAlignment.Left
                };

                var repeatedWidthAnimation = new FloatLerpAnimation
                {
                    Action = (current) => { Width = current; },
                    To = Width,
                    Repeat = Repeat.Forever,
                    Duration = TimeSpan.FromSeconds(2),
                    Enabled = true
                };
                Animations.Add(repeatedWidthAnimation);

                var repeatedHeightAnimation = new FloatLerpAnimation
                {
                    Action = (current) => { Height = current; },
                    To = Height,
                    Repeat = Repeat.Forever,
                    Duration = TimeSpan.FromSeconds(2),
                    Enabled = true
                };
                Animations.Add(repeatedHeightAnimation);
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
                Padding = new Thickness(16);

                Content = new TextBlock(screen)
                {
                    Text = "2nd Window",
                    ForegroundColor = Color.White,
                    BackgroundColor = Color.Black,
                    ShadowOffset = new Vector2(2),
                    TextOutlineWidth = 1,
                    FontStretch = new Vector2(1.5f),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    TextHorizontalAlignment = HorizontalAlignment.Right
                };

                var opacityAnimation = new FloatLerpAnimation
                {
                    Action = (current) => { Opacity = current; },
                    To = 1,
                    Repeat = Repeat.Forever,
                    Duration = TimeSpan.FromSeconds(2),
                    AutoReversed = true,
                    Enabled = true
                };
                Animations.Add(opacityAnimation);
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
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Margin = new Thickness(8)
                };
                Content = stackPanel;

                var title = new TextBlock(screen)
                {
                    Text = "3rd window",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    ForegroundColor = Color.White,
                    BackgroundColor = Color.Black,
                    ShadowOffset = new Vector2(2),
                    TextOutlineWidth = 1,
                    FontStretch = new Vector2(1.5f)
                };
                stackPanel.Children.Add(title);

                var changingLookAndFeelDemoButton = new Button(screen)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Padding = new Thickness(8),

                    Content = new TextBlock(screen)
                    {
                        Text = "Changing look & feel demo",
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black,
                        HorizontalAlignment = HorizontalAlignment.Left
                    }
                };
                changingLookAndFeelDemoButton.Click += (Control s, ref RoutedEventContext c) =>
                {
                    (screen as WindowDemoScreen).SwitchLookAndFeelSource();
                };
                stackPanel.Children.Add(changingLookAndFeelDemoButton);

                var overlayDialogDemoButton = new Button(screen)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Padding = new Thickness(8),

                    Content = new TextBlock(screen)
                    {
                        Text = "Overlay dialog demo",
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black,
                        HorizontalAlignment = HorizontalAlignment.Left
                    }
                };
                overlayDialogDemoButton.Click += (Control sender, ref RoutedEventContext context) =>
                {
                    var dialog = new FirstOverlayDialog(Screen);
                    dialog.Overlay.Opacity = 0.5f;
                    dialog.Show();
                };
                stackPanel.Children.Add(overlayDialogDemoButton);

                var drawing3DDemoButton = new Button(screen)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Padding = new Thickness(8),

                    Content = new TextBlock(screen)
                    {
                        Text = "Drawing 3D demo",
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black,
                        HorizontalAlignment = HorizontalAlignment.Left
                    }
                };
                drawing3DDemoButton.Click += (Control s, ref RoutedEventContext c) =>
                {
                    var window = new CubeWindow(Screen);
                    window.Show();
                };
                stackPanel.Children.Add(drawing3DDemoButton);

                var listBoxDemoButton = new Button(screen)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Padding = new Thickness(8),

                    Content = new TextBlock(screen)
                    {
                        Text = "ListBox demo",
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black,
                        HorizontalAlignment = HorizontalAlignment.Left
                    }
                };
                listBoxDemoButton.Click += (Control s, ref RoutedEventContext c) =>
                {
                    var window = new ListBoxDemoWindow(screen);
                    window.Show();
                };
                stackPanel.Children.Add(listBoxDemoButton);

                var switchScreenButton = new Button(screen)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Padding = new Thickness(8),

                    Content = new TextBlock(screen)
                    {
                        Text = "Switch screen",
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black,
                        HorizontalAlignment = HorizontalAlignment.Left
                    }
                };
                switchScreenButton.Click += OnSwitchScreenButtonClick;
                stackPanel.Children.Add(switchScreenButton);

                var exitButton = new Button(screen)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Padding = new Thickness(8),

                    Content = new TextBlock(screen)
                    {
                        Text = "Exit",
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black,
                        HorizontalAlignment = HorizontalAlignment.Left
                    }
                };
                exitButton.Click += OnExitButtonClick;
                stackPanel.Children.Add(exitButton);

                var startWidthAnimation = new FloatLerpAnimation
                {
                    Action = (current) => { Width = current; },
                    To = Width,
                    Duration = TimeSpan.FromSeconds(0.3f),
                    Enabled = true
                };
                Animations.Add(startWidthAnimation);

                var startHeightAnimation = new FloatLerpAnimation
                {
                    Action = (current) => { Height = current; },
                    To = Height,
                    Duration = TimeSpan.FromSeconds(0.3f),
                    Enabled = true
                };
                Animations.Add(startHeightAnimation);

                overlayDialogDemoButton.Focus();
            }

            void OnSwitchScreenButtonClick(Control sender, ref RoutedEventContext context)
            {
                var overlay = new Overlay(Screen)
                {
                    Opacity = 0,
                    BackgroundColor = Color.Black
                };
                overlay.Show();

                var opacityAnimation = new FloatLerpAnimation
                {
                    Action = (current) => { overlay.Opacity = current; },
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.5d),
                    Enabled = true
                };
                opacityAnimation.Completed += (s, e) =>
                {
                    Screen.ShowScreen("MainMenuDemoScreen");
                };
                Animations.Add(opacityAnimation);
            }

            void OnExitButtonClick(Control sender, ref RoutedEventContext context)
            {
                var overlay = new Overlay(Screen)
                {
                    Opacity = 0,
                    BackgroundColor = Color.Black
                };
                overlay.Show();

                var opacityAnimation = new FloatLerpAnimation
                {
                    Action = (current) => { overlay.Opacity = current; },
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.5d),
                    Enabled = true
                };
                opacityAnimation.Completed += (s, e) => Screen.Game.Exit();
                Animations.Add(opacityAnimation);
            }
        }

        #endregion

        #region CubeWindow

        class CubeWindow : Window
        {
            FloatLerpAnimation rotateCubeAnimation;

            public CubeWindow(Screen screen)
                : base(screen)
            {
                var stackPanel = new StackPanel(screen)
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(8)
                };
                Content = stackPanel;

                var cubeControl = new CubeControl(screen)
                {
                    CubePrimitive = CreateCubePrimitive(),
                    ForegroundColor = Color.White,
                    Width = unit * 7,
                    Height = unit * 7,
                    CubeVisible = true
                };
                stackPanel.Children.Add(cubeControl);

                var closeButton = new Button(screen)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Padding = new Thickness(8),

                    Content = new TextBlock(screen)
                    {
                        Text = "Close",
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
                    }
                };
                closeButton.Click += (Control s, ref RoutedEventContext c) => Close();
                stackPanel.Children.Add(closeButton);

                rotateCubeAnimation = new FloatLerpAnimation
                {
                    Action = (current) =>
                    {
                        cubeControl.Orientation = Matrix.CreateFromYawPitchRoll(current, current, current);
                    },
                    To = MathHelper.TwoPi,
                    Duration = TimeSpan.FromSeconds(4),
                    Repeat = Repeat.Forever
                };
                cubeControl.MouseEnter += OnCubeControlMouseEnter;
                cubeControl.MouseLeave += OnCubeControlMouseLeave;
                Animations.Add(rotateCubeAnimation);
            }

            void OnCubeControlMouseEnter(Control sender, ref RoutedEventContext context)
            {
                var cubeControl = sender as CubeControl;
                cubeControl.Scale = 1.5f;

                rotateCubeAnimation.Enabled = true;
            }

            void OnCubeControlMouseLeave(Control sender, ref RoutedEventContext context)
            {
                var cubeControl = sender as CubeControl;
                cubeControl.Scale = 1;

                rotateCubeAnimation.Enabled = false;
            }

            /// <summary>
            /// 立方体の GeometricPrimitive を生成します。
            /// </summary>
            /// <returns>生成された立方体の GeometricPrimitive。</returns>
            GeometricPrimitive CreateCubePrimitive()
            {
                var cube = new Cube
                {
                    Size = 1,
                    BackwardColor = Color.Blue,
                    ForwardColor = Color.BlueViolet,
                    RightColor = Color.OrangeRed,
                    LeftColor = Color.Red,
                    UpColor = Color.Green,
                    DownColor = Color.GreenYellow
                };
                var source = new VertexSource<VertexPositionNormalColor, ushort>();
                cube.Make(source);
                return Graphics.GeometricPrimitive.Create(Screen.GraphicsDevice, source);
            }
        }

        #endregion

        #region FirstOverlayDialog

        class FirstOverlayDialog : OverlayDialogBase
        {
            public FirstOverlayDialog(Screen screen)
                : base(screen)
            {
                BackgroundColor = Color.Green;
                Opacity = 0.8f;

                var stackPanel = new StackPanel(screen)
                {
                    Margin = new Thickness(8),
                    Orientation = Orientation.Horizontal
                };
                Content = stackPanel;

                var stackedOverlayDialogDemoButton = new Button(screen)
                {
                    Padding = new Thickness(8),
                    HorizontalAlignment = HorizontalAlignment.Left,

                    Content = new TextBlock(screen)
                    {
                        Text = "Stacked overlay dialog demo",
                        TextHorizontalAlignment = HorizontalAlignment.Left,
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
                    }
                };
                stackPanel.Children.Add(stackedOverlayDialogDemoButton);
                stackedOverlayDialogDemoButton.Click += OnStackedOverlayDialogDemoButtonClick;
                stackedOverlayDialogDemoButton.Focus();

                var closeButton = new Button(screen)
                {
                    Padding = new Thickness(8),
                    HorizontalAlignment = HorizontalAlignment.Left,

                    Content = new TextBlock(screen)
                    {
                        Text = "Close",
                        TextHorizontalAlignment = HorizontalAlignment.Left,
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
                    }
                };
                stackPanel.Children.Add(closeButton);
                closeButton.Click += OnCloseButtonClick;
            }

            void OnCloseButtonClick(Control sender, ref RoutedEventContext context)
            {
                Close();
            }

            void OnStackedOverlayDialogDemoButtonClick(Control sender, ref RoutedEventContext context)
            {
                var dialog = new SecondOverlayDialog(Screen);
                dialog.Overlay.Opacity = 0.5f;
                dialog.Show();
            }
        }

        #endregion

        #region SecondOverlayDialog

        class SecondOverlayDialog : OverlayDialogBase
        {
            public SecondOverlayDialog(Screen screen)
                : base(screen)
            {
                BackgroundColor = Color.Brown;
                Padding = new Thickness(16);

                var closeButton = new Button(screen)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Padding = new Thickness(8),

                    Content = new TextBlock(screen)
                    {
                        Text = "Close",
                        TextHorizontalAlignment = HorizontalAlignment.Left,
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
                    }
                };
                Content = closeButton;
                closeButton.Click += (Control s, ref RoutedEventContext c) => Close();
                closeButton.Focus();
            }
        }

        #endregion

        #region ListBoxDemoWindow

        class ListBoxDemoWindow : Window
        {
            public ListBoxDemoWindow(Screen screen)
                : base(screen)
            {
                Width = 400;

                var stackPanel = new StackPanel(screen)
                {
                    Orientation = Orientation.Vertical,
                    Padding = new Thickness(16),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };
                Content = stackPanel;

                var title = new TextBlock(screen)
                {
                    Text = "ListBox demo",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    ForegroundColor = Color.White,
                    BackgroundColor = Color.Black,
                    ShadowOffset = new Vector2(2),
                    TextOutlineWidth = 1,
                    FontStretch = new Vector2(1.5f)
                };
                stackPanel.Children.Add(title);

                var selectedIndexLabel = new TextBlock(screen)
                {
                    Text = "SelectedIndex: -1",
                    Padding = new Thickness(8),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    ForegroundColor = Color.White,
                    BackgroundColor = Color.Black,
                    TextHorizontalAlignment = HorizontalAlignment.Left
                };
                stackPanel.Children.Add(selectedIndexLabel);

                var listBox = new ListBox(screen)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                };
                listBox.SelectionChanged += (s, e) =>
                {
                    selectedIndexLabel.Text = string.Format("SelectedIndex: {0}", listBox.SelectedIndex);
                };
                stackPanel.Children.Add(listBox);

                for (int i = 0; i < 5; i++)
                {
                    var listBoxItem = new ListBoxItem(screen)
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Padding = new Thickness(8),

                        Content = new TextBlock(screen)
                        {
                            Text = string.Format("Item {0:d2}", i),
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            ForegroundColor = Color.White,
                            BackgroundColor = Color.Black,
                            TextHorizontalAlignment = HorizontalAlignment.Left
                        }
                    };
                    listBox.Items.Add(listBoxItem);
                }

                var switchSelectionModeButton = new Button(screen)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Padding = new Thickness(8),

                    Content = new TextBlock(screen)
                    {
                        Text = "Enable the multiple selection mode",
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
                    }
                };
                switchSelectionModeButton.Click += (Control s, ref RoutedEventContext c) =>
                {
                    var textBlock = switchSelectionModeButton.Content as TextBlock;
                    if (listBox.SelectionMode == ListBoxSelectionMode.Single)
                    {
                        listBox.SelectionMode = ListBoxSelectionMode.Multiple;
                        textBlock.Text = "Disable the multiple selection mode";
                    }
                    else
                    {
                        listBox.SelectionMode = ListBoxSelectionMode.Single;
                        textBlock.Text = "Enable the multiple selection mode";
                    }
                };
                stackPanel.Children.Add(switchSelectionModeButton);

                var closeButton = new Button(screen)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Padding = new Thickness(8),

                    Content = new TextBlock(screen)
                    {
                        Text = "Close",
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
                    }
                };
                closeButton.Click += (Control s, ref RoutedEventContext c) => Close();
                stackPanel.Children.Add(closeButton);

                closeButton.Focus();
            }
        }

        #endregion

        const int unit = 32;

        DefaultSpriteSheetSource spriteSheetSource;

        SelectableLookAndFeelSource selectableLookAndFeelSource = new SelectableLookAndFeelSource();

        public WindowDemoScreen(Game game)
            : base(game)
        {
            Content.RootDirectory = "Content";
        }

        internal void SwitchLookAndFeelSource()
        {
            if (selectableLookAndFeelSource.SelectedIndex == 0)
            {
                selectableLookAndFeelSource.SelectedIndex = 1;
            }
            else
            {
                selectableLookAndFeelSource.SelectedIndex = 0;
            }
        }

        protected override void LoadContent()
        {
            // 重いロードのテスト用にスリープさせてます。
            System.Threading.Thread.Sleep(2000);

            InitializeSpriteSheetSource();
            InitializeLookAndFeelSource();
            InitializeControls();

            base.LoadContent();
        }

        void InitializeSpriteSheetSource()
        {
            var windowTemplate = new WindowSpriteSheetTemplate(16, 16);
            var windowShadowConverter = new DecoloringTexture2DConverter(new Color(0, 0, 0, 0.5f));
            var windowTexture = Content.Load<Texture2D>("UI/Sprite/Window");
            var windowShadowTexture = windowShadowConverter.Convert(windowTexture);

            spriteSheetSource = new DefaultSpriteSheetSource();
            spriteSheetSource.SpriteSheetMap["Window"] = new SpriteSheet(windowTemplate, windowTexture);
            spriteSheetSource.SpriteSheetMap["WindowShadow"] = new SpriteSheet(windowTemplate, windowShadowTexture);
        }

        void InitializeLookAndFeelSource()
        {
            selectableLookAndFeelSource.Items.Add(CreateDefaultLookAndFeelSource());
            selectableLookAndFeelSource.Items.Add(DebugLooAndFeelUtil.CreateLookAndFeelSource(Game));
            selectableLookAndFeelSource.SelectedIndex = 0;

            LookAndFeelSource = selectableLookAndFeelSource;
        }

        ILookAndFeelSource CreateDefaultLookAndFeelSource()
        {
            var source = new DefaultLookAndFeelSource();

            source.LookAndFeelMap[typeof(Desktop)] = new DesktopLookAndFeel();
            source.LookAndFeelMap[typeof(Window)] = new SpriteSheetWindowLookAndFeel
            {
                WindowSpriteSheet = spriteSheetSource.GetSpriteSheet("Window"),
                WindowShadowSpriteSheet = spriteSheetSource.GetSpriteSheet("WindowShadow")
            };
            source.LookAndFeelMap[typeof(TextBlock)] = new TextBlockLookAndFeel();
            source.LookAndFeelMap[typeof(Overlay)] = new OverlayLookAndFeel();
            source.LookAndFeelMap[typeof(Button)] = new ButtonLookAndFeel
            {
                FocusColor = Color.Navy * 0.5f,
                MouseOverColor = Color.DarkGreen * 0.5f
            };
            source.LookAndFeelMap[typeof(ListBoxItem)] = new ListBoxItemLookAndFeel
            {
                FocusColor = Color.Navy * 0.5f,
                MouseOverColor = Color.DarkGreen * 0.5f,
                SelectionColor = Color.Orange * 0.5f
            };

            return source;
        }

        void InitializeControls()
        {

            var firstWindow = new FirstWindow(this);
            firstWindow.Show();

            var secondWindow = new SecondWindow(this);
            secondWindow.Show();

            var thirdWindow = new ThirdWindow(this);
            thirdWindow.Show();

            var startEffectOverlay = new Overlay(this)
            {
                Opacity = 1
            };
            startEffectOverlay.Show();

            var startEffectOverlay_opacityAnimation = new FloatLerpAnimation
            {
                Action = (current) => { startEffectOverlay.Opacity = current; },
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5d),
                Enabled = true
            };
            startEffectOverlay_opacityAnimation.Completed += (s, e) =>
            {
                startEffectOverlay.Close();
                thirdWindow.Activate();
            };
            startEffectOverlay.Animations.Add(startEffectOverlay_opacityAnimation);
        }
    }
}
