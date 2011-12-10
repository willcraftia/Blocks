#region Using

using System;
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
        /// キャプチャ対象の IMouse。
        /// </summary>
        IMouse mouse;

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
        }

        /// <summary>
        /// MouseMoved イベントのハンドラです。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void OnMouseMoved(int x, int y)
        {
            if (InputReceiver != null) InputReceiver.NotifyMouseMoved(x, y);
        }

        /// <summary>
        /// MouseButtonPressed イベントのハンドラです。
        /// </summary>
        /// <param name="buttons"></param>
        void OnMouseButtonPressed(MouseButtons buttons)
        {
            if (InputReceiver != null) InputReceiver.NotifyMouseButtonPressed(buttons);
        }

        /// <summary>
        /// MouseButtonReleased イベントのハンドラです。
        /// </summary>
        /// <param name="buttons"></param>
        void OnMouseButtonReleased(MouseButtons buttons)
        {
            if (InputReceiver != null) InputReceiver.NotifyMouseButtonReleased(buttons);
        }

        /// <summary>
        /// MouseWheelRotated イベントのハンドラです。
        /// </summary>
        /// <param name="ticks"></param>
        void OnMouseWheelRotated(int ticks)
        {
            if (InputReceiver != null) InputReceiver.NotifyMouseWheelRotated(ticks);
        }
    }
}
