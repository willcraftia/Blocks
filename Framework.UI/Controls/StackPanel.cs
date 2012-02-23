#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    /// <summary>
    /// 単一行に子 Control を整列させる Control です。
    /// </summary>
    public class StackPanel : Panel
    {
        /// <summary>
        /// 整列させる方向を取得または設定します。
        /// </summary>
        public Orientation Orientation { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        public StackPanel(Screen screen)
            : base(screen)
        {
            Orientation = Orientation.Horizontal;
        }

        protected virtual Size MeasureHorizontalDirection(Size availableSize)
        {
            // 暫定的に自身の希望サイズを計算します。
            var size = new Size();
            size.Width = !float.IsNaN(Width) ? Width : CalculateWidth(availableSize.Width);
            size.Height = !float.IsNaN(Height) ? Height : CalculateHeight(availableSize.Height);

            // 子が利用可能なサイズを計算します。
            var childAvailableSize = size;
            childAvailableSize.Width -= Padding.Left + Padding.Right;
            childAvailableSize.Height -= Padding.Top + Padding.Bottom;

            float measuredWidth = 0;
            float measuredHeight = 0;
            for (int i = 0; i < ChildrenCount; i++)
            {
                var child = GetChild(i);
                child.Measure(childAvailableSize);

                measuredWidth += child.MeasuredSize.Width + child.Margin.Left + child.Margin.Right;
                measuredHeight = Math.Max(measuredHeight, child.MeasuredSize.Height + child.Margin.Top + child.Margin.Bottom);
            }

            size.Width = measuredWidth + Padding.Left + Padding.Right;
            if (float.IsNaN(Height)) size.Height = ClampHeight(measuredHeight + Padding.Top + Padding.Bottom);

            return size;
        }

        protected virtual Size MeasureVerticalDirection(Size availableSize)
        {
            // 暫定的に自身の希望サイズを計算します。
            var size = new Size();
            size.Width = !float.IsNaN(Width) ? Width : CalculateWidth(availableSize.Width);
            size.Height = !float.IsNaN(Height) ? Height : CalculateHeight(availableSize.Height);

            // 子が利用可能なサイズを計算します。
            var childAvailableSize = size;
            childAvailableSize.Width -= Padding.Left + Padding.Right;
            childAvailableSize.Height -= Padding.Top + Padding.Bottom;

            float measuredWidth = 0;
            float measuredHeight = 0;
            for (int i = 0; i < ChildrenCount; i++)
            {
                var child = GetChild(i);
                child.Measure(childAvailableSize);

                measuredWidth = Math.Max(measuredWidth, child.MeasuredSize.Width + child.Margin.Left + child.Margin.Right);
                measuredHeight += child.MeasuredSize.Height + child.Margin.Top + child.Margin.Bottom;
            }

            if (float.IsNaN(Width)) size.Width = ClampWidth(measuredWidth + Padding.Left + Padding.Right);
            size.Height = measuredHeight + Padding.Top + Padding.Bottom;

            return size;
        }

        /// <summary>
        /// 水平方向へ整列させます。
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected virtual Size ArrangeHorizontalDirection(Size finalSize)
        {
            float offsetX = Padding.Left;
            for (int i = 0; i < ChildrenCount; i++)
            {
                var child = GetChild(i);
                offsetX += child.Margin.Left;

                var childBounds = new Rect(child.MeasuredSize);
                childBounds.X = offsetX;

                switch (child.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        childBounds.Y = Padding.Top + child.Margin.Top;
                        break;
                    case VerticalAlignment.Bottom:
                        childBounds.Y = finalSize.Height - child.MeasuredSize.Height - Padding.Bottom - child.Margin.Bottom;
                        break;
                    case VerticalAlignment.Center:
                        var paddedHeight = (finalSize.Height - Padding.Top - Padding.Bottom);
                        childBounds.Y = (paddedHeight - child.MeasuredSize.Height - child.Margin.Top - child.Margin.Bottom) * 0.5f;
                        childBounds.Y += child.Margin.Top;
                        childBounds.Y += Padding.Top;
                        break;
                    case VerticalAlignment.Stretch:
                    default:
                        childBounds.Y = Padding.Top + child.Margin.Top;
                        childBounds.Height = finalSize.Height - Padding.Top - Padding.Bottom - child.Margin.Top - child.Margin.Bottom;
                        break;
                }

                child.Arrange(childBounds);

                // 子が確定した幅の分だけ次の子の座標をずらします。
                offsetX += child.ActualWidth + child.Margin.Right;
            }

            return finalSize;
        }

        /// <summary>
        /// 垂直方向へ整列させます。
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected virtual Size ArrangeVerticalDirection(Size finalSize)
        {
            float offsetY = Padding.Top;
            for (int i = 0; i < ChildrenCount; i++)
            {
                var child = GetChild(i);
                offsetY += child.Margin.Top;

                var childBounds = new Rect(child.MeasuredSize);
                childBounds.Y = offsetY;

                switch (child.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        childBounds.X = Padding.Left + child.Margin.Left;
                        break;
                    case HorizontalAlignment.Right:
                        childBounds.X = finalSize.Width - child.MeasuredSize.Width - Padding.Right - child.Margin.Right;
                        break;
                    case HorizontalAlignment.Center:
                        var paddedWidth = (finalSize.Width - Padding.Left - Padding.Right);
                        childBounds.X = (paddedWidth - child.MeasuredSize.Width - child.Margin.Left - child.Margin.Right) * 0.5f;
                        childBounds.X += child.Margin.Left;
                        childBounds.X += Padding.Left;
                        break;
                    case HorizontalAlignment.Stretch:
                    default:
                        childBounds.X = Padding.Left + child.Margin.Left;
                        childBounds.Width = finalSize.Width - Padding.Left - Padding.Right - child.Margin.Left - child.Margin.Right;
                        break;
                }

                child.Arrange(childBounds);

                // 子が確定した幅の分だけ次の子の座標をずらします。
                offsetY += child.ActualHeight + child.Margin.Bottom;
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Orientation == Orientation.Horizontal)
            {
                return MeasureHorizontalDirection(availableSize);
            }
            else
            {
                return MeasureVerticalDirection(availableSize);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Orientation == Orientation.Horizontal)
            {
                return ArrangeHorizontalDirection(finalSize);
            }
            else
            {
                return ArrangeVerticalDirection(finalSize);
            }
        }
    }
}
