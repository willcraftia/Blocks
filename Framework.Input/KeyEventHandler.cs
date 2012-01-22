#region Using

using System;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    /// <summary>
    /// キーの押下と解放に関連するイベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="sender">イベント ハンドラがアタッチされているオブジェクト。</param>
    /// <param name="keyboardDevice">KeyboardDevice。</param>
    /// <param name="buttons">イベントに関連付けられているキー。</param>
    public delegate void KeyEventHandler(object sender, KeyboardDevice keyboardDevice, Keys key);
}
