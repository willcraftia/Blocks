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
        /// 表示文字列の輪郭線の太さを取得または設定します。
        /// 0 以下の値を設定した場合、輪郭線は描画されません。
        /// デフォルトは 0 です。
        /// </summary>
        public float TextOutlineWidth { get; set; }

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
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var controlSize = new Size(Width, Height);
            if (float.IsNaN(controlSize.Width) || float.IsNaN(controlSize.Height))
            {
                var textSize = MeasureTextSize();

                if (float.IsNaN(controlSize.Width))
                    controlSize.Width = ClampWidth(textSize.X + Padding.Left + Padding.Right);

                if (float.IsNaN(controlSize.Height))
                    controlSize.Height = ClampHeight(textSize.Y + Padding.Top + Padding.Bottom);
            }

            return new Size
            {
                Width = controlSize.Width + Margin.Left + Margin.Right,
                Height = controlSize.Height + Margin.Top + Margin.Bottom
            };
        }

        /// <summary>
        /// Text プロパティの文字列のサイズを測定します。
        /// Text プロパティが空の場合は "X" のサイズを測定して返します。
        /// Font プロパティあるいは Screen.Font プロパティが null の場合は Vector2.Zero を返します。
        /// </summary>
        /// <returns>Text プロパティの文字列のサイズ。</returns>
        Vector2 MeasureTextSize()
        {
            var font = Font ?? Screen.Font;
            if (font == null) return Vector2.Zero;

            var sampleText = Text;
            if (string.IsNullOrEmpty(sampleText)) sampleText = "X";
            return font.MeasureString(sampleText) * FontStretch;
        }
    }
}
