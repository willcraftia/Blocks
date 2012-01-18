#region Using

using System;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    /// <summary>
    /// キー押下と解放の通知に使用する delegate。
    /// </summary>
    /// <param name="key">キー。</param>
    public delegate void KeyDelegate(Keys key);

    /// <summary>
    /// 文字入力の通知に使用する delegate。
    /// </summary>
    /// <param name="character">文字。</param>
    public delegate void CharacterDelegate(char character);

    /// <summary>
    /// IKeyboard のデフォルト実装クラスです。
    /// </summary>
    public class KeyboardDevice : IInputDevice
    {
        /// <summary>
        /// キーが押下された時に発生します。
        /// </summary>
        public event KeyDelegate KeyDown;

        /// <summary>
        /// キーの押下が解放された時に発生します。
        /// </summary>
        public event KeyDelegate KeyUp;

        /// <summary>
        /// 文字が入力された時に発生します。
        /// </summary>
        public event CharacterDelegate CharacterEnter;

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
        
        // I/F
        public bool Enabled
        {
            get { return true; }
        }

        // I/F
        public string Name
        {
            get { return "Main Keyboard"; }
        }

        // I/F
        public void Update()
        {
            previouseKeyboard = currentKeyboard;
            currentKeyboard = Keyboard.GetState();

            if (KeyDown == null && KeyUp == null && CharacterEnter == null) return;

            foreach (var key in allKeys)
            {
                var previousState = previouseKeyboard[key];
                var currentState = previouseKeyboard[key];
                if (previousState == currentState) continue;

                if (currentState == KeyState.Down)
                {
                    RaiseKeyDown(key);
                    RaiseCharacterEnter(key);
                }
                else
                {
                    RaiseKeyUp(key);
                }
            }
        }

        /// <summary>
        /// KeyDown イベントを発生させます。
        /// </summary>
        /// <param name="key">キー。</param>
        void RaiseKeyDown(Keys key)
        {
            if (KeyDown != null) KeyDown(key);
        }

        /// <summary>
        /// KeyUp イベントを発生させます。
        /// </summary>
        /// <param name="key">キー。</param>
        void RaiseKeyUp(Keys key)
        {
            if (KeyUp != null) KeyUp(key);
        }

        /// <summary>
        /// CharacterEnter イベントを発生させます。
        /// </summary>
        /// <param name="key">キー。</param>
        void RaiseCharacterEnter(Keys key)
        {
            var shiftPressed = (currentKeyboard.IsKeyDown(Keys.LeftShift) || currentKeyboard.IsKeyDown(Keys.RightShift));

            char character;
            if (KeyboardHelper.KeyToCharacter(key, shiftPressed, out character))
            {
                if (CharacterEnter != null) CharacterEnter(character);
            }
        }
    }
}
