#region Using

using System;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    /// <summary>
    /// キーボード状態の変化からイベントを発生させるクラスです。
    /// </summary>
    /// <remarks>
    /// KeyboardDevice はイベント処理のためのクラスです。
    /// キーボード状態を参照したい場合は、XNA の Keyboard クラスから KeyboardState を取得して参照します。
    /// </remarks>
    public class KeyboardDevice
    {
        /// <summary>
        /// キーが押下された時に発生します。
        /// </summary>
        public event KeyEventHandler KeyDown;

        /// <summary>
        /// キーの押下が解放された時に発生します。
        /// </summary>
        public event KeyEventHandler KeyUp;

        /// <summary>
        /// 文字が入力された時に発生します。
        /// </summary>
        public event CharacterInputEventHandler CharacterEnter;

        /// <summary>
        /// Keys で定義された列挙値の配列。
        /// </summary>
        static readonly Keys[] allKeys = (Keys[]) Enum.GetValues(typeof(Keys));

        /// <summary>
        /// 前回の Update メソッドにおける KeyboardState。
        /// </summary>
        KeyboardState previouseKeyboard = new KeyboardState();

        /// <summary>
        /// Update メソッドで得る KeyboardState。
        /// </summary>
        KeyboardState currentKeyboard = new KeyboardState();

        /// <summary>
        /// デバイスの状態を更新します。
        /// </summary>
        internal void Update()
        {
            previouseKeyboard = currentKeyboard;
            currentKeyboard = Keyboard.GetState();

            if (KeyDown == null && KeyUp == null && CharacterEnter == null) return;

            foreach (var key in allKeys)
            {
                var previousState = previouseKeyboard[key];
                var currentState = currentKeyboard[key];
                if (previousState == currentState) continue;

                if (currentState == KeyState.Down)
                {
                    OnKeyDown(key);
                    OnCharacterEnter(key);
                }
                else
                {
                    OnKeyUp(key);
                }
            }
        }

        /// <summary>
        /// キーが押下された時に呼び出されます。
        /// KeyDown イベントを発生させます。
        /// </summary>
        /// <param name="key">押下されているキー。</param>
        protected void OnKeyDown(Keys key)
        {
            if (KeyDown != null) KeyDown(this, this, key);
        }

        /// <summary>
        /// キー押下が解放された時に呼び出されます。
        /// KeyUp イベントを発生させます。
        /// </summary>
        /// <param name="key">押下が解放されたキー。</param>
        protected void OnKeyUp(Keys key)
        {
            if (KeyUp != null) KeyUp(this, this, key);
        }

        /// <summary>
        /// CharacterEnter イベントを発生させます。
        /// </summary>
        /// <param name="key">押下されているキー。</param>
        protected void OnCharacterEnter(Keys key)
        {
            var shiftPressed = (currentKeyboard.IsKeyDown(Keys.LeftShift) || currentKeyboard.IsKeyDown(Keys.RightShift));

            char character;
            if (KeyboardHelper.KeyToCharacter(key, shiftPressed, out character))
            {
                if (CharacterEnter != null) CharacterEnter(this, this, character);
            }
        }
    }
}
