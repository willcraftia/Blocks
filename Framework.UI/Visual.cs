#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public class Visual
    {
        Size visualRenderSize;

        public bool ClipEnabled { get; set; }

        public Rectangle Bounds { get; set; }

        public float Opacity { get; set; }

        protected internal virtual void Draw(GameTime gameTime, IDrawContext drawContext) { }
    }
}
