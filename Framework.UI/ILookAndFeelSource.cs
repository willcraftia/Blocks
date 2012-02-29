#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// ILookAndFeel を提供するクラスのインタフェースです。
    /// </summary>
    public interface ILookAndFeelSource
    {
        /// <summary>
        /// Control に対応する ILookAndFeel を取得します。
        /// </summary>
        /// <param name="control">Control。</param>
        /// <returns>
        /// ILookAndFeel。Control に対応する ILookAndFeel が存在しない場合は null。
        /// </returns>
        ILookAndFeel GetLookAndFeel(Control control);
    }
}
