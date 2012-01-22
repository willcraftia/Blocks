#region Using

using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// 入力を受信するクラスのインタフェースです。
    /// </summary>
    public interface IInputReceiver
    {
        /// <summary>
        /// MouseMove イベントを通知します。
        /// </summary>
        /// <param name="mouseDevice">MouseDevice。</param>
        void NotifyMouseMove(MouseDevice mouseDevice);

        /// <summary>
        /// MouseDown イベントを通知します。
        /// </summary>
        /// <param name="mouseDevice">MouseDevice。</param>
        /// <param name="buttons">押下されたボタン。</param>
        void NotifyMouseDown(MouseDevice mouseDevice, MouseButtons buttons);

        /// <summary>
        /// MouseUp イベントを通知します。
        /// </summary>
        /// <param name="mouseDevice">MouseDevice。</param>
        /// <param name="buttons">押下が解放されたボタン。</param>
        void NotifyMouseUp(MouseDevice mouseDevice, MouseButtons buttons);

        /// <summary>
        /// MouseWheel イベントを通知します。
        /// </summary>
        /// <param name="mouseDevice">MouseDevice。</param>
        /// <param name="delta">マウス ホイールが変化した量。</param>
        void NotifyMouseWheel(MouseDevice mouseDevice, int delta);

        /// <summary>
        /// キーの押下を通知します。
        /// </summary>
        /// <param name="keyboardDevice">KeyboardDevice。</param>
        /// <param name="key">押下されているキー。</param>
        void NotifyKeyDown(KeyboardDevice keyboardDevice, Keys key);

        /// <summary>
        /// キー押下の解放を通知します。
        /// </summary>
        /// <param name="keyboardDevice">KeyboardDevice。</param>
        /// <param name="key">押下が解放されたキー。</param>
        void NotifyKeyUp(KeyboardDevice keyboardDevice, Keys key);

        /// <summary>
        /// 文字入力を通知します。
        /// </summary>
        /// <param name="keyboardDevice">KeyboardDevice。</param>
        /// <param name="character">入力された文字。</param>
        void NotifyCharacterEnter(KeyboardDevice keyboardDevice, char character);
    }
}
