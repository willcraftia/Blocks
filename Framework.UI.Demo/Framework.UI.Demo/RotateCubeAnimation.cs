#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo
{
    public sealed class RotateCubeAnimation : Animations.LerpAnimation
    {
        float yaw;
        float pitch;
        float roll;

        protected override void Update(GameTime gameTime, TimeSpan playElapsedTime, float current)
        {
            var button = Control as CubeButton;
            if (button == null) return;

            yaw = current;
            pitch = current;
            roll = current;

            button.Orientation = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);
        }
    }
}
