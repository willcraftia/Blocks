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
            // デフォルトでは Hit Test を無効にします。
            //HitTestEnabled = false;
            HitTestEnabled = true;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var size = new Size();

            var textSize = MeasureText();

            if (float.IsNaN(Width))
            {
                if (string.IsNullOrEmpty(Text))
                {
                    size.Width = CalculateWidth(availableSize.Width);
                }
                else
                {
                    // Text が設定されているならば文字列の幅で希望します。
                    size.Width = textSize.X;
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
                    size.Height = CalculateHeight(availableSize.Height);
                }
                else
                {
                    // Text が設定されているならば文字列の高さで希望します。
                    size.Height = textSize.Y;
                }
            }
            else
            {
                // 高さが設定されているならばそのまま希望します。
                size.Height = Height;
            }

            // 子は持たないことを前提として測定を終えます。
            return size;
        }

        /// <summary>
        /// Text プロパティの文字列のサイズを測定します。
        /// </summary>
        /// <returns>Text プロパティの文字列のサイズ。</returns>
        Vector2 MeasureText()
        {
            var fontSize = new Vector2();
            if (!string.IsNullOrEmpty(Text))
            {
                var font = Font ?? Screen.Font;
                fontSize = font.MeasureString(Text) * FontStretch;
            }
            return fontSize;
        }
    }
}
