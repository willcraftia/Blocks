#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Animations
{
    /// <summary>
    /// TimelineAnimation の処理の反復方法を表す構造体です。
    /// </summary>
    public struct Repeat
    {
        /// <summary>
        /// 無制限の反復を表す Repeat を取得します。
        /// </summary>
        public static Repeat Forever
        {
            get { return new Repeat(TimeSpan.MaxValue); }
        }

        /// <summary>
        /// 反復回数を取得します。
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// 反復期間を取得します。
        /// </summary>
        public TimeSpan Duration { get; private set; }

        /// <summary>
        /// 反復を回数で制御するかどうか示す値を取得します。
        /// </summary>
        /// <value>
        /// true (反復を回数で制御する場合)、false (それ以外の場合)。
        /// </value>
        public bool CountEnabled { get; private set; }

        /// <summary>
        /// 反復を期間で制御するかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (反復を期間で制御する場合)、false (それ以外の場合)。
        /// </value>
        public bool DurationEnabled { get; private set; }

        /// <summary>
        /// 反復を回数で制御することを表すインスタンスを生成します。
        /// </summary>
        /// <param name="count">反復回数。</param>
        public Repeat(int count)
            : this()
        {
            Count = count;
            CountEnabled = true;
        }

        /// <summary>
        /// 反復を期間で制御することを表すインスタンスを生成します。
        /// </summary>
        /// <param name="duration">反復期間。</param>
        public Repeat(TimeSpan duration)
            : this()
        {
            Duration = duration;
            DurationEnabled = true;
        }
    }
}
