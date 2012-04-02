#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;
using Willcraftia.Xna.Blocks.BlockViewer.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class LoadBlockMeshDialog : OverlayDialogBase
    {
        #region FileButton

        class FileButton : Button
        {
            TextBlock fileNameTextBlock;

            BlockMeshView blockMeshView;

            public FileButton(Screen screen)
                : base(screen)
            {
                Padding = new Thickness(8);
                HorizontalAlignment = HorizontalAlignment.Stretch;

                var stackPanel = new StackPanel(screen)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                Content = stackPanel;

                blockMeshView = new BlockMeshView(screen)
                {
                    Width = 64,
                    Height = 64
                };
                stackPanel.Children.Add(blockMeshView);

                fileNameTextBlock = new TextBlock(screen)
                {
                    ForegroundColor = Color.White,
                    BackgroundColor = Color.Black,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    TextHorizontalAlignment = HorizontalAlignment.Left,
                    ShadowOffset = new Vector2(2)
                };
                stackPanel.Children.Add(fileNameTextBlock);
            }

            public override void Update(GameTime gameTime)
            {
                var viewerViewModel = DataContext as ViewerViewModel;
                var meshName = viewerViewModel.MeshName;

                fileNameTextBlock.Text = meshName;

                Enabled = (meshName != null);

                base.Update(gameTime);
            }
        }

        #endregion

        TextBlock pageTextBlock;

        FileButton[] fileButtons = new FileButton[4];

        Button cancelButton;

        FloatLerpAnimation openAnimation;

        FloatLerpAnimation closeAnimation;

        ConfirmationDialog confirmationDialog;

        LoadBlockMeshViewModel ViewModel
        {
            get { return DataContext as LoadBlockMeshViewModel; }
        }

        public LoadBlockMeshDialog(Screen screen)
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

            var pageButtonPanel = new StackPanel(screen)
            {
                HorizontalAlignment = HorizontalAlignment.Right
            };
            stackPanel.Children.Add(pageButtonPanel);

            var backPageButton = new Button(screen)
            {
                Focusable = false,
                Width = BlockViewerGame.SpriteSize,
                HorizontalAlignment = HorizontalAlignment.Left,
                Content = new Image(screen)
                {
                    Texture = screen.Content.Load<Texture2D>("UI/ArrowLeft")
                }
            };
            backPageButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                ViewModel.BackPage();
            };
            pageButtonPanel.Children.Add(backPageButton);

            pageTextBlock = new TextBlock(screen)
            {
                Width = BlockViewerGame.SpriteSize * 2,
                ForegroundColor = Color.White,
                BackgroundColor = Color.Black,
                ShadowOffset = new Vector2(2)
            };
            pageButtonPanel.Children.Add(pageTextBlock);

            var forwardPageButton = new Button(screen)
            {
                Focusable = false,
                Width = BlockViewerGame.SpriteSize,
                HorizontalAlignment = HorizontalAlignment.Right,
                Content = new Image(screen)
                {
                    Texture = screen.Content.Load<Texture2D>("UI/ArrowRight")
                }
            };
            forwardPageButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                ViewModel.ForwardPage();
            };
            pageButtonPanel.Children.Add(forwardPageButton);

            var fileListPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            stackPanel.Children.Add(fileListPanel);

            for (int i = 0; i < fileButtons.Length; i++)
            {
                fileButtons[i] = new FileButton(screen)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                fileButtons[i].Click += OnFileButtonClick;
                fileButtons[i].KeyDown += OnFileNameButtonKeyDown;
                fileListPanel.Children.Add(fileButtons[i]);
            }

            var separator = ControlUtil.CreateDefaultSeparator(screen);
            stackPanel.Children.Add(separator);

            cancelButton = ControlUtil.CreateDefaultDialogButton(screen, Strings.CancelButton);
            cancelButton.Click += (Control s, ref RoutedEventContext c) => Close();
            stackPanel.Children.Add(cancelButton);

            const float windowWidth = 480;

            openAnimation = new FloatLerpAnimation
            {
                Action = (current) => { Width = current; },
                From = 0,
                To = windowWidth,
                Duration = TimeSpan.FromSeconds(0.1f)
            };
            Animations.Add(openAnimation);

            // 閉じる場合には closeAnimation を実行し、その完了で完全に閉じます。
            closeAnimation = new FloatLerpAnimation
            {
                Action = (current) => { Width = current; },
                From = windowWidth,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.1f)
            };
            closeAnimation.Completed += (s, e) => base.Close();
            Animations.Add(closeAnimation);
        }

        public override void Show()
        {
            ViewModel.Initialize(fileButtons.Length);
            
            for (int i = 0; i < fileButtons.Length; i++)
                fileButtons[i].DataContext = ViewModel.GetViewerViewModel(i);

            // 常に Cancel ボタンにフォーカスを設定します。
            cancelButton.Focus();

            openAnimation.Enabled = true;

            base.Show();
        }

        public override void Close()
        {
            ViewModel.UnloadMeshes();

            // Close 処理はまだ呼び出さずに closeAnimation を実行します。
            closeAnimation.Enabled = true;
        }

        public override void Update(GameTime gameTime)
        {
            //
            // MEMO
            //
            // ViewModel とのイベント駆動による通信を行わず、
            // Update(GameTime) からの状態のポーリングを基本パターンとします。
            //

            for (int i = 0; i < fileButtons.Length; i++)
            {
                var fileButton = fileButtons[i];

                // フォーカスが設定されていたボタンがファイルなしになった場合、
                // フォーカスを先頭のボタンに設定することを試みます。
                if (!fileButton.Enabled && fileButton.Focused) fileButtons[0].Focus();
            }

            var currentPageNo = ViewModel.CurrentPageIndex + 1;
            pageTextBlock.Text = currentPageNo + "/" + ViewModel.PageCount;

            base.Update(gameTime);
        }

        protected override void OnKeyDown(ref RoutedEventContext context)
        {
            if (Screen.KeyboardDevice.IsKeyPressed(Keys.Escape))
            {
                Close();
                context.Handled = true;
            }

            base.OnKeyDown(ref context);
        }

        void OnFileNameButtonKeyDown(Control sender, ref RoutedEventContext context)
        {
            if (Screen.KeyboardDevice.IsKeyPressed(Keys.Left))
            {
                var sound = Screen.GetSound(SoundKey.FocusNavigation);
                if (sound != null)
                {
                    if (sound.State != SoundState.Stopped) sound.Stop();
                    sound.Play();
                }

                ViewModel.BackPage();
                context.Handled = true;
            }
            else if (Screen.KeyboardDevice.IsKeyPressed(Keys.Right))
            {
                var sound = Screen.GetSound(SoundKey.FocusNavigation);
                if (sound != null)
                {
                    if (sound.State != SoundState.Stopped) sound.Stop();
                    sound.Play();
                }

                ViewModel.ForwardPage();
                context.Handled = true;
            }
        }

        void OnFileButtonClick(Control sender, ref RoutedEventContext context)
        {
            if (confirmationDialog == null)
            {
                confirmationDialog = new ConfirmationDialog(Screen)
                {
                    Message = new TextBlock(Screen)
                    {
                        Text = Strings.OpenFileConfirmation,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black,
                        ShadowOffset = new Vector2(2)
                    }
                };
                confirmationDialog.Closed += OnOpenFileConfirmationDialogClosed;
            }

            // 仮選択。
            ViewModel.SelectedFileName = (sender.DataContext as ViewerViewModel).MeshName;

            confirmationDialog.Show();

            context.Handled = true;
        }

        void OnOpenFileConfirmationDialogClosed(object sender, EventArgs e)
        {
            if (confirmationDialog.Result == MessageBoxResult.OK)
            {
                Close();
            }
            else
            {
                // 仮選択をキャンセル。
                ViewModel.SelectedFileName = null;
            }
        }
    }
}
