#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    /// <summary>
    /// 文字入力イベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="sender">イベント ハンドラがアタッチされているオブジェクト。</param>
    /// <param name="keyboardDevice">KeyboardDevice。</param>
    /// <param name="character">入力された文字。</param>
    public delegate void CharacterInputEventHandler(object sender, KeyboardDevice keyboardDevice, char character);
}
