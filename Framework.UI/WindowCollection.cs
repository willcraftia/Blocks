#region Using

using System;
using System.Collections.ObjectModel;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Window を管理するためのコレクションです。
    /// </summary>
    public class WindowCollection : Collection<Window>
    {
        /// <summary>
        /// このコレクションを所有する Root。
        /// </summary>
        protected Root Root { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="root">このコレクションを所有する Root。</param>
        public WindowCollection(Root root)
        {
            if (root == null) throw new ArgumentNullException("root");
            Root = root;
        }

        protected override void InsertItem(int index, Window item)
        {
            base.InsertItem(index, item);

            Root.AddChildInternal(item);
        }

        protected override void RemoveItem(int index)
        {
            var removedItem = this[index];
            base.RemoveItem(index);

            Root.RemoveChildInternal(removedItem);
        }

        protected override void SetItem(int index, Window item)
        {
            var removedItem = this[index];
            base.SetItem(index, item);

            Root.AddChildInternal(item);
            Root.RemoveChildInternal(removedItem);
        }
    }
}
