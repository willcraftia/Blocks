#region Using

using System;
using System.Collections.Generic;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    public class TabControl : Selector
    {
        protected override int ChildrenCount
        {
            get { return (SelectedIndex == -1 || Items.Count == 0) ? 0 : 1; }
        }

        public TabControl(Screen screen)
            : base(screen)
        {
        }

        protected override Control GetChild(int index)
        {
            if (index != 0 || SelectedIndex == -1 || Items.Count == 0)
                throw new ArgumentOutOfRangeException("index");

            return SelectedItem;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            // 暫定的に Control のサイズを計算します。
            var controlSize = new Size(Width, Height);
            if (float.IsNaN(controlSize.Width))
                controlSize.Width = CalculateWidth(availableSize.Width - Margin.Left - Margin.Right);
            if (float.IsNaN(controlSize.Height))
                controlSize.Height = CalculateWidth(availableSize.Height - Margin.Top - Margin.Bottom);

            // 要素の利用できるサイズは、内側の余白をとった領域のサイズです。
            var widthPadding = Padding.Left + Padding.Right;
            var heightPadding = Padding.Top + Padding.Bottom;
            var itemAvailableSize = new Size
            {
                Width = controlSize.Width - widthPadding,
                Height = controlSize.Height - heightPadding
            };

            // 要素の希望サイズを定めます。
            float maxItemMeasuredWidth = 0;
            float maxItemMeasuredHeight = 0;
            foreach (var item in Items)
            {
                item.Measure(itemAvailableSize);

                var itemMeasuredSize = item.MeasuredSize;
                maxItemMeasuredWidth = Math.Max(maxItemMeasuredWidth, itemMeasuredSize.Width);
                maxItemMeasuredHeight = Math.Max(maxItemMeasuredHeight, itemMeasuredSize.Height);
            }

            // 幅や高さが未設定ならば要素のうちの最大の値を Control のサイズにします。
            if (float.IsNaN(Width)) controlSize.Width = ClampWidth(maxItemMeasuredWidth + widthPadding);
            if (float.IsNaN(Height)) controlSize.Height = ClampHeight(maxItemMeasuredHeight + heightPadding);

            // 外側の余白を含めて描画に必要な希望サイズとします。
            return new Size
            {
                Width = controlSize.Width + Margin.Left + Margin.Right,
                Height = controlSize.Height + Margin.Top + Margin.Bottom
            };
        }
    }
}
