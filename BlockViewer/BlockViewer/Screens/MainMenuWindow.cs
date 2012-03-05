#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;
using Willcraftia.Xna.Blocks.BlockViewer.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class MainMenuWindow : Window
    {
        #region Controls

        TabControl tab;

        const int mainMenuIndex = 0;
        
        const int modeMenuIndex = 1;

        const int lodMenuIndex = 2;

        #region Main Menu

        Button loadButton;

        Button modeButton;

        Button lodButton;

        Button changeLookAndFeelButton;

        #endregion

        #region Mode Menu

        Button cameraModeButton;

        Button light0ModeButton;
        
        Button light1ModeButton;
        
        Button light2ModeButton;

        #endregion

        #region LoD Menu

        Button showLodButton;

        Button hideLodButton;

        #endregion

        OpenStorageDialog openStorageDialog;

        LightWindow lightWindow;

        LodWindow lodWindow;

        #endregion

        #region Animations

        FloatLerpAnimation openAnimation;

        FloatLerpAnimation closeAnimation;

        #endregion

        MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        public MainMenuWindow(Screen screen)
            : base(screen)
        {
            // 開く際に openAnimation で Width を設定するので 0 で初期化します。
            Width = 0;
            Height = 480;
            Padding = new Thickness(16);

            var stackPanel = new StackPanel(screen)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Orientation = Orientation.Vertical
            };
            Content = stackPanel;

            var title = new TextBlock(screen)
            {
                Text = Strings.MainMenuTitle,
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

            tab = new TabControl(screen)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                SelectedIndex = mainMenuIndex
            };
            stackPanel.Children.Add(tab);

            var mainMenuPanel = CreateMainMenuPanel();
            tab.Items.Add(mainMenuPanel);

            var modeMenuPanel = CreateModeMenuPanel();
            tab.Items.Add(modeMenuPanel);

            var lodMenuPanel = CreateLodMenuPanel();
            tab.Items.Add(lodMenuPanel);

            openAnimation = new FloatLerpAnimation
            {
                Action = (current) => { Width = current; },
                From = 0,
                To = 240,
                Duration = TimeSpan.FromSeconds(0.1f)
            };
            Animations.Add(openAnimation);

            // 閉じる場合には closeAnimation を実行し、その完了で完全に閉じます。
            closeAnimation = new FloatLerpAnimation
            {
                Action = (current) => { Width = current; },
                From = 240,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.1f)
            };
            closeAnimation.Completed += (s, e) =>
            {
                base.Close();
                // Screen は最前面の Window をアクティブにするので、
                // 強制的に Desktop をアクティブにします。
                //Screen.Desktop.Activate();
            };
            Animations.Add(closeAnimation);

            // デフォルト フォーカス。
            loadButton.Focus();
        }

        public override void Show()
        {
            openAnimation.Enabled = true;

            base.Show();
        }

        public override void Close()
        {
            // Close 処理はまだ呼び出さずに closeAnimation を実行します。
            closeAnimation.Enabled = true;
        }

        protected override void OnKeyDown(ref RoutedEventContext context)
        {
            base.OnKeyDown(ref context);

            if (Screen.KeyboardDevice.IsKeyPressed(Keys.Escape))
            {
                Close();

                context.Handled = true;
            }
        }

        void OnLoadButtonClick(Control sender, ref RoutedEventContext context)
        {
            if (openStorageDialog == null)
            {
                openStorageDialog = new OpenStorageDialog(Screen)
                {
                    DataContext = ViewModel.OpenStorageViewModel
                };
                openStorageDialog.Closed += new EventHandler(OnOpenStorageDialogClosed);
            }
            openStorageDialog.Show();
        }

        void OnOpenStorageDialogClosed(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ViewModel.OpenStorageViewModel.SelectedFileName)) return;

            Close();

            // TODO: 少しでも負荷がかかると短い Animation が効果を表すことなく完了してしまう。
            ViewModel.LoadBlockMeshFromStorage();
        }

        void OnModeButtonClick(Control sender, ref RoutedEventContext context)
        {
            tab.SelectedIndex = modeMenuIndex;

            switch (ViewModel.BlockMeshViewModel.Mode)
            {
                case Mode.Camera:
                    cameraModeButton.Focus();
                    break;
                case Mode.DirectionalLight0:
                    light0ModeButton.Focus();
                    break;
                case Mode.DirectionalLight1:
                    light1ModeButton.Focus();
                    break;
                case Mode.DirectionalLight2:
                    light2ModeButton.Focus();
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        void OnLodButtonClick(Control sender, ref RoutedEventContext context)
        {
            tab.SelectedIndex = lodMenuIndex;

            if (lodWindow != null && lodWindow.Visible)
            {
                showLodButton.Focus();
            }
            else
            {
                hideLodButton.Focus();
            }
        }

        void OnExitButtonClick(Control sender, ref RoutedEventContext context)
        {
            var overlay = new FadeOverlay(Screen);
            overlay.OpacityAnimation.To = 1;
            overlay.OpacityAnimation.Duration = TimeSpan.FromSeconds(1);
            overlay.OpacityAnimation.Completed += (s, e) => Screen.ShowScreen(ScreenNames.Start);
            overlay.Show();
        }

        void OnCloseButtonClick(Control sender, ref RoutedEventContext context)
        {
            Close();
        }

        Control CreateMainMenuPanel()
        {
            var stackPanel = new StackPanel(Screen)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Orientation = Orientation.Vertical
            };

            loadButton = CreateMenuButton(Strings.LoadButton);
            loadButton.Click += new RoutedEventHandler(OnLoadButtonClick);
            stackPanel.Children.Add(loadButton);

            modeButton = CreateMenuButton(Strings.ModeButton);
            modeButton.Click += new RoutedEventHandler(OnModeButtonClick);
            stackPanel.Children.Add(modeButton);

            lodButton = CreateMenuButton(Strings.LodButton);
            lodButton.Click += new RoutedEventHandler(OnLodButtonClick);
            stackPanel.Children.Add(lodButton);

            changeLookAndFeelButton = CreateMenuButton("Look & Feel [Debug]");
            changeLookAndFeelButton.Click += new RoutedEventHandler(OnChangeLookAndFeelButtonClick);
            stackPanel.Children.Add(changeLookAndFeelButton);

            var exitButton = CreateMenuButton(Strings.ExitButton);
            exitButton.Click += new RoutedEventHandler(OnExitButtonClick);
            stackPanel.Children.Add(exitButton);

            var closeButton = CreateMenuButton(Strings.CloseButton);
            closeButton.Click += new RoutedEventHandler(OnCloseButtonClick);
            stackPanel.Children.Add(closeButton);

            return stackPanel;
        }

        Control CreateModeMenuPanel()
        {
            var stackPanel = new StackPanel(Screen)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Orientation = Orientation.Vertical
            };

            var subTitle = new TextBlock(Screen)
            {
                Text = Strings.ModeMenuTitle,
                Padding = new Thickness(4),
                ForegroundColor = Color.LightGreen,
                BackgroundColor = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextHorizontalAlignment = HorizontalAlignment.Left,
                ShadowOffset = new Vector2(2)
            };
            stackPanel.Children.Add(subTitle);

            cameraModeButton = CreateMenuButton(Strings.CameraModeButton);
            cameraModeButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                ChangeMode(Mode.Camera);
            };
            stackPanel.Children.Add(cameraModeButton);

            light0ModeButton = CreateMenuButton(Strings.Light0ModeButton);
            light0ModeButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                ChangeMode(Mode.DirectionalLight0);
            };
            stackPanel.Children.Add(light0ModeButton);

            light1ModeButton = CreateMenuButton(Strings.Light1ModeButton);
            light1ModeButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                ChangeMode(Mode.DirectionalLight1);
            };
            stackPanel.Children.Add(light1ModeButton);

            light2ModeButton = CreateMenuButton(Strings.Light2ModeButton);
            light2ModeButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                ChangeMode(Mode.DirectionalLight2);
            };
            stackPanel.Children.Add(light2ModeButton);

            var backButton = CreateMenuButton(Strings.BackButton);
            backButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                tab.SelectedIndex = mainMenuIndex;
                modeButton.Focus();
            };
            stackPanel.Children.Add(backButton);

            return stackPanel;
        }

        Control CreateLodMenuPanel()
        {
            var stackPanel = new StackPanel(Screen)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Orientation = Orientation.Vertical
            };

            var subTitle = new TextBlock(Screen)
            {
                Text = Strings.LodMenuTitle,
                Padding = new Thickness(4),
                ForegroundColor = Color.LightGreen,
                BackgroundColor = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextHorizontalAlignment = HorizontalAlignment.Left,
                ShadowOffset = new Vector2(2)
            };
            stackPanel.Children.Add(subTitle);

            showLodButton = CreateMenuButton(Strings.ShowButton);
            showLodButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                ChangeLodWindowVisibility(true);
            };
            stackPanel.Children.Add(showLodButton);

            hideLodButton = CreateMenuButton(Strings.HideButton);
            hideLodButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                ChangeLodWindowVisibility(false);
            };
            stackPanel.Children.Add(hideLodButton);

            var backButton = CreateMenuButton(Strings.BackButton);
            backButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                tab.SelectedIndex = mainMenuIndex;
                lodButton.Focus();
            };
            stackPanel.Children.Add(backButton);

            return stackPanel;
        }

        Button CreateMenuButton(String text)
        {
            float buttonHeight = 32;

            var button = new Button(Screen)
            {
                Height = buttonHeight,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Padding = new Thickness(4),

                Content = new TextBlock(Screen)
                {
                    Text = text,
                    ForegroundColor = Color.White,
                    BackgroundColor = Color.Black,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    TextHorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(16, 0, 0, 0),
                    ShadowOffset = new Vector2(2)
                }
            };

            ControlUtil.SetDefaultBehavior(button);

            return button;
        }

        void ChangeMode(Mode mode)
        {
            ViewModel.BlockMeshViewModel.Mode = mode;

            if (Mode.Camera == mode)
            {
                if (lightWindow != null && lightWindow.Visible) lightWindow.Close();
            }
            else
            {
                if (lightWindow == null)
                {
                    lightWindow = new LightWindow(Screen)
                    {
                        DataContext = ViewModel.BlockMeshViewModel
                    };
                }
                lightWindow.Show();
            }

            tab.SelectedIndex = mainMenuIndex;
            modeButton.Focus();
            Close();
        }

        void ChangeLodWindowVisibility(bool show)
        {
            if (show)
            {
                if (lodWindow == null)
                {
                    lodWindow = new LodWindow(Screen)
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Bottom
                    };
                }
                lodWindow.Show();
            }
            else
            {
                if (lodWindow != null && lodWindow.Visible) lodWindow.Close();
            }
            tab.SelectedIndex = mainMenuIndex;
            lodButton.Focus();
            Close();
        }

        void OnChangeLookAndFeelButtonClick(Control sender, ref RoutedEventContext context)
        {
            var startScreen = Screen as MainScreen;
            if (startScreen.SelectedLookAndFeelSourceIndex == MainScreen.DefaultLookAndFeelIndex)
            {
                startScreen.SelectedLookAndFeelSourceIndex = MainScreen.DebugLookAndFeelIndex;
                (changeLookAndFeelButton.Content as TextBlock).Text = "Look & Feel [Default]";
            }
            else
            {
                startScreen.SelectedLookAndFeelSourceIndex = MainScreen.DefaultLookAndFeelIndex;
                (changeLookAndFeelButton.Content as TextBlock).Text = "Look & Feel [Debug]";
            }
        }
    }
}
