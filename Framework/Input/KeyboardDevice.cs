#region Using

using System;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    /// <summary>
    /// キーボードの状態を管理するクラスです。
    /// </summary>
    public class KeyboardDevice
    {
        /// <summary>
        /// 前回の Update メソッドで得られた KeyboardState。
        /// </summary>
        KeyboardState previouseKeyboardState = new KeyboardState();

        /// <summary>
        /// Update メソッドで得られた KeyboardState。
        /// </summary>
        KeyboardState keyboardState = new KeyboardState();

        /// <summary>
        /// 前回の Update メソッドで得られた KeyboardState を取得します。
        /// </summary>
        public KeyboardState PreviousKeyboardState
        {
            get { return previouseKeyboardState; }
        }

        /// <summary>
        /// Update メソッドで得られた KeyboardState を取得します。
        /// </summary>
        public KeyboardState KeyboardState
        {
            get { return keyboardState; }
        }

        /// <summary>
        /// 任意のキーが押されたかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (任意のキーが押された場合)、false (それ以外の場合)。
        /// </value>
        public bool KeyPressed { get; private set; }

        /// <summary>
        /// 任意のキーが離されたかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (任意のキーが離された場合)、false (それ以外の場合)。
        /// </value>
        public bool KeyReleased { get; private set; }

        /// <summary>
        /// 指定されたキーが押されたかどうかを判定します。
        /// </summary>
        /// <param name="key">判定するキー。</param>
        /// <returns>
        /// true (キーが押された場合)、false (それ以外の場合)。
        /// </returns>
        public bool IsKeyPressed(Keys key)
        {
            var previousState = previouseKeyboardState[key];
            var state = keyboardState[key];
            return (previousState != state) && (state == KeyState.Down);
        }

        /// <summary>
        /// 指定されたキーが離されたかどうかを判定します。
        /// </summary>
        /// <param name="key">判定するキー。</param>
        /// <returns>
        /// true (キーが離された場合)、false (それ以外の場合)。
        /// </returns>
        public bool IsKeyReleased(Keys key)
        {
            var previousState = previouseKeyboardState[key];
            var state = keyboardState[key];
            return (previousState != state) && (state == KeyState.Up);
        }

        /// <summary>
        /// 状態を更新します。
        /// </summary>
        public void Update()
        {
            // 状態をリセットします。
            KeyPressed = false;
            KeyReleased = false;

            // KeyboardState を取得します。
            previouseKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            // キーが押されたかどうか、および、離されたかどうかを判定します。
            foreach (var key in KeyboardHelper.AllKeys)
            {
                var previousState = previouseKeyboardState[key];
                var state = keyboardState[key];

                if (previousState != state)
                {
                    if (state == KeyState.Down)
                    {
                        KeyPressed = true;
                    }
                    else
                    {
                        KeyReleased = true;
                    }
                    break;
                }
            }
        }
    }
}
