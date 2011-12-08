#region Using

using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    /// <summary>
    /// マウス カーソルの移動の通知に利用する delegate。
    /// </summary>
    /// <param name="x">マウス カーソルの x 座標。</param>
    /// <param name="y">マウス カーソルの y 座標。</param>
    public delegate void MouseMoveDelegate(int x, int y);

    /// <summary>
    /// マウス ボタンの押下と開放の通知に利用する delegate。
    /// </summary>
    /// <param name="buttons">押下あるいは開放されたマウスのボタン (複数ボタンあり)。</param>
    public delegate void MouseButtonDelegate(MouseButtons buttons);

    /// <summary>
    /// マウス ホイールの回転の通知に利用する delegate。
    /// </summary>
    /// <param name="ticks">マウス ホイールの回転量。</param>
    public delegate void MouseWheelDelegate(int ticks);

    /// <summary>
    /// マウスを表す入力デバイスの拡張です。
    /// </summary>
    public interface IMouse : IInputDevice
    {
        /// <summary>
        /// マウス カーソルが移動した場合に発生します。
        /// </summary>
        event MouseMoveDelegate MouseMoved;

        /// <summary>
        /// マウス ボタンが押下された場合に発生します。
        /// </summary>
        event MouseButtonDelegate MouseButtonPressed;

        /// <summary>
        /// マウス ボタンの押下が開放された場合に発生します。
        /// </summary>
        event MouseButtonDelegate MouseButtonReleased;

        /// <summary>
        /// マウス ホイールが回転された場合に発生します。
        /// </summary>
        event MouseWheelDelegate MouseWheelRotated;

        /// <summary>
        /// マウスの現在状態を取得します。
        /// </summary>
        /// <returns></returns>
        MouseState GetState();

        /// <summary>
        /// マウス カーソルを指定の座標へ移動させます。
        /// </summary>
        /// <param name="x">移動先の x 座標。</param>
        /// <param name="y">移動先の y 座標。</param>
        void MoveTo(int x, int y);
    }
}
