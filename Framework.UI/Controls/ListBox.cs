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
        /// ListBoxSelectionMode。
        /// </summary>
        ListBoxSelectionMode selectionMode = ListBoxSelectionMode.Single;

        /// <summary>
        /// ListBoxSelectionMode を取得または設定します。
        /// デフォルトは ListBoxSelectionMode.Single です。
        /// </summary>
        public ListBoxSelectionMode SelectionMode
        {
            get { return selectionMode; }
            set
            {
                if (selectionMode == value) return;

                selectionMode = value;

                // Single になったならば、複数選択のうち、最初の項目以外の選択を解除します。
                if (selectionMode == ListBoxSelectionMode.Single && SelectedIndex != -1)
                {
                    for (int i = 0; i < Items.Count; i++)
                    {
                        if (i == SelectedIndex) continue;

                        var item = Items[i] as ListBoxItem;
                        if (item.IsSelected) item.IsSelected = false;
                    }
                }
            }
        }

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
        /// SelectedIndex プロパティを更新します。
        /// SelectionMode プロパティが Single の場合には、
        /// それまで選択されていた項目の IsSelected プロパティは false に設定されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="context"></param>
        protected virtual void OnItemSelected(Control sender, ref RoutedEventContext context)
        {
            if (selectionMode == ListBoxSelectionMode.Single)
            {
                var item = SelectedItem as ListBoxItem;
                if (item != null) item.IsSelected = false;
            }

            SelectedIndex = FindFirstSelectedIndex();
        }

        /// <summary>
        /// 項目が発生させた Unselected イベントを受け取り、
        /// SelectedIndex プロパティを更新します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="context"></param>
        protected virtual void OnItemUnselected(Control sender, ref RoutedEventContext context)
        {
            SelectedIndex = FindFirstSelectedIndex();
        }

        /// <summary>
        /// item が ListBoxItem の場合、それをそのまま項目として追加します。
        /// それ以外の場合、
        /// Content プロパティに item を設定した ListBoxItem インスタンスを生成し、
        /// それを項目として追加します。
        /// </summary>
        /// <param name="item"></param>
        protected override void AddItem(Control item)
        {
            var target = item;

            if (!(item is ListBoxItem))
            {
                target = new ListBoxItem(Screen)
                {
                    Content = item
                };
            }

            base.AddItem(target);
        }

        /// <summary>
        /// item が ListBoxItem の場合、それをそのまま項目から削除します。
        /// それ以外の場合、
        /// item に一致する Content プロパティを持つ ListBoxItem インスタンスを探し、
        /// それを項目から削除します。
        /// </summary>
        /// <param name="item"></param>
        protected override void RemoveItem(Control item)
        {
            var target = item;

            if (!(item is ListBoxItem))
            {
                target = null;
                foreach (ListBoxItem wrappedItem in Items)
                {
                    if (wrappedItem.Content == item)
                    {
                        target = wrappedItem;
                        break;
                    }
                }

                if (target == null)
                    throw new InvalidOperationException("No wrapped item exists.");
            }

            base.RemoveItem(target);
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

        /// <summary>
        /// Items プロパティから選択されている最初の項目を探し、そのインデクスを返します。
        /// </summary>
        /// <returns></returns>
        int FindFirstSelectedIndex()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if ((Items[i] as ListBoxItem).IsSelected)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
