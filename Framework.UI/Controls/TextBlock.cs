#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    public class TextBlock : Control
    {
        /// <summary>
        /// 表示文字列を取得または設定します。
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 表示文字列の水平方向についての配置方法を取得または設定します。
        /// </summary>
        public HorizontalAlignment TextHorizontalAlignment { get; set; }

        /// <summary>
        /// 表示文字列の垂直方向についての配置方法を取得または設定します。
        /// </summary>
        public VerticalAlignment TextVerticalAlignment { get; set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <pparam name="screen">Screen。</pparam>
        public TextBlock(Screen screen)
            : base(screen)
        {
            TextHorizontalAlignment = HorizontalAlignment.Center;
            TextVerticalAlignment = VerticalAlignment.Center;
            Enabled = true;
            // デフォルトでは文字描画に専念するものとしてフォーカスを設定させません。
            Focusable = false;
            // デフォルトでは Hit Test を無効にします。
            HitTestEnabled = false;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var size = new Size();

            Vector2 fontSize = new Vector2();
            if (!string.IsNullOrEmpty(Text))
            {
                var font = Font ?? Screen.Font;
                fontSize = font.MeasureString(Text) * FontStretch;
            }

            if (float.IsNaN(Width))
            {
                if (string.IsNullOrEmpty(Text))
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
                    var margin = Margin;
                    size.Width = MathHelper.Max(MinWidth, size.Width - margin.Left - margin.Right);
                }
                else
                {
                    // Text が設定されているならば、そのフォントの幅と Padding で希望します。
                    var padding = Padding;
                    size.Width = fontSize.X + padding.Left + padding.Right;
                }
            }
            else
            {
                // 幅が設定されているならばそのまま希望します。
                size.Width = Width;
            }

            if (float.IsNaN(Height))
            {
                if (string.IsNullOrEmpty(Text))
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
                    var margin = Margin;
                    size.Height = MathHelper.Max(MinHeight, size.Height - margin.Top - margin.Bottom);
                }
                else
                {
                    // Text が設定されているならば、そのフォントの高さと Padding で希望します。
                    var padding = Padding;
                    size.Height = fontSize.Y + padding.Top + padding.Bottom;
                }
            }
            else
            {
                // 高さが設定されているならばそのまま希望します。
                size.Height = Height;
            }

            // 自分が希望するサイズで子の希望サイズを定めます。
            foreach (var child in Children)
            {
                // 自身の希望サイズを測定したので、子が測定済かどうかによらず再測定します。
                child.Measure(size);
            }

            return size;
        }
    }
}
