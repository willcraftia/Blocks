#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    /// <summary>
    /// 単一行に子 Control を整列させる Control です。
    /// </summary>
    public class StackPanel : Control
    {
        /// <summary>
        /// 整列させる方向を取得または設定します。
        /// </summary>
        public Orientation Orientation { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public StackPanel()
        {
            FocusNavigationMode = FocusNavigationMode.Continue;
            Orientation = Orientation.Horizontal;
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

        protected virtual Size MeasureHorizontalDirection(Size availableSize)
        {
            var size = new Size();
            size.Width = CalculateBaseWidth(availableSize.Width);
            size.Height = CalculateBaseHeight(availableSize.Height);

            float measuredWidth = 0;
            foreach (var child in Children)
            {
                child.Measure(size);
                var childMeasuredSize = child.MeasuredSize;
                var childMargin = child.Margin;
                measuredWidth += childMeasuredSize.Width + childMargin.Left + childMargin.Right;
            }

            return new Size(measuredWidth, size.Height);
        }

        protected virtual Size MeasureVerticalDirection(Size availableSize)
        {
            var size = new Size();
            size.Width = CalculateBaseWidth(availableSize.Width);
            size.Height = CalculateBaseHeight(availableSize.Height);

            float measuredHeight = 0;
            foreach (var child in Children)
            {
                child.Measure(size);
                var childMeasuredSize = child.MeasuredSize;
                var childMargin = child.Margin;
                measuredHeight += childMeasuredSize.Height + childMargin.Top + childMargin.Bottom;
            }

            return new Size(size.Width, measuredHeight);
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

        /// <summary>
        /// 水平方向へ整列させます。
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected virtual Size ArrangeHorizontalDirection(Size finalSize)
        {
            float offsetX = 0;
            foreach (var child in Children)
            {
                var childMeasuredSize = child.MeasuredSize;
                var childMargin = child.Margin;

                offsetX += childMargin.Left;

                var childBounds = new Rect();
                childBounds.X = offsetX;
                childBounds.Y = childMargin.Top;
                childBounds.Width = childMeasuredSize.Width;

                if (float.IsNaN(child.Height))
                {
                    childBounds.Height = MathHelper.Min(finalSize.Height, child.MaxHeight) - (childMargin.Top + childMargin.Bottom);
                }
                else
                {
                    childBounds.Height = childMeasuredSize.Height;
                }

                switch (child.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        {
                            break;
                        }
                    case VerticalAlignment.Bottom:
                        {
                            childBounds.Y += finalSize.Height - (childBounds.Height + childMargin.Top + childMargin.Bottom);
                            break;
                        }
                    case UI.VerticalAlignment.Center:
                    default:
                        {
                            childBounds.Y += (finalSize.Height - (childBounds.Height + childMargin.Top + childMargin.Bottom)) * 0.5f;
                            break;
                        }
                }

                child.Arrange(childBounds);

                // 子が確定した幅の分だけ次の子の座標をずらします。
                offsetX += child.ActualWidth + childMargin.Right;
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
            float offsetY = 0;
            foreach (var child in Children)
            {
                var childMeasuredSize = child.MeasuredSize;
                var childMargin = child.Margin;

                offsetY += childMargin.Top;

                var childBounds = new Rect();
                childBounds.X = childMargin.Left;
                childBounds.Y = offsetY;
                childBounds.Height = childMeasuredSize.Height;

                if (float.IsNaN(child.Width))
                {
                    childBounds.Width = MathHelper.Min(finalSize.Width, child.MaxWidth) - (childMargin.Left + childMargin.Right);
                }
                else
                {
                    childBounds.Width = childMeasuredSize.Width;
                }

                switch (child.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        {
                            break;
                        }
                    case HorizontalAlignment.Right:
                        {
                            childBounds.X += finalSize.Width - (childBounds.Width - childMargin.Left + childMargin.Right);
                            break;
                        }
                    case HorizontalAlignment.Center:
                    default:
                        {
                            childBounds.X += (finalSize.Width - (childBounds.Width - childMargin.Left + childMargin.Right)) * 0.5f;
                            break;
                        }
                }

                child.Arrange(childBounds);

                // 子が確定した幅の分だけ次の子の座標をずらします。
                offsetY += child.ActualHeight + childMargin.Bottom;
            }

            return finalSize;
        }
    }
}
