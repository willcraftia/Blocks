#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// 親 Control のレイアウトにおける子 Control の垂直方向の配置方法を表します。
    /// </summary>
    public enum VerticalAlignment
    {
        /// <summary>
        /// 上端に揃えて配置します。
        /// </summary>
        Top,
        /// <summary>
        /// 下端に揃えて配置します。
        /// </summary>
        Bottom,
        /// <summary>
        /// 中央に揃えて配置します。
        /// </summary>
        Center,
        /// <summary>
        /// 親 Control が許容するサイズまで引き伸ばして配置します。
        /// </summary>
        Stretch
    }
}
