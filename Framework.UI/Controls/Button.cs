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
    public class Button : ContentControl
    {
        /// <summary>
        /// Button がクリックされた時に発生します。
        /// </summary>
        public event EventHandler Click;

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
            Enabled = true;
        }

        protected override void OnMouseEnter()
        {
            pressedByMouse = (Screen.MouseDevice.MouseState.LeftButton == ButtonState.Pressed);

            base.OnMouseEnter();
        }

        protected override void OnMouseLeave()
        {
            pressedByMouse = false;

            base.OnMouseLeave();
        }

        protected override void OnMouseDown()
        {
            // 機能が無効に設定されているならば、イベントを無視します。
            if (!Enabled) return;

            pressedByMouse = Screen.MouseDevice.IsButtonPressed(MouseButtons.Left);

            base.OnMouseDown();
        }

        protected override void OnMouseUp()
        {
            // Button が押された状態で機能が無効に設定される場合を考慮し、機能が有効かどうかに関わらず処理を進めます。
            if (Screen.MouseDevice.IsButtonReleased(MouseButtons.Left))
            {
                pressedByMouse = false;
                if (Enabled && !Pressed) OnClick();
            }

            base.OnMouseUp();
        }

        protected override bool OnKeyDown()
        {
            // 機能が無効に設定されているならば、イベントを無視します。
            if (!Enabled) return false;

            if (Screen.KeyboardDevice.IsKeyPressed(Keys.Enter)) pressedByEnterKey = true;

            return base.OnKeyDown();
        }

        protected override void OnKeyUp()
        {
            if (Screen.KeyboardDevice.IsKeyReleased(Keys.Enter))
            {
                pressedByEnterKey = false;
                if (Enabled && !Pressed) OnClick();
            }

            base.OnKeyUp();
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
