#region Using

using System;
using System.Collections.ObjectModel;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Screen の Animation を管理するコレクションです。
    /// </summary>
    public class AnimationCollection : KeyedCollection<string, Animation>
    {
        /// <summary>
        /// このコレクションを所持する Screen を取得します。
        /// </summary>
        public Screen Screen { get; private set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="screen">このコレクションを所持する Screen。</param>
        public AnimationCollection(Screen screen)
        {
            if (screen == null) throw new ArgumentNullException("screen");
            Screen = screen;
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

            // Screen を設定します。
            item.Screen = Screen;
        }

        protected override void RemoveItem(int index)
        {
            // Screen を解除します。
            base[index].Screen = null;

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, Animation item)
        {
            // Screen を解除します。
            base[index].Screen = null;

            base.SetItem(index, item);

            // Screen を設定します。
            item.Screen = Screen;
        }

        protected override void ClearItems()
        {
            foreach (var item in Items)
            {
                // Screen を解除します。
                item.Screen = null;
            }

            base.ClearItems();
        }
    }
}
