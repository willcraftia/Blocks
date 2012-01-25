#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// フォーカス移動の方向を表します。
    /// </summary>
    public enum FocusNavigation
    {
        /// <summary>
        /// フォーカスが設定された Control の次にフォーカス設定可能な Control へフォーカスを移動させます。
        /// </summary>
        Forward,
        /// <summary>
        /// フォーカスが設定された Control の前にフォーカス設定可能な Control へフォーカスを移動させます。
        /// </summary>
        Backward,
        /// <summary>
        /// フォーカスが設定された Control の上にあるフォーカス設定可能な Control へフォーカスを移動させます。
        /// </summary>
        Up,
        /// <summary>
        /// フォーカスが設定された Control の下にあるフォーカス設定可能な Control へフォーカスを移動させます。
        /// </summary>
        Down,
        /// <summary>
        /// フォーカスが設定された Control の左にあるフォーカス設定可能な Control へフォーカスを移動させます。
        /// </summary>
        Left,
        /// <summary>
        /// フォーカスが設定された Control の右にあるフォーカス設定可能な Control へフォーカスを移動させます。
        /// </summary>
        Right
    }
}
