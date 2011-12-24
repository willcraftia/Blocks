#region Using

using System;
using System.Collections.ObjectModel;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Control に設定する Animation を管理するコレクションです。
    /// </summary>
    public sealed class AnimationCollection : KeyedCollection<string, Animation>
    {
        /// <summary>
        /// このコレクションを所持する Control を取得します。
        /// </summary>
        public Control Control { get; private set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="control">このコレクションを所持する Control。</param>
        public AnimationCollection(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            Control = control;
        }

        /// <summary>
        /// 指定の要素についてコレクションで管理するキーを変更します。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newKey"></param>
        internal void ChangeKey(Animation item, string newKey)
        {
            base.ChangeItemKey(item, newKey);
        }

        protected override string GetKeyForItem(Animation item)
        {
            // 名前が未設定ならばコレクションで命名します。
            if (string.IsNullOrEmpty(item.Name)) item.Name = "Animation_" + Count;

            return item.Name;
        }

        protected override void InsertItem(int index, Animation item)
        {
            base.InsertItem(index, item);

            // Control を設定します。
            item.Control = Control;
        }

        protected override void RemoveItem(int index)
        {
            // Control を解除します。
            base[index].Control = null;

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, Animation item)
        {
            // Control を解除します。
            base[index].Control = null;

            base.SetItem(index, item);

            // Control を設定します。
            item.Control = Control;
        }

        protected override void ClearItems()
        {
            foreach (var item in Items)
            {
                // Control を解除します。
                item.Control = null;
            }

            base.ClearItems();
        }
    }
}
