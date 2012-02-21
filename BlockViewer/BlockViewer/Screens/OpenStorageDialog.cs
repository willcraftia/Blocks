#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class OpenStorageDialog : Window
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

        TextBlock[] fileNameBlocks;

        public StorageContainer StorageContainer { get; private set; }

        public OpenStorageDialog(Screen screen, StorageContainer storageContainer)
            : base(screen)
        {
            if (storageContainer == null) throw new ArgumentNullException("storageContainer");
            StorageContainer = storageContainer;

            Padding = new Thickness(16);

            var basePanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical
            };
            Content = basePanel;

            var pageButtonsPanel = new StackPanel(screen);
            basePanel.Children.Add(pageButtonsPanel);

            var previousePageButton = new Button(screen)
            {
                Width = BlockViewerGame.SpriteSize,
                Content = new Image(screen)
                {
                    Texture = screen.Content.Load<Texture2D>("UI/ArrowLeft")
                }
            };
            pageButtonsPanel.Children.Add(previousePageButton);

            var nextPageButton = new Button(screen)
            {
                Width = BlockViewerGame.SpriteSize,
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

            fileNameBlocks = new TextBlock[listSize];
            for (int i = 0; i < listSize; i++)
            {
                fileNameBlocks[i] = new TextBlock(screen)
                {
                    ForegroundColor = Color.White,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    TextHorizontalAlignment = HorizontalAlignment.Left,
                    Width = BlockViewerGame.SpriteSize * 8,
                    Height = BlockViewerGame.SpriteSize
                };
                fileNameListPanel.Children.Add(fileNameBlocks[i]);
            }

            var acceptButtonsPanel = new StackPanel(screen)
            {
                HorizontalAlignment = HorizontalAlignment.Right
            };
            basePanel.Children.Add(acceptButtonsPanel);

            var okButton = new CustomButton(screen);
            okButton.TextBlock.Text = "OK";
            acceptButtonsPanel.Children.Add(okButton);

            var cancelButton = new CustomButton(screen);
            cancelButton.TextBlock.Text = "Cancel";
            cancelButton.Click += new RoutedEventHandler(OnCancelButtonClick);
            acceptButtonsPanel.Children.Add(cancelButton);

            cancelButton.Focus();
        }

        public override void Show()
        {
            fileNames = StorageContainer.GetFileNames();
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
            for (int i = 0; i < listSize; i++)
            {
                var fileBlock = fileNameBlocks[i];

                int fileNameIndex = pageIndex * listSize + i;
                if (fileNameIndex < fileNames.Length)
                {
                    fileBlock.Text = fileNames[fileNameIndex];
                    fileBlock.Focusable = true;
                }
                else
                {
                    fileBlock.Text = string.Empty;
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
