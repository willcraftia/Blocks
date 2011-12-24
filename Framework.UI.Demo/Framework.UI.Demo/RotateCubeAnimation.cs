#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo
{
    public sealed class RotateCubeAnimation : Animation
    {
        float yaw;
        float pitch;
        float roll;

        // 1 秒 (1000 ミリ秒) で半回転。
        float delta = MathHelper.PiOver2 / 1000.0f;

        public override void Update(GameTime gameTime)
        {
            var button = Control as CubeButton;
            if (button == null) return;

            //float time = (float) gameTime.TotalGameTime.TotalSeconds;

            //yaw = time * 0.4f;
            //pitch = time * 0.7f;
            //roll = time * 1.1f;

            float elapsed = (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            yaw += elapsed * delta;
            pitch += elapsed * delta;
            roll += elapsed * delta;

            button.Orientation = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);
        }
    }
}
