#region Using

using System;
using System.Collections.ObjectModel;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Window を管理するためのコレクションです。
    /// </summary>
    public sealed class WindowCollection : Collection<Window>
    {
        /// <summary>
        /// このコレクションを所有する Root。
        /// </summary>
        Root root;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="root">このコレクションを所有する Root。</param>
        internal WindowCollection(Root root)
        {
            if (root == null) throw new ArgumentNullException("root");
            this.root = root;
        }

        protected override void InsertItem(int index, Window item)
        {
            base.InsertItem(index, item);

            root.AddChildInternal(item);
        }

        protected override void RemoveItem(int index)
        {
            var removedItem = this[index];
            base.RemoveItem(index);

            root.RemoveChildInternal(removedItem);
        }

        protected override void SetItem(int index, Window item)
        {
            var removedItem = this[index];
            base.SetItem(index, item);

            root.AddChildInternal(item);
            root.RemoveChildInternal(removedItem);
        }
    }
}
