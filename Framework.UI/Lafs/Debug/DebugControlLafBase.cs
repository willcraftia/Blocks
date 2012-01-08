#region Using

using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    /// <summary>
    /// デバッグ用 LaF の基礎クラスです。
    /// </summary>
    public abstract class DebugControlLafBase : IControlLaf
    {
        /// <summary>
        /// DebugControlLafSource を取得します。
        /// </summary>
        protected internal DebugControlLafSource Source { get; internal set; }

        // I/F
        public abstract void Draw(Control control, IDrawContext drawContext);
    }
}
