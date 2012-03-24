#region Using

using System;
using System.Text;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.Text;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    /// <summary>
    /// 文字列を表示するための Control です。
    /// </summary>
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
        /// 表示文字列がコンテナの端に達する時の折り返し方法を取得または設定します。
        /// </summary>
        public TextWrapping TextWrapping { get; set; }

        /// <summary>
        /// WrappedText を取得します。
        /// このプロパティは、TextWrapping プロパティに Wrap を指定した場合に、
        /// 最初の Measure() の呼び出しで有効になります。
        /// 原則として、内部処理および描画処理のためのプロパティであり、
        /// このプロパティを直接用いて折り返しを制御しようとしてはなりません。
        /// </summary>
        public WrappedText WrappedText { get; private set; }

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

            if (TextWrapping == TextWrapping.Wrap)
            {
                float availableWidth = Width - (Padding.Left + Padding.Right);
                if (float.IsNaN(availableWidth))
                    availableWidth = availableSize.Width - (Padding.Left + Padding.Right);
                
                var wrappedSize = MeasureWrappedTextSize(availableWidth);

                if (float.IsNaN(controlSize.Width))
                    controlSize.Width = ClampWidth(wrappedSize.X + Padding.Left + Padding.Right);
                if (float.IsNaN(controlSize.Height))
                    controlSize.Height = ClampHeight(wrappedSize.Y + Padding.Top + Padding.Bottom);
            }
            else
            {
                if (float.IsNaN(controlSize.Width) || float.IsNaN(controlSize.Height))
                {
                    var textSize = MeasureTextSize();

                    if (float.IsNaN(controlSize.Width))
                        controlSize.Width = ClampWidth(textSize.X + Padding.Left + Padding.Right);

                    if (float.IsNaN(controlSize.Height))
                        controlSize.Height = ClampHeight(textSize.Y + Padding.Top + Padding.Bottom);
                }
            }

            return new Size
            {
                Width = controlSize.Width + Margin.Left + Margin.Right,
                Height = controlSize.Height + Margin.Top + Margin.Bottom
            };
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            // TODO: 必要？
            if (TextWrapping == TextWrapping.Wrap && arrangeSize.Width < MeasuredSize.Width)
            {
                var wrappedSize = MeasureWrappedTextSize(arrangeSize.Width);
                var size = new Size
                {
                    Width = wrappedSize.X,
                    Height = wrappedSize.Y
                };
                return size;
            }

            return base.ArrangeOverride(arrangeSize);
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

        /// <summary>
        /// TextWrapping プロパティが Wrap に設定されている場合での文字列サイズを測定します。
        /// また同時に、WrappedText プロパティを利用可能な状態へと設定します。
        /// </summary>
        /// <param name="availableWidth">利用可能な幅。</param>
        /// <returns>折り返しを考慮した文字列のサイズ。</returns>
        Vector2 MeasureWrappedTextSize(float availableWidth)
        {
            if (WrappedText == null) WrappedText = new WrappedText();
            WrappedText.Text = Text;
            WrappedText.Font = Font ?? Screen.Font;
            WrappedText.FontStretch = FontStretch;
            WrappedText.ClientWidth = availableWidth;

            WrappedText.Wrap();

            return WrappedText.MeasuredSize;
        }
    }
}
