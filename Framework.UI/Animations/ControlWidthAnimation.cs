#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Animations
{
    public sealed class ControlWidthAnimation : LerpAnimation
    {
        protected override void Update(GameTime gameTime, TimeSpan playElapsedTime, float current)
        {
            Control.Width = current;
        }
    }
}
