#region Using

using System;
using System.Collections.Generic;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
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

                parent.AddChildInternal(item);
            }

            protected override void RemoveItem(int index)
            {
                var removedItem = this[index];
                base.RemoveItem(index);

                parent.RemoveChildInternal(removedItem);
            }

            protected override void SetItem(int index, Control item)
            {
                var removedItem = this[index];
                base.SetItem(index, item);

                parent.AddChildInternal(item);
                parent.RemoveChildInternal(removedItem);
            }
        }

        #endregion

        public event EventHandler SelectionChanged = delegate { };

        int selectedIndex = -1;

        public ControlCollection Items { get; private set; }

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

        public Control SelectedItem
        {
            get
            {
                if (selectedIndex == -1 || Items.Count == 0) return null;
                return selectedIndex < Items.Count ? Items[selectedIndex] : Items[Items.Count - 1];
            }
        }

        protected override int ChildrenCount
        {
            get { return Items.Count; }
        }

        protected Selector(Screen screen)
            : base(screen)
        {
            Items = new InternalControlCollection(this);
        }

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
            if (SelectionChanged != null) SelectionChanged(this, EventArgs.Empty);
        }

        internal void AddChildInternal(Control child)
        {
            AddChild(child);
        }

        internal void RemoveChildInternal(Control child)
        {
            RemoveChild(child);
        }
    }
}
