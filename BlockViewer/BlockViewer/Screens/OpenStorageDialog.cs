#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
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

        Button[] fileNameButtons;

        MessageBox messageBox;

        string targetFileName;

        public OpenStorageDialog(Screen screen, OpenStorageViewModel viewModel)
            : base(screen)
        {
            DataContext = viewModel;

            Padding = new Thickness(16);

            var basePanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical
            };
            Content = basePanel;

            var pageButtonsPanel = new StackPanel(screen)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            basePanel.Children.Add(pageButtonsPanel);

            var previousePageButton = new Button(screen)
            {
                Focusable = false,
                Width = BlockViewerGame.SpriteSize,
                HorizontalAlignment = HorizontalAlignment.Left,
                Content = new Image(screen)
                {
                    Texture = screen.Content.Load<Texture2D>("UI/ArrowLeft")
                }
            };
            pageButtonsPanel.Children.Add(previousePageButton);

            var nextPageButton = new Button(screen)
            {
                Focusable = false,
                Width = BlockViewerGame.SpriteSize,
                HorizontalAlignment = HorizontalAlignment.Right,
                Content = new Image(screen)
                {
                    Texture = screen.Content.Load<Texture2D>("UI/ArrowRight")
                }
            };
            pageButtonsPanel.Children.Add(nextPageButton);

            var fileNameListPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical
            };
            basePanel.Children.Add(fileNameListPanel);

            fileNameButtons = new Button[listSize];
            for (int i = 0; i < listSize; i++)
            {
                fileNameButtons[i] = new Button(screen)
                {
                    ForegroundColor = Color.White,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Width = BlockViewerGame.SpriteSize * 8,
                    Height = BlockViewerGame.SpriteSize,
                    Content = new TextBlock(screen)
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        TextHorizontalAlignment = HorizontalAlignment.Left
                    }
                };
                fileNameButtons[i].Click += new RoutedEventHandler(OnFileNameButtonClick);
                fileNameListPanel.Children.Add(fileNameButtons[i]);
            }
        }

        void OnFileNameButtonClick(Control sender, ref RoutedEventContext context)
        {
            if (messageBox == null)
            {
                messageBox = new MessageBox(Screen, MessageBoxButton.OKCancel, MessageBoxResult.Cancel);
                messageBox.Padding = new Thickness(16);
                messageBox.TextBlock.Text = "Are you sure you want to load this file?";
                messageBox.Closed += new EventHandler(OnMessageBoxClosed);
            }

            (DataContext as OpenStorageViewModel).SelectedFileName = null;
            targetFileName = ((sender as Button).Content as TextBlock).Text;
            messageBox.Show();

            context.Handled = true;
        }

        void OnMessageBoxClosed(object sender, EventArgs e)
        {
            if (messageBox.Result == MessageBoxResult.OK)
            {
                (DataContext as OpenStorageViewModel).SelectedFileName = targetFileName;
                Close();
            }
            else
            {
                targetFileName = null;
            }
        }

        public override void Show()
        {
            fileNames = (DataContext as OpenStorageViewModel).GetFileNames();
            SetFiles(0);

            base.Show();
        }

        protected override void OnKeyDown(ref RoutedEventContext context)
        {
            if (Screen.KeyboardDevice.IsKeyPressed(Keys.Escape))
            {
                Hide();
                context.Handled = true;
            }

            base.OnKeyDown(ref context);
        }

        void SetFiles(int pageIndex)
        {
            bool focused = false;
            for (int i = 0; i < listSize; i++)
            {
                var fileBlock = fileNameButtons[i];

                int fileNameIndex = pageIndex * listSize + i;
                if (fileNameIndex < fileNames.Length)
                {
                    (fileBlock.Content as TextBlock).Text = fileNames[fileNameIndex];
                    fileBlock.Focusable = true;
                    if (!focused)
                    {
                        fileBlock.Focus();
                        focused = true;
                    }
                }
                else
                {
                    (fileBlock.Content as TextBlock).Text = string.Empty;
                    fileBlock.Focusable = false;
                }
            }
        }

        void OnCancelButtonClick(Control sender, ref RoutedEventContext context)
        {
            Hide();
            context.Handled = true;
        }
    }
}
