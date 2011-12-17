#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// 子 Control を管理するためのコレクションです。
    /// </summary>
    /// <remarks>
    /// コレクションのインデックスは、画面における Control の前後関係を表します。
    /// インデックス 0 は、その親 Control 内での最背面を表します。
    /// </remarks>
    public class ControlCollection : ObservableCollection<Control>
    {
        Control parent;

        /// <summary>
        /// parent で指定した Control の子を管理するためのインスタンスを生成します。
        /// </summary>
        /// <param name="parent">このコレクションを所有する Control。</param>
        public ControlCollection(Control parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            this.parent = parent;
        }

        /// <summary>
        /// 指定の Control を最前面 (インデックス 0) へ移動させます。
        /// </summary>
        /// <param name="item"></param>
        public void MoveToTopMost(Control item)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (!Contains(item)) throw new ArgumentException("Collection dose not contain the specified control.", "item");

            base.RemoveAt(IndexOf(item));
            base.Insert(Count, item);
        }

        protected override void InsertItem(int index, Control item)
        {
            // 子として追加可能かどうかを検査
            validateControl(item);
            
            base.InsertItem(index, item);
            
            // 親を設定
            item.Parent = parent;
        }

        protected override void RemoveItem(int index)
        {
            // 除去される Control の親をリセット
            base[index].Parent = null;

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, Control item)
        {
            // 子として追加可能かどうかを検査
            validateControl(item);

            // 除去される Control の親をリセット
            base[index].Parent = null;
            
            base.SetItem(index, item);

            // 親を設定
            item.Parent = parent;
        }

        protected override void ClearItems()
        {
            // 親をリセット
            foreach (var item in this) item.Parent = null;

            base.ClearItems();
        }

        /// <summary>
        /// 指定の Control が、このコレクションに追加可能な状態であるかどうかを検査します。
        /// 追加不能な状態の場合には例外が発生します。
        /// </summary>
        /// <param name="control">このコレクションに追加しようとしている Control。</param>
        void validateControl(Control control)
        {
            // 他の子であってはならない
            if (control.Parent != null) throw new InvalidOperationException("Control is already the child of another control.");
            // 自身を子孫にはできない
            if (IsAncestor(control)) throw new InvalidOperationException("Control can not be the descendant of one's own.");
        }

        /// <summary>
        /// 指定の Control が、このコレクションの所有者、あるいは、祖先であるかどうかを判定します。
        /// </summary>
        /// <param name="control">判定対象の Control。</param>
        /// <returns>true (指定の Control がこのコレクションを所有者、あるいは、祖先である場合)、false (それ以外の場合)。</returns>
        bool IsAncestor(Control control)
        {
            var ancestor = parent;
            while (ancestor != null)
            {
                if (ancestor == control) return true;
                ancestor = ancestor.Parent;
            }
            return false;
        }
    }
}
