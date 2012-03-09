#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    /// <summary>
    /// ListBox での選択動作を定義します。
    /// </summary>
    public enum ListBoxSelectionMode
    {
        /// <summary>
        /// 一度に 1 つの項目しか選択できません。
        /// </summary>
        Single,
        /// <summary>
        /// 一度に複数の項目を選択できます。
        /// </summary>
        Multiple
    }
}
