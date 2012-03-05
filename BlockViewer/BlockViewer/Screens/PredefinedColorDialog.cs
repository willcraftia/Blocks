#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    //
    // MEMO
    //
    // あとで UI Framework か Blocks UI Foundation などでまとめる予定。
    //

    public delegate void PredefinedColorSelected(PredefinedColor predefinedColor);

    public sealed class PredefinedColorDialog : OverlayDialogBase
    {
        #region PredefinedColorGrid

        class PredefinedColorGrid : ContentControl
        {
            #region ItemButton

            class ItemButton : Button
            {
                public ItemButton(Screen screen)
                    : base(screen)
                {
                    Width = 32;
                    Height = 32;
                    Padding = new Thickness(4);

                    Content = new Canvas(screen)
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                }
            }

            #endregion

            PredefinedColorDialog owner;

            const int columnCount = 10;

            const int rowCount = 5;

            TextBlock pageTextBlock;

            ItemButton[] itemButtons = new ItemButton[columnCount * rowCount];

            public int CurrentPageIndex { get; set; }

            int PageCount
            {
                get
                {
                    var pageCount = owner.PredefinedColors.Count / itemButtons.Length;
                    if (owner.PredefinedColors.Count % itemButtons.Length != 0) pageCount++;
                    return pageCount;
                }
            }

            public PredefinedColorGrid(Screen screen, PredefinedColorDialog owner)
                : base(screen)
            {
                this.owner = owner;

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

                var vColorPanel = new StackPanel(screen)
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                stackPanel.Children.Add(vColorPanel);

                for (int i = 0; i < rowCount; i++)
                {
                    var hColorPanel = new StackPanel(screen);
                    vColorPanel.Children.Add(hColorPanel);

                    for (int j = i * columnCount; j < (i + 1) * columnCount; j++)
                    {
                        itemButtons[j] = new ItemButton(screen);
                        hColorPanel.Children.Add(itemButtons[j]);

                        int mod = j % columnCount;
                        if (mod == 0)
                        {
                            itemButtons[j].KeyDown += new RoutedEventHandler(OnLeftColorButtonKeyDown);
                        }
                        else if (mod == columnCount - 1)
                        {
                            itemButtons[j].KeyDown += new RoutedEventHandler(OnRightColorButtonKeyDown);
                        }

                        itemButtons[j].GotFocus += new RoutedEventHandler(OnItemButtonGotFocus);
                        itemButtons[j].LostFocus += new RoutedEventHandler(OnItemButtonLostFocus);
                        itemButtons[j].Click += new RoutedEventHandler(OnItemButtonClick);
                    }
                }
            }

            public void ReloadPage()
            {
                // 状態を初期化します。
                for (int i = 0; i < itemButtons.Length; i++)
                {
                    var itemButton = itemButtons[i];

                    int colorIndex = CurrentPageIndex * itemButtons.Length + i;
                    if (colorIndex < owner.PredefinedColors.Count)
                    {
                        var predefinedColor = owner.PredefinedColors[colorIndex];
                        itemButton.DataContext = predefinedColor;
                        itemButton.Content.BackgroundColor = predefinedColor.Color;
                        itemButton.Enabled = true;
                    }
                    else
                    {
                        // フォーカスが設定されていたボタンが未使用になった場合、
                        // 先頭のボタンにフォーカスを設定します。
                        if (itemButton.Focused)
                        {
                            if (!itemButtons[0].Focus())
                            {
                                // 先頭のボタンに設定できない場合は Cancel ボタンに設定します。
                                owner.cancelButton.Focus();
                            }
                        }

                        itemButton.DataContext = null;
                        itemButton.Content.BackgroundColor = Color.Transparent;
                        itemButton.Enabled = false;
                    }
                }

                var currentPageNo = CurrentPageIndex + 1;
                pageTextBlock.Text = currentPageNo + "/" + PageCount;
            }

            void OnItemButtonGotFocus(Control sender, ref RoutedEventContext context)
            {
                var itemButton = sender as ItemButton;

                itemButton.Padding = new Thickness(0);
            }

            void OnItemButtonLostFocus(Control sender, ref RoutedEventContext context)
            {
                var itemButton = sender as ItemButton;

                itemButton.Padding = new Thickness(4);
            }

            void OnItemButtonClick(Control sender, ref RoutedEventContext context)
            {
                var itemButton = sender as ItemButton;

                if (owner.Selected != null) owner.Selected(itemButton.DataContext as PredefinedColor);

                owner.Close();
            }

            void OnLeftColorButtonKeyDown(Control sender, ref RoutedEventContext context)
            {
                if (Screen.KeyboardDevice.IsKeyPressed(Keys.Left))
                {
                    BackPage();
                    context.Handled = true;
                }
            }

            void OnRightColorButtonKeyDown(Control sender, ref RoutedEventContext context)
            {
                if (Screen.KeyboardDevice.IsKeyPressed(Keys.Right))
                {
                    ForwardPage();
                    context.Handled = true;
                }
            }

            void BackPage()
            {
                CurrentPageIndex--;
                // 先頭を越えるならば末尾のページを設定します。
                if (CurrentPageIndex < 0) CurrentPageIndex = PageCount - 1;

                ReloadPage();
            }

            void ForwardPage()
            {
                CurrentPageIndex++;
                // 末尾を越えるならば先頭のページを設定します。
                if (PageCount - 1 < CurrentPageIndex) CurrentPageIndex = 0;

                ReloadPage();
            }
        }

        #endregion

        #region PredefinedColorList

        class PredefinedColorList : ContentControl
        {
            #region ItemButton

            class ItemButton : Button
            {
                public TextBlock NameTextBlock { get; private set; }

                public Canvas ColorCanvas { get; private set; }

                public ItemButton(Screen screen)
                    : base(screen)
                {
                    var stackPanel = new StackPanel(screen)
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Padding = new Thickness(16, 0, 16, 0)
                    };
                    Content = stackPanel;

                    NameTextBlock = new TextBlock(screen)
                    {
                        Width = 200,
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black,
                        ShadowOffset = new Vector2(2),
                        TextHorizontalAlignment = HorizontalAlignment.Left
                    };
                    stackPanel.Children.Add(NameTextBlock);

                    ColorCanvas = new Canvas(screen)
                    {
                        Width = 124,
                        Height = 30,
                        Margin = new Thickness(2)
                    };
                    stackPanel.Children.Add(ColorCanvas);
                }
            }

            #endregion

            const int listSize = 5;

            PredefinedColorDialog owner;

            TextBlock pageTextBlock;

            ItemButton[] itemButtons = new ItemButton[listSize];

            public int CurrentPageIndex { get; set; }

            int PageCount
            {
                get
                {
                    var pageCount = owner.PredefinedColors.Count / listSize;
                    if (owner.PredefinedColors.Count % listSize != 0) pageCount++;
                    return pageCount;
                }
            }

            public PredefinedColorList(Screen screen, PredefinedColorDialog owner)
                : base(screen)
            {
                this.owner = owner;

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

                for (int i = 0; i < listSize; i++)
                {
                    itemButtons[i] = new ItemButton(screen);
                    itemButtons[i].KeyDown += new RoutedEventHandler(OnListItemButtonKeyDown);
                    itemButtons[i].Click += new RoutedEventHandler(OnListItemButtonClick);
                    stackPanel.Children.Add(itemButtons[i]);
                }
            }

            public void ReloadPage()
            {
                // 状態を初期化します。
                for (int i = 0; i < listSize; i++)
                {
                    var listItem = itemButtons[i];

                    int colorIndex = CurrentPageIndex * listSize + i;
                    if (colorIndex < owner.PredefinedColors.Count)
                    {
                        var predefinedColor = owner.PredefinedColors[colorIndex];
                        listItem.DataContext = predefinedColor;
                        listItem.NameTextBlock.Text = predefinedColor.Name;
                        listItem.ColorCanvas.BackgroundColor = predefinedColor.Color;
                        listItem.Enabled = true;
                    }
                    else
                    {
                        // フォーカスが設定されていたボタンが未使用になった場合、
                        // 先頭のボタンにフォーカスを設定します。
                        if (listItem.Focused)
                        {
                            if (!itemButtons[0].Focus())
                            {
                                // 先頭のボタンに設定できない場合は Cancel ボタンに設定します。
                                owner.cancelButton.Focus();
                            }
                        }

                        listItem.DataContext = null;
                        listItem.NameTextBlock.Text = null;
                        listItem.ColorCanvas.BackgroundColor = Color.Transparent;
                        listItem.Enabled = false;
                    }
                }

                var currentPageNo = CurrentPageIndex + 1;
                pageTextBlock.Text = currentPageNo + "/" + PageCount;
            }

            void OnListItemButtonClick(Control sender, ref RoutedEventContext context)
            {
                var itemButton = sender as ItemButton;

                if (owner.Selected != null) owner.Selected(itemButton.DataContext as PredefinedColor);

                owner.Close();
            }

            void OnListItemButtonKeyDown(Control sender, ref RoutedEventContext context)
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
                CurrentPageIndex--;
                // 先頭を越えるならば末尾のページを設定します。
                if (CurrentPageIndex < 0) CurrentPageIndex = PageCount - 1;

                ReloadPage();
            }

            void ForwardPage()
            {
                CurrentPageIndex++;
                // 末尾を越えるならば先頭のページを設定します。
                if (PageCount - 1 < CurrentPageIndex) CurrentPageIndex = 0;

                ReloadPage();
            }
        }

        #endregion

        TabControl tab;

        PredefinedColorGrid predefinedColorGrid;

        PredefinedColorList predefinedColorList;

        Button viewModeButton;

        Button cancelButton;

        FloatLerpAnimation openAnimation;

        FloatLerpAnimation closeAnimation;

        /// <summary>
        /// 選択対象とする PredefinedColor のリストを取得します。
        /// デフォルトでは PredefinedColor.PredefinedColors に含まれる
        /// PredefinedColor が設定されています。
        /// 選択させたくない PredefinedColor がある場合、
        /// それらをこのリストから削除するか、
        /// リスト内の要素を全て削除した後に必要な PredefinedColor のみを追加します。
        /// </summary>
        public List<PredefinedColor> PredefinedColors { get; private set; }

        /// <summary>
        /// PredefinedColor が選択された時に呼び出されるメソッドを取得または設定します。
        /// </summary>
        public PredefinedColorSelected Selected { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen"></param>
        public PredefinedColorDialog(Screen screen)
            : base(screen)
        {
            PredefinedColors = new List<PredefinedColor>();
            PredefinedColors.AddRange(PredefinedColor.PredefinedColors);
            PredefinedColors.Sort((x, y) => x.Name.CompareTo(y.Name));

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

            tab = new TabControl(screen)
            {
                SelectedIndex = 0
            };
            stackPanel.Children.Add(tab);

            predefinedColorGrid = new PredefinedColorGrid(screen, this)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            tab.Items.Add(predefinedColorGrid);

            predefinedColorList = new PredefinedColorList(screen, this)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            tab.Items.Add(predefinedColorList);

            var separator = ControlUtil.CreateDefaultSeparator(screen);
            stackPanel.Children.Add(separator);

            viewModeButton = ControlUtil.CreateDefaultDialogButton(screen, Strings.ListViewModeButton);
            viewModeButton.Click += new RoutedEventHandler(OnViewModeButtonClick);
            stackPanel.Children.Add(viewModeButton);

            var sortByNameButton = ControlUtil.CreateDefaultDialogButton(screen, Strings.SortByNameButton);
            sortByNameButton.Click += new RoutedEventHandler(OnSortByNameClick);
            stackPanel.Children.Add(sortByNameButton);

            var sortByColorButton = ControlUtil.CreateDefaultDialogButton(screen, Strings.SortByColorButton);
            sortByColorButton.Click += new RoutedEventHandler(OnSortByColorClick);
            stackPanel.Children.Add(sortByColorButton);

            cancelButton = ControlUtil.CreateDefaultDialogButton(screen, Strings.CancelButton);
            cancelButton.Click += (Control s, ref RoutedEventContext c) => Close();
            stackPanel.Children.Add(cancelButton);

            const float windowWidth = 400;

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
            // ページを初期化します。
            predefinedColorGrid.ReloadPage();
            predefinedColorList.ReloadPage();

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

        void OnViewModeButtonClick(Control sender, ref RoutedEventContext context)
        {
            if (tab.SelectedIndex == 0)
            {
                tab.SelectedIndex = 1;
                (viewModeButton.Content as TextBlock).Text = Strings.GridViewModeButton;
            }
            else
            {
                tab.SelectedIndex = 0;
                (viewModeButton.Content as TextBlock).Text = Strings.ListViewModeButton;
            }
        }

        void OnSortByNameClick(Control sender, ref RoutedEventContext context)
        {
            PredefinedColors.Sort((x, y) => x.Name.CompareTo(y.Name));

            // ページをリセットしてからリロードします。
            predefinedColorGrid.CurrentPageIndex = 0;
            predefinedColorGrid.ReloadPage();
            predefinedColorList.CurrentPageIndex = 0;
            predefinedColorList.ReloadPage();
        }

        void OnSortByColorClick(Control sender, ref RoutedEventContext context)
        {
            PredefinedColors.Sort(SortPredefinedColorByColor);
            //PredefinedColors.Sort((x, y) => x.Color.PackedValue.CompareTo(y.Color.PackedValue));

            // ページをリセットしてからリロードします。
            predefinedColorGrid.CurrentPageIndex = 0;
            predefinedColorGrid.ReloadPage();
            predefinedColorList.CurrentPageIndex = 0;
            predefinedColorList.ReloadPage();
        }

        int SortPredefinedColorByColor(PredefinedColor x, PredefinedColor y)
        {
            var xColor = x.Color;
            var yColor = y.Color;

            if (xColor.R != yColor.R) return xColor.R.CompareTo(yColor.R);
            if (xColor.G != yColor.G) return xColor.G.CompareTo(yColor.G);
            if (xColor.B != yColor.B) return xColor.B.CompareTo(yColor.B);
            if (xColor.A != yColor.A) return xColor.A.CompareTo(yColor.A);

            return 0;
        }
    }
}
