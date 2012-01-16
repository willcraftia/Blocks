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
        public event KeyDelegate KeyPressed;

        /// <summary>
        /// キーの押下が解放された時に発生します。
        /// </summary>
        public event KeyDelegate KeyReleased;

        /// <summary>
        /// 文字が入力された時に発生します。
        /// </summary>
        public event CharacterDelegate CharacterEntered;

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

            if (KeyPressed == null && KeyReleased == null && CharacterEntered == null) return;

            foreach (var key in allKeys)
            {
                var previousState = previouseKeyboard[key];
                var currentState = previouseKeyboard[key];
                if (previousState == currentState) continue;

                if (currentState == KeyState.Down)
                {
                    RaiseKeyPressed(key);
                    RaiseCharacterEvent(key);
                }
                else
                {
                    RaiseKeyReleased(key);
                }
            }
        }

        /// <summary>
        /// KeyPressed イベントを発生させます。
        /// </summary>
        /// <param name="key">キー。</param>
        void RaiseKeyPressed(Keys key)
        {
            if (KeyPressed != null) KeyPressed(key);
        }

        /// <summary>
        /// KeyReleased イベントを発生させます。
        /// </summary>
        /// <param name="key">キー。</param>
        void RaiseKeyReleased(Keys key)
        {
            if (KeyReleased != null) KeyReleased(key);
        }

        /// <summary>
        /// CharacterEntered イベントを発生させます。
        /// </summary>
        /// <param name="key">キー。</param>
        void RaiseCharacterEvent(Keys key)
        {
            var shiftPressed = (currentKeyboard.IsKeyDown(Keys.LeftShift) || currentKeyboard.IsKeyDown(Keys.RightShift));

            char character;
            if (KeyboardHelper.KeyToCharacter(key, shiftPressed, out character))
            {
                if (CharacterEntered != null) CharacterEntered(character);
            }
        }
    }
}
