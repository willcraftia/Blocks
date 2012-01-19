#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    /// <summary>
    /// Button として振る舞う Control です。
    /// </summary>
    public class Button : Control
    {
        /// <summary>
        /// Button がクリックされた時に発生します。
        /// </summary>
        public event EventHandler Click;

        /// <summary>
        /// 表示文字列。
        /// </summary>
        string text;

        /// <summary>
        /// マウス ボタンが押された状態かどうかを示す値。
        /// </summary>
        /// <value>
        /// true (マウス ボタンが押された状態の場合)、false (それ以外の場合)。
        /// </value>
        bool pressedByMouse;

        /// <summary>
        /// Enter キーが押された状態かどうかを示す値。
        /// </summary>
        /// <value>
        /// true (Enter キーが押された状態の場合)、false (それ以外の場合)。
        /// </value>
        bool pressedByEnterKey;

        /// <summary>
        /// 表示文字列を取得または設定します。
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                if (text == value) return;
                text = value;
            }
        }

        /// <summary>
        /// 表示文字列の水平方向についての配置方法を取得または設定します。
        /// </summary>
        public HorizontalAlignment TextHorizontalAlignment { get; set; }

        /// <summary>
        /// 表示文字列の垂直方向についての配置方法を取得または設定します。
        /// </summary>
        public VerticalAlignment TextVerticalAlignment { get; set; }

        /// <summary>
        /// Button が押された状態にあるかどうかを取得します。
        /// </summary>
        /// <value>true (Button が押された状態にある場合)、false (それ以外の場合)。</value>
        public bool Pressed
        {
            get { return MouseDirectlyOver && (pressedByMouse || pressedByEnterKey); }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <pparam name="screen">Screen。</pparam>
        public Button(Screen screen)
            : base(screen)
        {
            TextHorizontalAlignment = HorizontalAlignment.Center;
            TextVerticalAlignment = VerticalAlignment.Center;
            Enabled = true;
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

        protected override void OnMouseEnter()
        {
            // マウス状態を直接参照してボタン押下状態を復帰させます。
            if (Mouse.GetState().LeftButton == ButtonState.Pressed) pressedByMouse = true;

            base.OnMouseEnter();
        }

        protected override void OnMouseLeave()
        {
            // マウス押下状態を解除します。
            pressedByMouse = false;

            base.OnMouseLeave();
        }

        protected override void OnMouseDown(MouseButtons button)
        {
            // 機能が無効に設定されているならば、イベントを無視します。
            if (!Enabled) return;

            if ((button & MouseButtons.Left) == MouseButtons.Left) pressedByMouse = true;

            base.OnMouseDown(button);
        }

        protected override void OnMouseUp(MouseButtons button)
        {
            // Button が押された状態で機能が無効に設定される場合を考慮し、機能が有効かどうかに関わらず処理を進めます。

            if ((button & MouseButtons.Left) == MouseButtons.Left)
            {
                pressedByMouse = false;
                if (Enabled && !Pressed) OnClick();
            }

            base.OnMouseUp(button);
        }

        protected override bool OnKeyDown(Keys key)
        {
            // 機能が無効に設定されているならば、イベントを無視します。
            if (!Enabled) return false;

            if (key == Keys.Enter) pressedByEnterKey = true;

            return base.OnKeyDown(key);
        }

        protected override void OnKeyUp(Keys key)
        {
            if (key == Keys.Enter)
            {
                pressedByEnterKey = false;
                if (Enabled && !Pressed) OnClick();
            }

            base.OnKeyUp(key);
        }

        /// <summary>
        /// ボタンがクリックされた時に呼び出されます。
        /// Click イベントを発生させます。
        /// </summary>
        protected virtual void OnClick()
        {
            if (Click != null) Click(this, EventArgs.Empty);
        }
    }
}
