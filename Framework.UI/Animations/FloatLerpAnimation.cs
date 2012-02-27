#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Animations
{
    /// <summary>
    /// 値範囲と再生時間から線形補間した float 値を算出して再生に用いる Animation です。
    /// </summary>
    public sealed class FloatLerpAnimation : TimelineAnimation
    {
        /// <summary>
        /// 開始値を取得または設定します。
        /// </summary>
        public float From { get; set; }

        /// <summary>
        /// 終了値を取得または設定します。
        /// </summary>
        public float To { get; set; }

        /// <summary>
        /// 線形補間した値を設定するメソッドを取得または設定します。
        /// </summary>
        public Action<float> Action { get; set; }

        protected override void Update(GameTime gameTime, TimeSpan playElapsedTime)
        {
            float amount = (float) (playElapsedTime.TotalMilliseconds / Duration.TotalMilliseconds);

            float from = (!Reversed) ? From : To;
            float to = (!Reversed) ? To : From;
            float current = MathHelper.Lerp(from, to, amount);

            if (Action != null) Action(current);
        }

        protected override void OnEnanbledChanged()
        {
            if (!Enabled)
            {
                // 強制終了の可能性を考えて終了値の設定を要求します。
                float to = (!AutoReversed) ? To : From;
                if (Action != null) Action(to);
            }

            base.OnEnanbledChanged();
        }
    }
}
