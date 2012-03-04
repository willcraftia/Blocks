#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class PredefinedColorDialog : OverlayDialogBase
    {
        const int columnCount = 5;

        const int rowCount = 5;

        int currentPageIndex;

        TextBlock pageTextBlock;

        ColorButton[] colorButtons;

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

            colorButtons = new ColorButton[columnCount * rowCount];
            for (int i = 0; i < rowCount; i++)
            {
                var hColorPanel = new StackPanel(screen);
                vColorPanel.Children.Add(hColorPanel);

                for (int j = i * rowCount; j < (i + 1) * rowCount; j++)
                {
                    colorButtons[j] = new ColorButton(screen)
                    {
                        Width = 30,
                        Height = 30,
                        Margin = new Thickness(2)
                    };
                    hColorPanel.Children.Add(colorButtons[j]);
                }
            }

            var separator = ControlUtil.CreateDefaultSeparator(screen);
            stackPanel.Children.Add(separator);

            var sortByName = ControlUtil.CreateDefaultDialogButton(screen, "Sort by Name");
            sortByName.Click += new RoutedEventHandler(OnSortByNameClick);
            stackPanel.Children.Add(sortByName);

            var sortByColor = ControlUtil.CreateDefaultDialogButton(screen, "Sort by Color");
            sortByColor.Click += new RoutedEventHandler(OnSortByColorClick);
            stackPanel.Children.Add(sortByColor);

            cancelButton = ControlUtil.CreateDefaultDialogButton(screen, Strings.CancelButton);
            cancelButton.Click += (Control s, ref RoutedEventContext c) => Close();
            stackPanel.Children.Add(cancelButton);

            const float windowWidth = 280;

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

        void OnSortByNameClick(Control sender, ref RoutedEventContext context)
        {
            PredefinedColors.Sort((x, y) => x.Name.CompareTo(y.Name));

            ReloadPage();
        }

        void OnSortByColorClick(Control sender, ref RoutedEventContext context)
        {
            PredefinedColors.Sort(SortPredefinedColorByColor);

            ReloadPage();
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

        void BackPage()
        {
            currentPageIndex--;
            // 先頭を越えるならば末尾のページを設定します。
            if (currentPageIndex < 0)
                currentPageIndex = PredefinedColors.Count / colorButtons.Length;

            ReloadPage();
        }

        void ForwardPage()
        {
            currentPageIndex++;
            // 末尾を越えるならば先頭のページを設定します。
            if (PredefinedColors.Count / colorButtons.Length < currentPageIndex)
                currentPageIndex = 0;

            ReloadPage();
        }

        void Sort(IComparer<PredefinedColor> comparer)
        {
            PredefinedColors.Sort(comparer);

            ReloadPage();
        }

        void ReloadPage()
        {
            // 状態を初期化します。
            for (int i = 0; i < colorButtons.Length; i++)
            {
                var colorButton = colorButtons[i];

                int colorIndex = currentPageIndex * colorButtons.Length + i;
                if (colorIndex < PredefinedColors.Count)
                {
                    var predefinedColor = PredefinedColors[colorIndex];
                    colorButton.ForegroundColor = predefinedColor.Color;
                    colorButton.Enabled = true;
                }
                else
                {
                    // フォーカスが設定されていたボタンが未使用になった場合、
                    // 先頭のボタンにフォーカスを設定します。
                    if (colorButton.Focused)
                    {
                        if (!colorButtons[0].Focus())
                        {
                            // 先頭のボタンに設定できない場合は Cancel ボタンに設定します。
                            cancelButton.Focus();
                        }
                    }

                    colorButton.ForegroundColor = Color.Transparent;
                    colorButton.Enabled = false;
                }
            }

            var currentPageNo = currentPageIndex + 1;
            var lastPageNo = PredefinedColors.Count / colorButtons.Length + 1;

            pageTextBlock.Text = currentPageNo + "/" + lastPageNo;
        }
    }
}
