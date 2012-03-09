#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Lafs;
using Willcraftia.Xna.Framework.UI.Lafs.Debug;
using Willcraftia.Xna.Framework.UI.Demo.Lafs;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo.Screens
{
    public sealed class MainMenuDemoScreen : Screen
    {
        #region MenuWindow

        class MenuWindow : Window
        {
            public MenuWindow(Screen screen)
                : base(screen)
            {
                BackgroundColor = Color.Blue;

                var stackPanel = new StackPanel(screen)
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(16)
                };
                Content = stackPanel;

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
                    (screen as MainMenuDemoScreen).SwitchLookAndFeelSource();
                };
                stackPanel.Children.Add(changingLookAndFeelDemoButton);

                var switchScreenButton = new Button(screen)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Padding = new Thickness(8),

                    Content = new TextBlock(screen)
                    {
                        Text = "SWITCH SCREEN",
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
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
                        Text = "EXIT",
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
                    }
                };
                exitButton.Click += OnExitButtonClick;
                stackPanel.Children.Add(exitButton);

                switchScreenButton.Focus();
            }

            void OnSwitchScreenButtonClick(Control sender, ref RoutedEventContext context)
            {
                var overlay = new Overlay(Screen);
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
                    var uiService = Screen.Game.Services.GetRequiredService<IUIService>();
                    uiService.Show("WindowDemoScreen");
                };
                Animations.Add(opacityAnimation);
            }

            void OnExitButtonClick(Control sender, ref RoutedEventContext context)
            {
                var overlay = new Overlay(Screen);
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

        const int unit = 32;

        DefaultSpriteSheetSource spriteSheetSource;

        SelectableLookAndFeelSource selectableLookAndFeelSource = new SelectableLookAndFeelSource();

        public MainMenuDemoScreen(Game game)
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
            source.LookAndFeelMap[typeof(Button)] = new MyButtonLookAndFeel();

            return source;
        }

        void InitializeControls()
        {
            var menuWindow = new MenuWindow(this);
            menuWindow.Show();

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
                menuWindow.Activate();
            };
            startEffectOverlay.Animations.Add(startEffectOverlay_opacityAnimation);
        }
    }
}
