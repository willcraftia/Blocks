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

        int selectedIndex = -1;

        public ControlCollection Items { get; private set; }

        protected override int ChildrenCount
        {
            get { return Items.Count == 0 ? 0 : 1; }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (value < -1) throw new ArgumentOutOfRangeException();
                if (selectedIndex == value) return;

                selectedIndex = value;
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

        protected Selector(Screen screen)
            : base(screen)
        {
            Items = new InternalControlCollection(this);
        }

        protected override Control GetChild(int index)
        {
            if (index != 0 || selectedIndex == -1 || Items.Count == 0)
                throw new ArgumentOutOfRangeException("index");

            return SelectedItem;
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
