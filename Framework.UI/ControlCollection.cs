#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// 子コントロールを管理するためのコレクションです。
    /// </summary>
    /// <remarks>
    /// コレクションのインデックスは、画面におけるコントロールの前後関係を表します。
    /// インデックス 0 は、その親コントロール内における最前面の子コントロールを表します。
    /// </remarks>
    public class ControlCollection : Collection<Control>
    {
        Control parent;

        /// <summary>
        /// parent で指定したコントロールの子コントロールを管理するためのインスタンスを生成します。
        /// </summary>
        /// <param name="parent">このコレクションを所有するコントロール。</param>
        public ControlCollection(Control parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            
            this.parent = parent;
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
            // 除去されるコントロールの親をリセット
            base[index].Parent = null;

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, Control item)
        {
            // 子として追加可能かどうかを検査
            validateControl(item);
            // 除去されるコントロールの親をリセット
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
        /// 指定のコントロールが、このコレクションに追加可能な状態であるかどうかを検査します。
        /// 追加不能な状態の場合には例外が発生します。
        /// </summary>
        /// <param name="control">このコレクションに追加しようとしているコントロール。</param>
        void validateControl(Control control)
        {
            // 他コントロールの子であってはならない
            if (control.Parent != null) throw new InvalidOperationException("Control is already the child of another control.");
            // 自身を子孫コントロールにはできない
            if (IsAncestor(control)) throw new InvalidOperationException("Control can not be the descendant of one's own.");
        }

        /// <summary>
        /// 指定のコントロールが、このコレクションの所有者、あるいは、祖先であるかどうかを判定します。
        /// </summary>
        /// <param name="control">判定対象のコントロール。</param>
        /// <returns>true (指定のコントロールがこのコレクションを所有者、あるいは、祖先である場合)、false (それ以外の場合)。</returns>
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
