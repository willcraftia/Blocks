#region Using

using System;
using Willcraftia.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs
{
    public abstract class SpriteSheetLookAndFeel : ILookAndFeel
    {
        protected ISpriteSheetSource SpriteSheetSource { get; private set; }

        protected SpriteSheetLookAndFeel(ISpriteSheetSource spriteSheetSource)
        {
            if (spriteSheetSource == null) throw new ArgumentNullException("spriteSheetSource");
            SpriteSheetSource = spriteSheetSource;
        }

        public abstract void Draw(Control control, IDrawContext drawContext);
    }
}
