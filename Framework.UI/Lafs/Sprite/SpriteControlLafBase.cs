#region Using

using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    public abstract class SpriteControlLafBase : ControlLafBase
    {
        public SpriteControlLafSource Source { get; set; }

        protected SpriteControlLafBase() { }
    }
}
