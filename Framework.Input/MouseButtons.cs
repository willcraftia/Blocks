#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    /// <summary>
    /// マウス ボタン。
    /// </summary>
    [Flags]
    public enum MouseButtons
    {
        /// <summary>
        /// 左マウス ボタン。
        /// </summary>
        Left = 1,

        /// <summary>
        /// 中央マウス ボタン。
        /// </summary>
        Middle = 2,

        /// <summary>
        /// 右マウス ボタン。
        /// </summary>
        Right = 4,
    }
}
