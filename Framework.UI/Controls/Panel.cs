﻿#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    /// <summary>
    /// Control のコンテナとして振る舞う Control です。
    /// </summary>
    public class Panel : Control
    {
        #region InternalControlCollection

        class InternalControlCollection : ControlCollection
        {
            Panel parent;

            internal InternalControlCollection(Panel parent)
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

        public ControlCollection Children { get; private set; }

        /// <summary>
        /// Children のサイズを返します。
        /// </summary>
        protected override int ChildrenCount
        {
            get { return Children.Count; }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        protected Panel(Screen screen)
            : base(screen)
        {
            Children = new InternalControlCollection(this);
        }

        protected override Control GetChild(int index)
        {
            if (index < 0 || ChildrenCount <= index)
                throw new ArgumentOutOfRangeException("index");
            return Children[index];
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
