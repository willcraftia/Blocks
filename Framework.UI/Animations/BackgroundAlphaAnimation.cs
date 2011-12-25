#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Animations
{
    public sealed class BackgroundAlphaAnimation : LerpAnimation
    {
        protected override void Update(GameTime gameTime, TimeSpan playElapsedTime, float current)
        {
            var color = Control.BackgroundColor;
            Control.BackgroundColor = new Color(color.R, color.G, color.B, current);
        }
    }
}
