#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Animations
{
    /// <summary>
    /// 再生時間制御を行う Animation です。
    /// </summary>
    public abstract class TimelineAnimation : Animation
    {
        /// <summary>
        /// アニメーションが有効になってから再生を開始するまでの時間。
        /// </summary>
        TimeSpan beginTime = TimeSpan.Zero;

        /// <summary>
        /// 再生期間。
        /// </summary>
        TimeSpan duration = TimeSpan.Zero;

        /// <summary>
        /// 再生の反復方法。
        /// </summary>
        Repeat repeat = Repeat.Once;

        /// <summary>
        /// 再生時間の判定を開始したゲーム内経過時間。
        /// </summary>
        TimeSpan activatedTime;

        /// <summary>
        /// 再生回数。
        /// </summary>
        int repeatCount;

        /// <summary>
        /// true (再生時間の判定を開始した場合)、false (それ以外の場合)。
        /// </summary>
        bool activated;

        /// <summary>
        /// アニメーションが有効になってから再生を開始するまでの時間を取得または設定します。。
        /// </summary>
        public TimeSpan BeginTime
        {
            get { return beginTime; }
            set { beginTime = value; }
        }

        /// <summary>
        /// 再生期間を取得または設定します。
        /// </summary>
        public TimeSpan Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        /// <summary>
        /// 再生の反復方法を取得または設定します。
        /// </summary>
        public Repeat Repeat
        {
            get { return repeat; }
            set { repeat = value; }
        }

        /// <summary>
        /// 再生完了後に逆再生するかどうかを示す値を取得または設定します。
        /// </summary>
        /// <value>
        /// true (再生完了後に逆再生する場合)、false (それ以外の場合)。
        /// </value>
        /// <remarks>
        /// AutoReverse プロパティを true に設定すると、再生時間が Duration プロパティで指定した長さの 2 倍になります。 
        /// </remarks>
        public bool AutoReversed { get; set; }

        /// <summary>
        /// 逆再生中であるかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (逆再生中の場合)、false (それ以外の場合)。
        /// </value>
        public bool Reversed { get; private set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        protected TimelineAnimation()
        {
            Enabled = false;
        }

        /// <summary>
        /// 再生期間内の場合に呼び出されます。
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="playElapsedTime">再生を開始してからの経過時間。</param>
        protected abstract void Update(GameTime gameTime, TimeSpan playElapsedTime);

        public sealed override void Update(GameTime gameTime)
        {
            if (!activated)
            {
                // 再生開始時間を記録します。
                activatedTime = gameTime.TotalGameTime;
                activated = true;
            }
            else
            {
                var elapsedTime = gameTime.TotalGameTime - activatedTime;
                
                // 開始時間に到達していないならば時間経過を待ちます。
                if (elapsedTime < beginTime) return;

                // 終了時間を越えるならば Repeat に従います。
                var endTime = beginTime + duration;
                if (endTime < elapsedTime)
                {
                    if (AutoReversed && !Reversed)
                    {
                        Reversed = true;
                        // 逆再生開始時間を記録します。
                        activatedTime = gameTime.TotalGameTime;
                        return;
                    }

                    // まずはアニメーション終了としてマークします。
                    activated = false;

                    // 逆再生していたなら逆再生終了としてマークします。
                    Reversed = false;

                    if (repeat.CountEnabled)
                    {
                        // 指定回数未満ならば再生の開始から Update されるようにします。
                        repeatCount++;
                        if (repeatCount < repeat.Count) return;
                    }
                    else if (repeat.DurationEnabled)
                    {
                        // 指定期間未満ならば再生の開始から Update されるようにします。
                        if (endTime < repeat.Duration) return;
                    }

                    // 再生を終えます。
                    Enabled = false;
                    // 完了イベントを発生させます。
                    OnCompleted();
                    return;
                }

                // 再生時間内ならばアニメーションを処理します。
                var playElapsedTime = elapsedTime - beginTime;
                Update(gameTime, playElapsedTime);
            }
        }

        protected override void OnEnanbledChanged()
        {
            if (!Enabled)
            {
                // false が設定されたならば再生を強制終了させます。
                // なお、強制終了の場合には、Completed イベントを発生させないことにします。
                activatedTime = TimeSpan.Zero;
                repeatCount = 0;
                activated = false;
                Reversed = false;
            }

            base.OnEnanbledChanged();
        }
    }
}
