#region Using

using System;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public class Button : Control
    {
        /// <summary>
        /// Button がクリックされた時に発生します。
        /// </summary>
        public event EventHandler Clicked;

        bool pressedByMouse;

        public bool MouseHovering { get; private set; }

        /// <summary>
        /// Button が押された状態にあるかどうかを取得します。
        /// </summary>
        /// <value>true (Button が押された状態にある場合)、false (それ以外の場合)。</value>
        public bool Pressed
        {
            get
            {
                return MouseHovering && pressedByMouse;
            }
        }

        public Button()
        {
            Enabled = true;
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
