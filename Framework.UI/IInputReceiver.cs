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
        /// マウス カーソルの移動を通知します。
        /// </summary>
        /// <param name="x">マウス カーソルの x 座標。</param>
        /// <param name="y">マウス カーソルの y 座標。</param>
        void NotifyMouseMove(int x, int y);

        /// <summary>
        /// マウス ボタンの押下を通知します。
        /// </summary>
        /// <param name="button">マウス ボタン。</param>
        void NotifyMouseDown(MouseButtons button);
        
        /// <summary>
        /// マウス ボタン押下の解放を通知します。
        /// </summary>
        /// <param name="button">マウス ボタン。</param>
        void NotifyMouseUp(MouseButtons button);
        
        /// <summary>
        /// マウス ホイールの回転を通知します。
        /// </summary>
        /// <param name="ticks">マウス ホイールの回転量。</param>
        void NotifyMouseWheel(int ticks);

        /// <summary>
        /// キーの押下を通知します。
        /// </summary>
        /// <param name="key">キー。</param>
        void NotifyKeyDown(Keys key);

        /// <summary>
        /// キー押下の解放を通知します。
        /// </summary>
        /// <param name="key">キー。</param>
        void NotifyKeyUp(Keys key);

        /// <summary>
        /// 文字入力を通知します。
        /// </summary>
        /// <param name="character">文字。</param>
        void NotifyCharacterEnter(char character);
    }
}
