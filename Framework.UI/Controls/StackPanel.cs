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
            if (Orientation == Orientation.Horizontal)
            {
                ArrangeHorizontalDirection();
            }
            else
            {
                ArrangeVerticalDirection();
            }

            base.Arrange();
        }

        protected virtual void ArrangeHorizontalDirection()
        {
            // 親から描画時サイズが設定されていないならば、まだ処理を行いません。
            if (!Arranged) return;

            // シンプルに実装します。
            float right = 0;
            foreach (var child in Children)
            {
                // 幅は指定された値をそのまま使うことにします。
                child.ActualWidth = child.ClampedWidth;
                // 左マージンを調整します。
                var childMargin = child.Margin;
                child.Margin = new Thickness(right, childMargin.Top, childMargin.Right, childMargin.Bottom);
                
                // 高さは描画領域に収まるように調整します。
                var childMarginHeight = childMargin.Top + childMargin.Bottom;
                var childHeight = child.ClampedHeight;
                if (ActualHeight < childHeight + childMarginHeight)
                {
                    // 子に高さが設定されていて自分の幅を越えるようならば、自分の高さに収まる最大サイズで調整を試みます。
                    child.ActualHeight = ActualHeight - childMarginHeight;
                }
                else
                {
                    // それ以外は子に設定された高さをそのまま設定するように試みます。
                    child.ActualHeight = childHeight;
                }

                right += childMargin.Left + childMargin.Right + child.ActualWidth;
            }
        }

        protected virtual void ArrangeVerticalDirection()
        {
        }
    }
}
