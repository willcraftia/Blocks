#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Animations
{
    /// <summary>
    /// 指定の値範囲と再生時間から線形補間した値を算出して再生に用いる Animation です。
    /// </summary>
    public abstract class LerpAnimation : TimelineAnimation
    {
        /// <summary>
        /// 開始値を取得または設定します。
        /// </summary>
        public float From { get; set; }

        /// <summary>
        /// 終了値を取得または設定します。
        /// </summary>
        public float To { get; set; }

        protected sealed override void Update(GameTime gameTime, TimeSpan playElapsedTime)
        {
            float amount = (float) (playElapsedTime.TotalMilliseconds / Duration.TotalMilliseconds);
            float current;
            if (!Reversed)
            {
                current = MathHelper.Lerp(From, To, amount);
            }
            else
            {
                current = MathHelper.Lerp(To, From, amount);
            }
            Update(gameTime, playElapsedTime, current);
        }

        /// <summary>
        /// 線形補間した値の算出後に呼び出されます。
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="playElapsedTime"></param>
        /// <param name="current">線形補間した値。</param>
        protected abstract void Update(GameTime gameTime, TimeSpan playElapsedTime, float current);
    }
}
