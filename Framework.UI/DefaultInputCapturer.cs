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
        MouseDevice mouseDevice;

        /// <summary>
        /// キャプチャ対象の KeyboardDevice。
        /// </summary>
        KeyboardDevice keyboardDevice;

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
            mouseDevice = inputService.MouseDevice;
            mouseDevice.MouseMove += new MouseEventHandler(OnMouseMove);
            mouseDevice.MouseDown += new MouseButtonEventHandler(OnMouseDown);
            mouseDevice.MouseUp += new MouseButtonEventHandler(OnMouseUp);
            mouseDevice.MouseWheel += new MouseWheelEventHandler(OnMouseWheel);

            keyboardDevice = inputService.KeyboardDevice;
            keyboardDevice.KeyDown += new KeyEventHandler(OnKeyDown);
            keyboardDevice.KeyUp += new KeyEventHandler(OnKeyUp);
            keyboardDevice.CharacterEnter += new CharacterInputEventHandler(OnCharacterEnter);
        }

        /// <summary>
        /// MouseDevice で MouseMove イベントが発生した時に呼び出され、
        /// イベントを InputReceiver へ通知します。
        /// </summary>
        /// <param name="sender">MouseDevice。</param>
        /// <param name="mouseDevice">MouseDevice。</param>
        void OnMouseMove(object sender, MouseDevice mouseDevice)
        {
            if (InputReceiver != null) InputReceiver.NotifyMouseMove(mouseDevice);
        }

        /// <summary>
        /// MouseDevice で MouseDown イベントが発生した時に呼び出され、
        /// イベントを InputReceiver へ通知します。
        /// </summary>
        /// <param name="sender">MouseDevice。</param>
        /// <param name="mouseDevice">MouseDevice。</param>
        /// <param name="buttons">押下されたボタン。</param>
        void OnMouseDown(object sender, MouseDevice mouseDevice, MouseButtons buttons)
        {
            if (InputReceiver != null) InputReceiver.NotifyMouseDown(mouseDevice, buttons);
        }

        /// <summary>
        /// MouseDevice で MouseUp イベントが発生した時に呼び出され、
        /// イベントを InputReceiver へ通知します。
        /// </summary>
        /// <param name="sender">MouseDevice。</param>
        /// <param name="mouseDevice">MouseDevice。</param>
        /// <param name="buttons">押下が解放されたボタン。</param>
        void OnMouseUp(object sender, MouseDevice mouseDevice, MouseButtons buttons)
        {
            if (InputReceiver != null) InputReceiver.NotifyMouseUp(mouseDevice, buttons);
        }

        /// <summary>
        /// MouseDevice で MouseWheel イベントが発生した時に呼び出され、
        /// イベントを InputReceiver へ通知します。
        /// </summary>
        /// <param name="sender">MouseDevice。</param>
        /// <param name="mouseDevice">MouseDevice。</param>
        /// <param name="delta">マウス ホイールが変化した量。</param>
        void OnMouseWheel(object sender, MouseDevice mouseDevice, int delta)
        {
            if (InputReceiver != null) InputReceiver.NotifyMouseWheel(mouseDevice, delta);
        }

        /// <summary>
        /// KeyboardDevice で KeyDown イベントが発生した時に呼び出され、
        /// イベントを InputReceiver へ通知します。
        /// </summary>
        /// <param name="sender">KeyboardDevice。</param>
        /// <param name="keyboardDevice">KeyboardDevice。</param>
        /// <param name="key">押下されているキー。</param>
        void OnKeyDown(object sender, KeyboardDevice keyboardDevice, Keys key)
        {
            if (InputReceiver != null) InputReceiver.NotifyKeyDown(keyboardDevice, key);
        }

        /// <summary>
        /// KeyboardDevice で KeyUp イベントが発生した時に呼び出され、
        /// イベントを InputReceiver へ通知します。
        /// </summary>
        /// <param name="sender">KeyboardDevice。</param>
        /// <param name="keyboardDevice">KeyboardDevice。</param>
        /// <param name="key">押下が解放されたキー。</param>
        void OnKeyUp(object sender, KeyboardDevice keyboardDevice, Keys key)
        {
            if (InputReceiver != null) InputReceiver.NotifyKeyUp(keyboardDevice, key);
        }

        /// <summary>
        /// CharacterEnter イベントのハンドラです。
        /// </summary>
        /// <param name="sender">KeyboardDevice。</param>
        /// <param name="keyboardDevice">KeyboardDevice。</param>
        /// <param name="character">入力された文字。</param>
        void OnCharacterEnter(object sender, KeyboardDevice keyboardDevice, char character)
        {
            if (InputReceiver != null) InputReceiver.NotifyCharacterEnter(keyboardDevice, character);
        }
    }
}
