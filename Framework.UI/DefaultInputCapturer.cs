#region Using

using System;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// IInputCapturer のデフォルト実装です。
    /// </summary>
    public class DefaultInputCapturer : IInputCapturer
    {
        /// <summary>
        /// IInputService。
        /// </summary>
        IInputService inputService;

        /// <summary>
        /// キャプチャ対象の MouseDevice。
        /// </summary>
        MouseDevice mouse;

        /// <summary>
        /// キャプチャ対象の KeyboardDevice。
        /// </summary>
        KeyboardDevice keyboard;

        // I/F
        public IInputReceiver InputReceiver { get; set; }

        /// <summary>
        /// 指定の IInputService が提供する IInputDevice の状態をキャプチャするインスタンスを生成します。
        /// </summary>
        /// <param name="inputService">IInputService。</param>
        public DefaultInputCapturer(IInputService inputService)
        {
            if (inputService == null) throw new ArgumentNullException("inputService");

            this.inputService = inputService;

            BindInputDevices();
        }

        /// <summary>
        /// IInputDevice をキャプチャ対象としてバインドします。
        /// </summary>
        void BindInputDevices()
        {
            mouse = inputService.Mouse;
            mouse.MouseMoved += new MouseMoveDelegate(OnMouseMoved);
            mouse.MouseButtonPressed += new MouseButtonDelegate(OnMouseButtonPressed);
            mouse.MouseButtonReleased += new MouseButtonDelegate(OnMouseButtonReleased);
            mouse.MouseWheelRotated += new MouseWheelDelegate(OnMouseWheelRotated);

            keyboard = inputService.Keyboard;
            keyboard.KeyPressed += new KeyDelegate(OnKeyPressed);
            keyboard.KeyReleased += new KeyDelegate(OnKeyReleased);
            keyboard.CharacterEntered += new CharacterDelegate(OnCharacterEntered);
        }

        /// <summary>
        /// MouseMoved イベントのハンドラです。
        /// </summary>
        /// <param name="x">マウス カーソルの x 座標。</param>
        /// <param name="y">マウス カーソルの y 座標。</param>
        void OnMouseMoved(int x, int y)
        {
            if (InputReceiver != null) InputReceiver.NotifyMouseMoved(x, y);
        }

        /// <summary>
        /// MouseButtonPressed イベントのハンドラです。
        /// </summary>
        /// <param name="buttons">マウス ボタン。</param>
        void OnMouseButtonPressed(MouseButtons buttons)
        {
            if (InputReceiver != null) InputReceiver.NotifyMouseButtonPressed(buttons);
        }

        /// <summary>
        /// MouseButtonReleased イベントのハンドラです。
        /// </summary>
        /// <param name="buttons">マウス ボタン。</param>
        void OnMouseButtonReleased(MouseButtons buttons)
        {
            if (InputReceiver != null) InputReceiver.NotifyMouseButtonReleased(buttons);
        }

        /// <summary>
        /// MouseWheelRotated イベントのハンドラです。
        /// </summary>
        /// <param name="ticks">マウス ホイールの回転量。</param>
        void OnMouseWheelRotated(int ticks)
        {
            if (InputReceiver != null) InputReceiver.NotifyMouseWheelRotated(ticks);
        }

        /// <summary>
        /// KeyPressed イベントのハンドラです。
        /// </summary>
        /// <param name="key">キー。</param>
        void OnKeyPressed(Keys key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// KeyReleased イベントのハンドラです。
        /// </summary>
        /// <param name="key">キー。</param>
        void OnKeyReleased(Keys key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// CharacterEntered イベントのハンドラです。
        /// </summary>
        /// <param name="character">文字。</param>
        void OnCharacterEntered(char character)
        {
            throw new NotImplementedException();
        }
    }
}
