#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Animations
{
    /// <summary>
    /// TimelineAnimation をグループ化する TimelineAnimation です。 
    /// </summary>
    public class TimelineGroupAnimation : TimelineAnimation
    {
        /// <summary>
        /// 子 TimelineAnimation のコレクションを取得します。
        /// </summary>
        public TimelineAnimationCollection Children { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public TimelineGroupAnimation()
        {
            Children = new TimelineAnimationCollection(this);
        }

        protected override void Update(GameTime gameTime, TimeSpan playElapsedTime)
        {
            // 再生時間内であるならば、子の再生を試みます。
            foreach (var child in Children)
            {
                if (child.Enabled) child.Update(gameTime);
            }
        }

        protected override void Complete(GameTime gameTime)
        {
            // 親が先に再生完了判定をするため、再生完了の直前で子の Update を呼び出します。
            foreach (var child in Children)
            {
                if (child.Enabled) child.Update(gameTime);
            }

            base.Complete(gameTime);
        }

        protected override void OnEnanbledChanged()
        {
            // 子は親の Enabled に従います。
            foreach (var child in Children) child.Enabled = Enabled;

            base.OnEnanbledChanged();
        }

        internal void AddChild(Animation child)
        {
            // 子は親の Enabled に従います。
            child.Enabled = Enabled;
        }

        internal void RemoveChild(Animation child)
        {
            // 削除時は子を強制停止します。
            child.Enabled = false;
        }
    }
}
