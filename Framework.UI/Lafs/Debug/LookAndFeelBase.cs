#region Using

using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    /// <summary>
    /// デバッグ用 ILookAndFeel の基礎クラスです。
    /// </summary>
    public abstract class LookAndFeelBase : ILookAndFeel
    {
        // I/F
        public abstract void Draw(Control control, IDrawContext drawContext);
    }
}
