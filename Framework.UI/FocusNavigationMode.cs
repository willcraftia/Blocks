#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// フォーカスの移動方法を表します。
    /// </summary>
    public enum FocusNavigationMode
    {
        /// <summary>
        /// コンテナ内でフォーカス ナビゲーションを許可していません。
        /// </summary>
        None,

        /// <summary>
        /// コンテナの先頭あるいは末尾に到達した時に、末尾あるいは先頭にフォーカスが戻ります。
        /// </summary>
        Cycle,

        /// <summary>
        /// コンテナの先頭あるいは末尾に到達した時に、その要素でフォーカスの移動が停止します。
        /// </summary>
        Wrapped
    }
}
