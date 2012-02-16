#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// FocusScope を持つ Control のインタフェースです。
    /// </summary>
    public interface IFocusScopeControl
    {
        /// <summary>
        /// FocusScope を取得します。
        /// </summary>
        FocusScope FocusScope { get; }
    }
}
