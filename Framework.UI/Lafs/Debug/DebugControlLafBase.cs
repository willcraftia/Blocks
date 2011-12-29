#region Using

using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public abstract class DebugControlLafBase : ControlLafBase
    {
        public DebugControlLafSource Source { get; set; }

        protected DebugControlLafBase() { }
    }
}
