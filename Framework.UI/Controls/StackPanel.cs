#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    public class StackPanel : Control
    {
        public Orientation Orientation { get; set; }

        public StackPanel()
        {
            Orientation = Orientation.Horizontal;
        }

        protected internal override void Arrange()
        {
            if (Arranged) return;

            if (Orientation == Orientation.Horizontal)
            {
                ArrangeHorizontalDirection();
            }
            else
            {
                ArrangeVerticalDirection();
            }

            Arranged = true;
        }

        protected virtual void ArrangeHorizontalDirection()
        {
            // シンプルに実装します。
            float left = 0;
            foreach (var child in Children)
            {
                // 幅も高さも指定された値をそのまま使うことにします。
                child.ActualWidth = child.ClampedWidth;
                child.ActualHeight = child.ClampedHeight;

                // マージンを調整します。
                var childMargin = child.Margin;
                float top = 0.0f;
                switch (child.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        {
                            top = childMargin.Top;
                            break;
                        }
                    case VerticalAlignment.Bottom:
                        {
                            top = ActualHeight - childMargin.Bottom - childMargin.Top - child.ActualHeight;
                            break;
                        }
                    case VerticalAlignment.Center:
                    default:
                        {
                            top = (ActualHeight - child.ActualHeight) * 0.5f + childMargin.Top;
                            break;
                        }
                }

                child.Margin = new Thickness(left, top, childMargin.Right, childMargin.Bottom);
                
                left += childMargin.Left + childMargin.Right + child.ActualWidth;
            }
        }

        protected virtual void ArrangeVerticalDirection()
        {
            // シンプルに実装します。
            float top = 0;
            foreach (var child in Children)
            {
                // 幅も高さも指定された値をそのまま使うことにします。
                child.ActualWidth = child.ClampedWidth;
                child.ActualHeight = child.ClampedHeight;

                // マージンを調整します。
                var childMargin = child.Margin;
                float left = 0.0f;
                switch (child.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        {
                            left = childMargin.Left;
                            break;
                        }
                    case HorizontalAlignment.Right:
                        {
                            left = ActualWidth - childMargin.Right - childMargin.Left - child.ActualWidth;
                            break;
                        }
                    case HorizontalAlignment.Center:
                    default:
                        {
                            left = (ActualWidth - child.ActualWidth) * 0.5f + childMargin.Left;
                            break;
                        }
                }

                child.Margin = new Thickness(left, top, childMargin.Right, childMargin.Bottom);

                top += childMargin.Top + childMargin.Bottom + child.ActualHeight;
            }
        }
    }
}
