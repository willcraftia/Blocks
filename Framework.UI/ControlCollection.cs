#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Screen の Control を管理するコレクションです。
    /// </summary>
    public class ControlCollection : KeyedCollection<string, Control>
    {
        /// <summary>
        /// このコレクションを所持する Screen を取得します。
        /// </summary>
        public Screen Screen { get; private set; }

        /// <summary>
        /// 型の簡易名をキーとして、その型のインスタンスが追加された数を値とする Dictionary。
        /// </summary>
        Dictionary<string, int> counters;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="screen">このコレクションを所持する Screen。</param>
        public ControlCollection(Screen screen)
        {
            if (screen == null) throw new ArgumentNullException("screen");
            Screen = screen;
        }

        /// <summary>
        /// 指定の要素についてコレクションで管理するキーを変更します。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newKey"></param>
        internal void ChangeKey(Control item, string newKey)
        {
            base.ChangeItemKey(item, newKey);
        }

        /// <summary>
        /// 指定の要素に設定するキーを生成します。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual string GenerateKey(Control item)
        {
            if (counters == null) counters = new Dictionary<string, int>();

            var baseName = item.GetType().Name;
            int counter = 0;
            counters.TryGetValue(baseName, out counter);
            counters[baseName] = ++counter;
            return baseName + "_" + counter;
        }

        protected override string GetKeyForItem(Control item)
        {
            // 名前が未設定ならばコレクションで命名します。
            if (string.IsNullOrEmpty(item.Name)) item.Name = GenerateKey(item);

            return item.Name;
        }

        protected override void RemoveItem(int index)
        {
            var removedItem = base[index];
            removedItem.Defocus();

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, Control item)
        {
            var removedItem = base[index];
            removedItem.Defocus();

            base.SetItem(index, item);
        }

        protected override void ClearItems()
        {
            foreach (var item in Items) item.Defocus();

            base.ClearItems();
        }
    }
}
