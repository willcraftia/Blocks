#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Control のアニメーション制御を行うクラスです。
    /// </summary>
    public abstract class Animation
    {
        /// <summary>
        /// Enabled プロパティが変更された時に発生します。
        /// </summary>
        public event EventHandler EnanbledChanged = delegate { };

        /// <summary>
        /// アニメーションが完了した時に発生します。
        /// </summary>
        public event EventHandler Completed = delegate { };

        /// <summary>
        /// true (Update の実行が有効な場合)、false (それ以外の場合)。
        /// </summary>
        bool enabled;

        /// <summary>
        /// Update の実行が有効かどうかを示す値を取得または設定します。
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled == value) return;

                enabled = value;

                // イベントを発生させます。
                OnEnanbledChanged();
            }
        }

        /// <summary>
        /// この Animation を所有する Screen を取得します。
        /// </summary>
        public Screen Screen { get; internal set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        protected Animation() { }

        /// <summary>
        /// 更新します。
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Enabled プロパティが変更された時に呼び出されます。
        /// EnanbledChanged イベントを発生させます。
        /// </summary>
        protected virtual void OnEnanbledChanged()
        {
            if (EnanbledChanged != null) EnanbledChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Completed イベントが発生される時に呼び出されます。
        /// Completed イベントを発生させます。
        /// </summary>
        protected virtual void OnCompleted()
        {
            if (Completed != null) Completed(this, EventArgs.Empty);
        }
    }
}
