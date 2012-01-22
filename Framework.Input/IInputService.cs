#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    /// <summary>
    /// 入力サービスのインタフェースです。
    /// </summary>
    public interface IInputService
    {
        /// <summary>
        /// MouseDevice を取得します。
        /// </summary>
        MouseDevice MouseDevice { get; }

        /// <summary>
        /// KeyboardDevice を取得します。
        /// </summary>
        KeyboardDevice KeyboardDevice { get; }
    }
}
