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

        public CubeButton CubeButton { get; set; }

        protected override void Update(GameTime gameTime, TimeSpan playElapsedTime, float current)
        {
            if (CubeButton == null) throw new InvalidOperationException("CubeButton is null.");

            yaw = current;
            pitch = current;
            roll = current;

            CubeButton.Orientation = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);
        }
    }
}
