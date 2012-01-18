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
        /// 指定の IInputService が提供する IInputDevice をキャプチャするインスタンスを生成します。
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
            mouse.MouseMove += new MouseMoveDelegate(OnMouseMove);
            mouse.MouseDown += new MouseButtonDelegate(OnMouseDown);
            mouse.MouseUp += new MouseButtonDelegate(OnMouseUp);
            mouse.MouseWheel += new MouseWheelDelegate(OnMouseWheel);

            keyboard = inputService.Keyboard;
            keyboard.KeyDown += new KeyDelegate(OnKeyDown);
            keyboard.KeyUp += new KeyDelegate(OnKeyUp);
            keyboard.CharacterEnter += new CharacterDelegate(OnCharacterEnter);
        }

        /// <summary>
        /// MouseMove イベントのハンドラです。
        /// </summary>
        /// <param name="x">マウス カーソルの x 座標。</param>
        /// <param name="y">マウス カーソルの y 座標。</param>
        void OnMouseMove(int x, int y)
        {
            if (InputReceiver != null) InputReceiver.NotifyMouseMove(x, y);
        }

        /// <summary>
        /// MouseDown イベントのハンドラです。
        /// </summary>
        /// <param name="buttons">マウス ボタン。</param>
        void OnMouseDown(MouseButtons buttons)
        {
            if (InputReceiver != null) InputReceiver.NotifyMouseDown(buttons);
        }

        /// <summary>
        /// MouseUp イベントのハンドラです。
        /// </summary>
        /// <param name="buttons">マウス ボタン。</param>
        void OnMouseUp(MouseButtons buttons)
        {
            if (InputReceiver != null) InputReceiver.NotifyMouseUp(buttons);
        }

        /// <summary>
        /// MouseWheel イベントのハンドラです。
        /// </summary>
        /// <param name="ticks">マウス ホイールの回転量。</param>
        void OnMouseWheel(int ticks)
        {
            if (InputReceiver != null) InputReceiver.NotifyMouseWheel(ticks);
        }

        /// <summary>
        /// KeyDown イベントのハンドラです。
        /// </summary>
        /// <param name="key">キー。</param>
        void OnKeyDown(Keys key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// KeyUp イベントのハンドラです。
        /// </summary>
        /// <param name="key">キー。</param>
        void OnKeyUp(Keys key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// CharacterEnter イベントのハンドラです。
        /// </summary>
        /// <param name="character">文字。</param>
        void OnCharacterEnter(char character)
        {
            throw new NotImplementedException();
        }
    }
}
