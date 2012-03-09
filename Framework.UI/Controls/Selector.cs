#region Using

using System;
using System.Collections.Generic;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    /// <summary>
    /// 子要素から項目を選択できるようにする Control です。
    /// </summary>
    public class Selector : Control
    {
        #region InternalControlCollection

        class InternalControlCollection : ControlCollection
        {
            Selector parent;

            internal InternalControlCollection(Selector parent)
                : base(parent)
            {
                this.parent = parent;
            }

            protected override void InsertItem(int index, Control item)
            {
                base.InsertItem(index, item);

                parent.AddItemInternal(item);
            }

            protected override void RemoveItem(int index)
            {
                var removedItem = this[index];
                base.RemoveItem(index);

                parent.RemoveItemInternal(removedItem);
            }

            protected override void SetItem(int index, Control item)
            {
                var removedItem = this[index];
                base.SetItem(index, item);

                parent.AddItemInternal(item);
                parent.RemoveItemInternal(removedItem);
            }
        }

        #endregion

        /// <summary>
        /// 選択項目が変更された時に呼び出されます。
        /// </summary>
        public event EventHandler SelectionChanged = delegate { };

        /// <summary>
        /// 選択されている最初の項目のインデックス。
        /// 選択範囲が空の場合は -1。
        /// </summary>
        int selectedIndex = -1;

        /// <summary>
        /// 選択できる項目のリストを取得します。
        /// </summary>
        public ControlCollection Items { get; private set; }

        /// <summary>
        /// 選択されている最初の項目のインデックスを取得または設定します。
        /// 選択範囲が空の場合は -1 です。
        /// </summary>
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (value < -1) throw new ArgumentOutOfRangeException();
                if (selectedIndex == value) return;

                selectedIndex = value;

                OnSelectionChanged();
            }
        }

        /// <summary>
        /// 選択されている最初の項目を取得します。
        /// 選択された項目が存在しない場合は null を返します。
        /// </summary>
        public Control SelectedItem
        {
            get
            {
                if (selectedIndex == -1 || Items.Count == 0) return null;
                return selectedIndex < Items.Count ? Items[selectedIndex] : Items[Items.Count - 1];
            }
        }

        /// <summary>
        /// Items プロパティの Count プロパティを返します。
        /// </summary>
        protected override int ChildrenCount
        {
            get { return Items.Count; }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        protected Selector(Screen screen)
            : base(screen)
        {
            Items = new InternalControlCollection(this);
        }

        /// <summary>
        /// Items プロパティから index 位置にある要素を返します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override Control GetChild(int index)
        {
            return Items[index];
        }

        /// <summary>
        /// SelectedIndex/SelectedItem プロパティが変更された時に呼び出されます。
        /// SelectionChanged イベントを発生させます。
        /// </summary>
        protected virtual void OnSelectionChanged()
        {
            SelectionChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// 項目を追加します。
        /// </summary>
        /// <param name="item">追加する項目。</param>
        protected virtual void AddItem(Control item)
        {
            AddChild(item);
        }

        /// <summary>
        /// 項目を削除します。
        /// </summary>
        /// <param name="item">削除する項目。</param>
        protected virtual void RemoveItem(Control item)
        {
            RemoveChild(item);
        }

        /// <summary>
        /// InternalControlCollection に項目を追加した時に呼び出され、
        /// AddItem(Control) メソッドを呼び出します。
        /// </summary>
        /// <param name="child"></param>
        internal void AddItemInternal(Control child)
        {
            AddItem(child);
        }

        /// <summary>
        /// InternalControlCollection から項目を削除した時に呼び出され、
        /// RemoveItem(Control) メソッドを呼び出します。
        /// </summary>
        /// <param name="child"></param>
        internal void RemoveItemInternal(Control child)
        {
            RemoveItem(child);
        }
    }
}
