#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// フォーカス設定の方向を表します。
    /// </summary>
    public enum FocusNavigationDirection
    {
        /// <summary>
        /// 上にある Control にフォーカスを設定します。
        /// </summary>
        Up,
        /// <summary>
        /// 下にある Control にフォーカスを設定します。
        /// </summary>
        Down,
        /// <summary>
        /// 左にある Control にフォーカスを設定します。
        /// </summary>
        Left,
        /// <summary>
        /// 右にある Control にフォーカスを設定します。
        /// </summary>
        Right,
    }
}
