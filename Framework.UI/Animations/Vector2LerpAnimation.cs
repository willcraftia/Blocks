#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Animations
{
    public sealed class Vector2LerpAnimation : TimelineAnimation
    {
        /// <summary>
        /// 開始値を取得または設定します。
        /// </summary>
        public Vector2 From { get; set; }

        /// <summary>
        /// 終了値を取得または設定します。
        /// </summary>
        public Vector2 To { get; set; }

        /// <summary>
        /// 線形補間した値を設定するメソッドを取得または設定します。
        /// </summary>
        public Action<Vector2> Action { get; set; }

        protected override void Update(GameTime gameTime, TimeSpan playElapsedTime)
        {
            float amount = (float) (playElapsedTime.TotalMilliseconds / Duration.TotalMilliseconds);

            Vector2 from = (!Reversed) ? From : To;
            Vector2 to = (!Reversed) ? To : From;
            Vector2 current = new Vector2
            {
                X = MathHelper.Lerp(from.X, to.X, amount),
                Y = MathHelper.Lerp(from.Y, to.Y, amount)
            };

            if (Action != null) Action(current);
        }

        protected override void OnEnanbledChanged()
        {
            if (!Enabled)
            {
                // 強制終了の可能性を考えて終了値の設定を要求します。
                Vector2 to = (!AutoReversed) ? To : From;
                if (Action != null) Action(to);
            }

            base.OnEnanbledChanged();
        }
    }
}
