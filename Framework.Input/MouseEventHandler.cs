#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    /// <summary>
    /// マウス ボタンまたはマウス ホイールを含まないマウス関連のイベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="sender">イベント ハンドラがアタッチされているオブジェクト。</param>
    /// <param name="mouseDevice">MouseDevice。</param>
    public delegate void MouseEventHandler(object sender, MouseDevice mouseDevice);
}
