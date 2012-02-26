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

                maxChildMeasuredWidth += child.MeasuredSize.Width;
                maxChildMeasuredHeight = Math.Max(maxChildMeasuredHeight, child.MeasuredSize.Height);
            }

            if (float.IsNaN(Height)) controlSize.Width = ClampWidth(maxChildMeasuredWidth + widthPadding);
            if (float.IsNaN(Height)) controlSize.Height = ClampHeight(maxChildMeasuredHeight + heightPadding);

            // 外側の余白を含めて描画に必要な希望サイズとします。
            return new Size
            {
                Width = controlSize.Width + Margin.Left + Margin.Right,
                Height = controlSize.Height + Margin.Top + Margin.Bottom
            };
        }

        protected virtual Size MeasureVerticalDirection(Size availableSize)
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
        /// 水平方向へ整列させます。
        /// </summary>
        /// <param name="arrangeSize"></param>
        /// <returns></returns>
        protected virtual Size ArrangeHorizontalDirection(Size arrangeSize)
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

            float offsetX = paddedBounds.Left;
            for (int i = 0; i < ChildrenCount; i++)
            {
                var child = GetChild(i);
                var childBounds = new Rect
                {
                    X = offsetX,
                    Width = child.MeasuredSize.Width,
                    Height = child.MeasuredSize.Height
                };

                switch (child.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        childBounds.Y = paddedBounds.Top;
                        break;
                    case VerticalAlignment.Bottom:
                        childBounds.Y = paddedBounds.Bottom - child.MeasuredSize.Height;
                        break;
                    case VerticalAlignment.Center:
                        childBounds.Y = paddedBounds.Top + (paddedBounds.Height - child.MeasuredSize.Height) * 0.5f;
                        break;
                    case VerticalAlignment.Stretch:
                    default:
                        childBounds.Y = paddedBounds.Top;
                        childBounds.Height = paddedBounds.Height;
                        break;
                }

                child.Arrange(childBounds);

                // 今の子の右端が次の子の左端です。
                offsetX = childBounds.Right;
            }

            return controlSize;
        }

        /// <summary>
        /// 垂直方向へ整列させます。
        /// </summary>
        /// <param name="arrangeSize"></param>
        /// <returns></returns>
        protected virtual Size ArrangeVerticalDirection(Size arrangeSize)
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
