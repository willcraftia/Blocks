#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Screen で管理する全ての Control のルートとなる Control です。
    /// </summary>
    public sealed class Desktop : ContentControl
    {
        #region InternalWindowCollection

        /// <summary>
        /// Desktop の Windows プロパティのクラスです。
        /// </summary>
        class InternalWindowCollection : WindowCollection
        {
            /// <summary>
            /// インスタンスを生成します。
            /// </summary>
            /// <param name="desktop">インスタンスを所持する Desktop。</param>
            internal InternalWindowCollection(Desktop desktop)
                : base(desktop)
            {
            }

            protected override void InsertItem(int index, Window item)
            {
                base.InsertItem(index, item);

                Desktop.AddChildInternal(item);
            }

            protected override void RemoveItem(int index)
            {
                var removedItem = this[index];
                base.RemoveItem(index);

                Desktop.RemoveChildInternal(removedItem);
            }

            protected override void SetItem(int index, Window item)
            {
                var removedItem = this[index];
                base.SetItem(index, item);

                Desktop.AddChildInternal(item);
                Desktop.RemoveChildInternal(removedItem);
            }
        }

        #endregion

        /// <summary>
        /// 管理している Window のコレクションを取得します。
        /// </summary>
        public WindowCollection Windows { get; private set; }

        /// <summary>
        /// Content に Control が設定されているならば Window 数 + 1 を返し、
        /// それ以外ならば Window 数を返します。
        /// </summary>
        protected override int ChildrenCount
        {
            get { return Windows.Count + base.ChildrenCount; }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        internal Desktop(Screen screen)
            : base(screen)
        {
            Windows = new InternalWindowCollection(this);
        }

        protected override Control GetChild(int index)
        {
            if (index < 0 || ChildrenCount <= index)
                throw new ArgumentOutOfRangeException("index");

            if (Content == null) return Windows[index];
            return (index == 0)  ? Content : Windows[index - 1];
        }

        /// <summary>
        /// InternalWindowCollection へ Control を追加した時に呼び出され、
        /// 追加された Control を Desktop の子として関連付けます。
        /// </summary>
        /// <param name="child"></param>
        internal void AddChildInternal(Control child)
        {
            AddChild(child);
        }

        /// <summary>
        /// InternalWindowCollection から Control を削除した時に呼び出され、
        /// Desktop から削除された子 Control の関連付けを削除します。
        /// </summary>
        /// <param name="child"></param>
        internal void RemoveChildInternal(Control child)
        {
            RemoveChild(child);
        }
    }
}
