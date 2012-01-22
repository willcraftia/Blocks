#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    /// <summary>
    /// マウス ボタン関連イベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="sender">イベント ハンドラがアタッチされているオブジェクト。</param>
    /// <param name="mouseDevice">MouseDevice。</param>
    /// <param name="buttons">イベントに関連付けられているボタン。</param>
    public delegate void MouseButtonEventHandler(object sender, MouseDevice mouseDevice, MouseButtons buttons);
}
