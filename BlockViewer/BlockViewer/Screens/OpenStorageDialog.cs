﻿#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Willcraftia.Xna.Framework.UI;
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

        TextButton[] fileNameButtons;

        MessageBox openFileConfirmMessageBox;

        MessageBox noFileErrorMessageBox;

        string targetFileName;

        public OpenStorageDialog(Screen screen, OpenStorageViewModel viewModel)
            : base(screen)
        {
            DataContext = viewModel;

            TitleContent = ControlUtil.CreateDefaultTitle(screen, "Load");
            ShadowOffset = new Vector2(4);

            var basePanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(16, 4, 16, 16)
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

            fileNameButtons = new TextButton[listSize];
            for (int i = 0; i < listSize; i++)
            {
                fileNameButtons[i] = new TextButton(screen)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Width = BlockViewerGame.SpriteSize * 8,
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
                fileNameListPanel.Children.Add(fileNameButtons[i]);
            }
        }

        void OnFileNameButtonClick(Control sender, ref RoutedEventContext context)
        {
            if (openFileConfirmMessageBox == null)
            {
                openFileConfirmMessageBox = new MessageBox(Screen, MessageBoxButton.OKCancel, MessageBoxResult.Cancel);
                openFileConfirmMessageBox.TitleContent = ControlUtil.CreateDefaultTitle(Screen, "Confirmation");
                openFileConfirmMessageBox.ShadowOffset = new Vector2(4);
                openFileConfirmMessageBox.Content.Margin = new Thickness(16, 4, 16, 16);
                openFileConfirmMessageBox.TextBlock.Text = Strings.OpenFileConfirmation;
                openFileConfirmMessageBox.TextBlock.ForegroundColor = Color.White;
                openFileConfirmMessageBox.TextBlock.BackgroundColor = Color.Black;
                openFileConfirmMessageBox.TextBlock.ShadowOffset = new Vector2(2);
                openFileConfirmMessageBox.OKButton.Content.ForegroundColor = Color.White;
                openFileConfirmMessageBox.OKButton.Content.BackgroundColor = Color.Black;
                openFileConfirmMessageBox.OKButton.Content.ShadowOffset = new Vector2(2);
                openFileConfirmMessageBox.OKButton.Margin = new Thickness(0, 0, 4, 0);
                openFileConfirmMessageBox.OKButton.Padding = new Thickness(4);
                openFileConfirmMessageBox.CancelButton.Content.ForegroundColor = Color.White;
                openFileConfirmMessageBox.CancelButton.Content.BackgroundColor = Color.Black;
                openFileConfirmMessageBox.CancelButton.Content.ShadowOffset = new Vector2(2);
                openFileConfirmMessageBox.CancelButton.Margin = new Thickness(4, 0, 0, 0);
                openFileConfirmMessageBox.CancelButton.Padding = new Thickness(4);
                openFileConfirmMessageBox.ButtonsPanel.Padding = new Thickness(4);
                openFileConfirmMessageBox.Closed += new EventHandler(OnMessageBoxClosed);
                ControlUtil.SetDefaultBehavior(openFileConfirmMessageBox.OKButton);
                ControlUtil.SetDefaultBehavior(openFileConfirmMessageBox.CancelButton);
            }

            (DataContext as OpenStorageViewModel).SelectedFileName = null;
            targetFileName = ((sender as Button).Content as TextBlock).Text;
            openFileConfirmMessageBox.Show();

            context.Handled = true;
        }

        void OnMessageBoxClosed(object sender, EventArgs e)
        {
            if (openFileConfirmMessageBox.Result == MessageBoxResult.OK)
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

            // ファイルがない場合は、その旨を MessageBox で表示して終えます。
            if (fileNames.Length == 0)
            {
                ShowNoFileErrorMessageBox();
                return;
            }

            SetFiles(0);

            base.Show();
        }

        void ShowNoFileErrorMessageBox()
        {
            if (noFileErrorMessageBox == null)
            {
                noFileErrorMessageBox = new MessageBox(Screen, MessageBoxButton.OK, MessageBoxResult.OK);
                noFileErrorMessageBox.Padding = new Thickness(16);
                noFileErrorMessageBox.TextBlock.Text = Strings.NoFileInStorageError;
                noFileErrorMessageBox.TextBlock.ForegroundColor = Color.White;
                noFileErrorMessageBox.OKButton.Content.ForegroundColor = Color.White;
                noFileErrorMessageBox.OKButton.Padding = new Thickness(4);
                noFileErrorMessageBox.ButtonsPanel.Padding = new Thickness(4);
            }
            noFileErrorMessageBox.Show();
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
            // 状態を初期化します。
            for (int i = 0; i < listSize; i++)
            {
                var fileNameButton = fileNameButtons[i];
                fileNameButton.TextBlock.Text = string.Empty;
                fileNameButton.Focusable = false;
            }

            // ファイル名を設定します。
            bool focused = false;
            for (int i = 0; i < listSize; i++)
            {
                var fileNameButton = fileNameButtons[i];

                int fileNameIndex = pageIndex * listSize + i;
                if (fileNameIndex < fileNames.Length)
                {
                    fileNameButton.TextBlock.Text = fileNames[fileNameIndex];
                    fileNameButton.Focusable = true;
                    if (!focused)
                    {
                        fileNameButton.Focus();
                        focused = true;
                    }
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
