#region Using

using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    public class TextBox : Control
    {
        static readonly string textMeasuringHint = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        StringBuilder stringBuilder = new StringBuilder();

        public string Text
        {
            get { return stringBuilder.ToString(); }
            set
            {
                stringBuilder.Clear();
                if (value != null) stringBuilder.Append(value);
            }
        }

        public TextBox(Screen screen)
            : base(screen)
        {
            Enabled = true;
            Focusable = true;
        }

        protected override void OnKeyDown(ref RoutedEventContext context)
        {
            var keyboardDevice = Screen.KeyboardDevice;
            var keyboardState = keyboardDevice.KeyboardState;

            // 先に Shift キーの押下を判定します。
            bool shiftPressed = keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift);

            bool handled = false;
            foreach (var key in keyboardState.GetPressedKeys())
            {
                // Shift キーは除外します。
                if (key == Keys.LeftShift || key == Keys.RightShift) continue;

                if (key == Keys.Back)
                {
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                    handled = true;
                    continue;
                }

                // 押されたキーに対応する文字を取得します。
                char c;
                if (KeyboardHelper.KeyToCharacter(key, shiftPressed, out c))
                {
                    stringBuilder.Append(c);
                    handled = true;
                }
            }

            // イベントのバブル アップを停止させます。
            if (handled) context.Handled = true;

            base.OnKeyDown(ref context);
        }

        public override void Draw(GameTime gameTime, IDrawContext drawContext)
        {
            base.Draw(gameTime, drawContext);

            if (stringBuilder.Length == 0) return;

            var font = Font ?? Screen.Font;
            if (font == null) return;

            // TODO: 面倒なのでキャレットをそのまま文字連結していますが、本当はダメ。
            drawContext.DrawString(
                new Rect(RenderSize), font, stringBuilder.ToString() + "|", FontStretch,
                HorizontalAlignment.Left, VerticalAlignment.Top,
                ForegroundColor, Padding);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var size = new Size();

            var textSize = MeasureText();

            if (float.IsNaN(Width))
            {
                size.Width = CalculateWidth(availableSize.Width);
            }
            else
            {
                // 幅が設定されているならばそのまま希望します。
                size.Width = Width;
            }

            if (float.IsNaN(Height))
            {
                // 測定した文字の最大サイズに従います。
                size.Height = textSize.Y;
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
        /// 文字列のサイズを測定します。
        /// </summary>
        /// <returns>文字列のサイズ。</returns>
        Vector2 MeasureText()
        {
            var font = Font ?? Screen.Font;
            return font.MeasureString(textMeasuringHint) * FontStretch;
        }
    }
}
