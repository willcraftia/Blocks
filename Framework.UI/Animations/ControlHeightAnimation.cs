#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Animations
{
    public sealed class ControlHeightAnimation : LerpAnimation
    {
        protected override void Update(GameTime gameTime, TimeSpan playElapsedTime, float current)
        {
            Control.Height = current;
        }
    }
}
