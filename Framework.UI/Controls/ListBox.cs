#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    /// <summary>
    /// リスト上に並べた項目を選択するための Selector です。
    /// </summary>
    public class ListBox : Selector
    {
        #region Routing Events

        /// <summary>
        /// 項目が選択された時に発生します。
        /// 項目からの Selected イベント受信のために使用します。
        /// </summary>
        protected static readonly string SelectedEvent = "Selected";

        /// <summary>
        /// 項目が選択が解除された時に発生します。
        /// 項目からの Unselected イベント受信のために使用します。
        /// </summary>
        protected static readonly string UnselectedEvent = "Unselected";

        #endregion

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        public ListBox(Screen screen)
            : base(screen)
        {
            AddHandler(SelectedEvent, OnItemSelected);
            AddHandler(UnselectedEvent, OnItemUnselected);
        }

        /// <summary>
        /// 項目が発生させた Selected イベントを受け取り、
        /// 選択された項目のインデックスを SelectedIndex プロパティへ設定します。
        /// 既存の選択項目が ListBoxItem の場合、
        /// 選択解除のために、その IsSelected プロパティは false に設定されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="context"></param>
        protected virtual void OnItemSelected(Control sender, ref RoutedEventContext context)
        {
            var item = SelectedItem as ListBoxItem;
            if (item != null) item.IsSelected = false;

            SelectedIndex = Items.IndexOf(context.Source);
        }

        /// <summary>
        /// 項目が発生させた Unselected イベントを受け取り、
        /// SelectedIndex プロパティを -1 に設定します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="context"></param>
        protected virtual void OnItemUnselected(Control sender, ref RoutedEventContext context)
        {
            if (SelectedItem == context.Source) SelectedIndex = -1;
        }

        /// <summary>
        /// Orientation プロパティが Vertical の StackPanel.MeasureOverride と同じロジックで測定します。
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            // 暫定的に Control のサイズを計算します。
            var controlSize = new Size(Width, Height);
            if (float.IsNaN(controlSize.Width))
                controlSize.Width = CalculateWidth(availableSize.Width - Margin.Left - Margin.Right);
            if (float.IsNaN(controlSize.Height))
                controlSize.Height = CalculateWidth(availableSize.Height - Margin.Top - Margin.Bottom);

            // 子の利用できるサイズは、内側の余白をとった領域のサイズです。
            var widthPadding = Padding.Left + Padding.Right;
            var heightPadding = Padding.Top + Padding.Bottom;
            var childAvailableSize = new Size
            {
                Width = controlSize.Width - widthPadding,
                Height = controlSize.Height - heightPadding
            };

            // 子の希望サイズを定めます。
            float maxChildMeasuredWidth = 0;
            float maxChildMeasuredHeight = 0;
            for (int i = 0; i < ChildrenCount; i++)
            {
                var child = GetChild(i);
                child.Measure(childAvailableSize);

                maxChildMeasuredWidth = Math.Max(maxChildMeasuredWidth, child.MeasuredSize.Width);
                maxChildMeasuredHeight += child.MeasuredSize.Height;
            }

            if (float.IsNaN(Width)) controlSize.Width = ClampWidth(maxChildMeasuredWidth + widthPadding);
            if (float.IsNaN(Height)) controlSize.Height = ClampHeight(maxChildMeasuredHeight + heightPadding);

            // 外側の余白を含めて描画に必要な希望サイズとします。
            return new Size
            {
                Width = controlSize.Width + Margin.Left + Margin.Right,
                Height = controlSize.Height + Margin.Top + Margin.Bottom
            };
        }

        /// <summary>
        /// Orientation プロパティが Vertical の StackPanel.ArrangeOverride と同じロジックで配置します。
        /// </summary>
        /// <param name="arrangeSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var controlSize = new Size
            {
                Width = arrangeSize.Width - Margin.Left - Margin.Right,
                Height = arrangeSize.Height - Margin.Top - Margin.Bottom
            };

            var paddedBounds = new Rect
            {
                X = Padding.Left,
                Y = Padding.Top,
                Width = controlSize.Width - Padding.Left - Padding.Right,
                Height = controlSize.Height - Padding.Top - Padding.Bottom
            };

            float offsetY = paddedBounds.Top;
            for (int i = 0; i < ChildrenCount; i++)
            {
                var child = GetChild(i);
                var childBounds = new Rect
                {
                    Y = offsetY,
                    Width = child.MeasuredSize.Width,
                    Height = child.MeasuredSize.Height
                };

                switch (child.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        childBounds.X = paddedBounds.Left;
                        break;
                    case HorizontalAlignment.Right:
                        childBounds.X = paddedBounds.Right - child.MeasuredSize.Width;
                        break;
                    case HorizontalAlignment.Center:
                        childBounds.X = paddedBounds.Left + (paddedBounds.Width - child.MeasuredSize.Width) * 0.5f;
                        break;
                    case HorizontalAlignment.Stretch:
                    default:
                        childBounds.X = paddedBounds.Left;
                        childBounds.Width = paddedBounds.Width;
                        break;
                }

                child.Arrange(childBounds);

                // 今の子の下端が次の子の上端です。
                offsetY = childBounds.Bottom;
            }

            return controlSize;
        }
    }
}
