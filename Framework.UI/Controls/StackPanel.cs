#region Using

using System;

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
            Orientation = Orientation.Horizontal;
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
                point.Y = childMargin.Top;
                switch (child.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        {
                            break;
                        }
                    case VerticalAlignment.Bottom:
                        {
                            point.Y += finalSize.Height - childMeasuredSize.Height;
                            break;
                        }
                    case UI.VerticalAlignment.Center:
                    default:
                        {
                            point.Y += (finalSize.Height - childMeasuredSize.Height) * 0.5f;
                            break;
                        }
                }

                child.Arrange(new Rect(point, childMeasuredSize));

                // 子が確定した幅の分だけ次の子の座標をずらします。
                point.X += child.ActualWidth;
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
                switch (child.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        {
                            break;
                        }
                    case HorizontalAlignment.Right:
                        {
                            point.X += finalSize.Width - childMeasuredSize.Width;
                            break;
                        }
                    case HorizontalAlignment.Center:
                    default:
                        {
                            point.X += (finalSize.Width - childMeasuredSize.Width) * 0.5f;
                            break;
                        }
                }

                child.Arrange(new Rect(point, childMeasuredSize));

                // 子が確定した幅の分だけ次の子の座標をずらします。
                point.Y += child.ActualHeight;
            }

            return finalSize;
        }
    }
}
