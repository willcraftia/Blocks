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
        public StackPanel(Screen screen)
            : base(screen)
        {
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
            var margin = Margin;

            if (float.IsNaN(Width))
            {
                // 幅が未設定ならば可能な限り最大の幅を希望します。
                if (MinWidth < MaxWidth)
                {
                    size.Width = MathHelper.Clamp(availableSize.Width, MinWidth, MaxWidth);
                }
                else
                {
                    // 上限と下限の関係に不整合があるならば最大の下限を希望します。
                    size.Width = MathHelper.Max(availableSize.Width, MinWidth);
                }
                // 余白で調整します。
                size.Width = MathHelper.Max(MinWidth, size.Width - margin.Left - margin.Right);
            }
            else
            {
                // 幅が設定されているならばそのまま希望します。
                size.Width = Width;
            }

            if (float.IsNaN(Height))
            {
                // 高さが未設定ならば可能な限り最大の幅を希望します。
                if (MinHeight < MaxHeight)
                {
                    size.Height = MathHelper.Clamp(availableSize.Height, MinHeight, MaxHeight);
                }
                else
                {
                    // 上限と下限の関係に不整合があるならば最大の下限を希望します。
                    size.Height = MathHelper.Max(availableSize.Height, MinHeight);
                }
                // 余白で調整します。
                size.Height = MathHelper.Max(MinHeight, size.Height - margin.Top - margin.Bottom);
            }
            else
            {
                // 高さが設定されているならばそのまま希望します。
                size.Height = Height;
            }

            float measuredWidth = 0;
            foreach (var child in Children)
            {
                child.Measure(size);
                var childMeasuredSize = child.MeasuredSize;
                var childMargin = child.Margin;
                measuredWidth += childMeasuredSize.Width + margin.Left + margin.Right;
            }

            return new Size(measuredWidth, size.Height);
        }

        protected virtual Size MeasureVerticalDirection(Size availableSize)
        {
            var size = new Size();
            var margin = Margin;

            if (float.IsNaN(Width))
            {
                // 幅が未設定ならば可能な限り最大の幅を希望します。
                if (MinWidth < MaxWidth)
                {
                    size.Width = MathHelper.Clamp(availableSize.Width, MinWidth, MaxWidth);
                }
                else
                {
                    // 上限と下限の関係に不整合があるならば最大の下限を希望します。
                    size.Width = MathHelper.Max(availableSize.Width, MinWidth);
                }
                // 余白で調整します。
                size.Width = MathHelper.Max(MinWidth, size.Width - margin.Left - margin.Right);
            }
            else
            {
                // 幅が設定されているならばそのまま希望します。
                size.Width = Width;
            }

            if (float.IsNaN(Height))
            {
                // 高さが未設定ならば可能な限り最大の幅を希望します。
                if (MinHeight < MaxHeight)
                {
                    size.Height = MathHelper.Clamp(availableSize.Height, MinHeight, MaxHeight);
                }
                else
                {
                    // 上限と下限の関係に不整合があるならば最大の下限を希望します。
                    size.Height = MathHelper.Max(availableSize.Height, MinHeight);
                }
                // 余白で調整します。
                size.Height = MathHelper.Max(MinHeight, size.Height - margin.Top - margin.Bottom);
            }
            else
            {
                // 高さが設定されているならばそのまま希望します。
                size.Height = Height;
            }

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
            // シンプルに実装します。
            // 幅も高さも指定された値をそのまま使うことにします。

            var point = new Point();
            foreach (var child in Children)
            {
                var childMeasuredSize = child.MeasuredSize;
                var childMargin = child.Margin;
                point.X += childMargin.Left;
                point.Y = childMargin.Top;
                switch (child.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        {
                            break;
                        }
                    case VerticalAlignment.Bottom:
                        {
                            point.Y += finalSize.Height- (childMeasuredSize.Height + childMargin.Top + childMargin.Bottom);
                            break;
                        }
                    case UI.VerticalAlignment.Center:
                    default:
                        {
                            point.Y += (finalSize.Height - (childMeasuredSize.Height + childMargin.Top + childMargin.Bottom)) * 0.5f;
                            break;
                        }
                }

                child.Arrange(new Rect(point, childMeasuredSize));

                // 子が確定した幅の分だけ次の子の座標をずらします。
                point.X += child.ActualWidth + childMargin.Right;
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
            // シンプルに実装します。
            // 幅も高さも指定された値をそのまま使うことにします。

            var point = new Point();
            foreach (var child in Children)
            {
                var childMeasuredSize = child.MeasuredSize;
                var childMargin = child.Margin;
                point.X = childMargin.Left;
                point.Y += childMargin.Top;
                switch (child.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        {
                            break;
                        }
                    case HorizontalAlignment.Right:
                        {
                            point.X += finalSize.Width - (childMeasuredSize.Width - childMargin.Left + childMargin.Right);
                            break;
                        }
                    case HorizontalAlignment.Center:
                    default:
                        {
                            point.X += (finalSize.Width - (childMeasuredSize.Width - childMargin.Left + childMargin.Right)) * 0.5f;
                            break;
                        }
                }

                child.Arrange(new Rect(point, childMeasuredSize));

                // 子が確定した幅の分だけ次の子の座標をずらします。
                point.Y += child.ActualHeight + childMargin.Bottom;
            }

            return finalSize;
        }
    }
}
