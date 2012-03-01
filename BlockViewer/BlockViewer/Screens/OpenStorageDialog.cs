#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;
using Willcraftia.Xna.Blocks.BlockViewer.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class OpenStorageDialog : OverlayDialogBase
    {
        //
        // MEMO
        //
        // XNA の StorageDevice を用いたファイル オープンを想定。
        // セーブ ディレクトリ固定で、その中のセーブ データ一覧を表示し、
        // 選択することを想定。
        //

        const int listSize = 5;

        int currentPageIndex;

        // Show() 呼び出しのタイミングでファイルをキャッシュする前提。
        string[] fileNames;

        TextBlock pageTextBlock;

        TextButton[] fileNameButtons;

        Button cancelButton;

        FloatLerpAnimation openAnimation;

        FloatLerpAnimation closeAnimation;

        ConfirmationDialog confirmationDialog;

        ErrorDialog noFileErrorDialog;

        string targetFileName;

        public OpenStorageDialog(Screen screen, OpenStorageViewModel viewModel)
            : base(screen)
        {
            DataContext = viewModel;

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
            backPageButton.Click += (Control s, ref RoutedEventContext c) => BackPage();
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
            forwardPageButton.Click += (Control s, ref RoutedEventContext c) => ForwardPage();
            pageButtonPanel.Children.Add(forwardPageButton);

            var fileNameListPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            stackPanel.Children.Add(fileNameListPanel);

            fileNameButtons = new TextButton[listSize];
            for (int i = 0; i < listSize; i++)
            {
                fileNameButtons[i] = new TextButton(screen)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Height = BlockViewerGame.SpriteSize,
                    Padding = new Thickness(4)
                };
                fileNameButtons[i].TextBlock.ForegroundColor = Color.White;
                fileNameButtons[i].TextBlock.BackgroundColor = Color.Black;
                fileNameButtons[i].TextBlock.FontStretch = new Vector2(0.8f);
                fileNameButtons[i].TextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                fileNameButtons[i].TextBlock.TextHorizontalAlignment = HorizontalAlignment.Left;
                fileNameButtons[i].TextBlock.ShadowOffset = new Vector2(2);
                fileNameButtons[i].Click += new RoutedEventHandler(OnFileNameButtonClick);
                fileNameButtons[i].KeyDown += new RoutedEventHandler(OnFileNameButtonKeyDown);
                fileNameListPanel.Children.Add(fileNameButtons[i]);
            }

            var separator = ControlUtil.CreateDefaultSeparator(screen);
            stackPanel.Children.Add(separator);

            cancelButton = ControlUtil.CreateDefaultDialogButton(screen, Strings.CancelButton);
            cancelButton.Click += (Control s, ref RoutedEventContext c) => Close();
            stackPanel.Children.Add(cancelButton);

            const float windowWidth = 320;

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
            (DataContext as OpenStorageViewModel).SelectedFileName = null;
            fileNames = (DataContext as OpenStorageViewModel).GetFileNames();

            // ファイルがない場合は、その旨を MessageBox で表示して終えます。
            if (fileNames.Length == 0)
            {
                ShowNoFileErrorMessageBox();
                return;
            }

            ReloadPage();

            // 常に Cancel ボタンにフォーカスを設定します。
            cancelButton.Focus();

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
                BackPage();
                context.Handled = true;
            }
            else if (Screen.KeyboardDevice.IsKeyPressed(Keys.Right))
            {
                ForwardPage();
                context.Handled = true;
            }
        }

        void BackPage()
        {
            currentPageIndex--;
            // 先頭を越えるならば末尾のページを設定します。
            if (currentPageIndex < 0) currentPageIndex = fileNames.Length / listSize;

            ReloadPage();
        }

        void ForwardPage()
        {
            currentPageIndex++;
            // 末尾を越えるならば先頭のページを設定します。
            if (fileNames.Length / listSize < currentPageIndex) currentPageIndex = 0;

            ReloadPage();
        }

        void OnFileNameButtonClick(Control sender, ref RoutedEventContext context)
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
                confirmationDialog.Closed += new EventHandler(OnOpenFileConfirmationDialogClosed);
            }

            targetFileName = ((sender as Button).Content as TextBlock).Text;

            confirmationDialog.Show();

            context.Handled = true;
        }

        void OnOpenFileConfirmationDialogClosed(object sender, EventArgs e)
        {
            if (confirmationDialog.Result == MessageBoxResult.OK)
            {
                (DataContext as OpenStorageViewModel).SelectedFileName = targetFileName;
                Close();
            }
            else
            {
                targetFileName = null;
            }
        }

        void ShowNoFileErrorMessageBox()
        {
            if (noFileErrorDialog == null)
            {
                noFileErrorDialog = new ErrorDialog(Screen)
                {
                    Message = new TextBlock(Screen)
                    {
                        Text = Strings.NoFileError,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black,
                        ShadowOffset = new Vector2(2)
                    }
                };
            }
            noFileErrorDialog.Show();
        }

        void ReloadPage()
        {
            // 状態を初期化します。
            for (int i = 0; i < listSize; i++)
            {
                var fileNameButton = fileNameButtons[i];
                fileNameButton.TextBlock.Text = string.Empty;
                fileNameButton.Focusable = false;
            }

            // ファイル名を設定します。
            for (int i = 0; i < listSize; i++)
            {
                var fileNameButton = fileNameButtons[i];

                int fileNameIndex = currentPageIndex * listSize + i;
                if (fileNameIndex < fileNames.Length)
                {
                    fileNameButton.TextBlock.Text = fileNames[fileNameIndex];
                    fileNameButton.Focusable = true;
                }
                else
                {
                    // フォーカスが設定されていたボタンがファイルなしになった場合、
                    // フォーカスを先頭のボタンに設定することを試みます。
                    if (fileNameButton.Focused)
                    {
                        if (!fileNameButtons[0].Focus())
                        {
                            // 先頭のボタンに設定できない場合は Cancel ボタンに設定します。
                            cancelButton.Focus();
                        }
                    }
                }
            }

            var currentPageNo = currentPageIndex + 1;
            var lastPageNo = fileNames.Length / listSize + 1;

            pageTextBlock.Text = currentPageNo + "/" + lastPageNo;
        }
    }
}
