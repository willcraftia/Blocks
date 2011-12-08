using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// 中マウス ボタン。
        /// </summary>
        Middle = 2,

        /// <summary>
        /// 右マウス ボタン。
        /// </summary>
        Right = 4,
    }
}
