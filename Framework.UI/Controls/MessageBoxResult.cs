#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    /// <summary>
    /// MessageBox で選択された結果を表します。
    /// </summary>
    public enum MessageBoxResult
    {
        /// <summary>
        /// 結果なし。
        /// </summary>
        None,
        /// <summary>
        /// [OK] ボタンが選択されました。
        /// </summary>
        OK,
        /// <summary>
        /// [Cancel] ボタンが選択されました。
        /// </summary>
        Cancel,
        /// <summary>
        /// [Yes] ボタンが選択されました。
        /// </summary>
        Yes,
        /// <summary>
        /// [No] ボタンが選択されました。
        /// </summary>
        No
    }
}
