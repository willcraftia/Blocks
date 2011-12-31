#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    public class Button : Control
    {
        /// <summary>
        /// Button がクリックされた時に発生します。
        /// </summary>
        public event EventHandler Clicked;

        string text;

        bool pressedByMouse;

        public string Text
        {
            get { return text; }
            set
            {
                if (text == value) return;
                text = value;
                Measured = false;
            }
        }

        public HorizontalAlignment TextHorizontalAlignment { get; set; }

        public VerticalAlignment TextVerticalAlignment { get; set; }

        public bool MouseHovering { get; private set; }

        /// <summary>
        /// Button が押された状態にあるかどうかを取得します。
        /// </summary>
        /// <value>true (Button が押された状態にある場合)、false (それ以外の場合)。</value>
        public bool Pressed
        {
            get { return MouseHovering && pressedByMouse; }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
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

            if (float.IsNaN(Width))
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
                // 幅が設定されているならばそのまま希望します。
                size.Width = Width;
            }

            if (float.IsNaN(Height))
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

        protected override void OnMouseEntered()
        {
            MouseHovering = true;
        }

        protected override void OnMouseLeft()
        {
            MouseHovering = false;
        }

        protected override void OnMouseButtonPressed(MouseButtons button)
        {
            // 機能が無効に設定されているならば、イベントを無視します。
            if (!Enabled) return;

            if (button == MouseButtons.Left) pressedByMouse = true;
        }

        protected override void OnMouseButtonReleased(MouseButtons button)
        {
            // Button が押された状態で機能が無効に設定される場合を考慮し、機能が有効かどうかに関わらず処理を進めます。

            if (button == MouseButtons.Left)
            {
                pressedByMouse = false;

                // Button の上でマウス ボタンが離されたのならば、Clicked イベントを発生させます。
                if (Enabled && !Pressed) RaiseClicked();
            }
        }

        /// <summary>
        /// Click イベントが発生する時に呼び出されます。
        /// </summary>
        protected virtual void OnClicked() { }

        /// <summary>
        /// Clicked イベントを発生させます。
        /// </summary>
        void RaiseClicked()
        {
            OnClicked();
            if (Clicked != null) Clicked(this, EventArgs.Empty);
        }
    }
}
