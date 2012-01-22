#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    /// <summary>
    /// マウス ホイール関連イベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="sender">イベント ハンドラがアタッチされているオブジェクト。</param>
    /// <param name="mouseDevice">MouseDevice。</param>
    /// <param name="delta">マウス ホイールが変化した量。</param>
    public delegate void MouseWheelEventHandler(object sender, MouseDevice mouseDevice, int delta);
}
