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
    public class ControlCollection : KeyedCollection<string, Control>
    {
        /// <summary>
        /// このコレクションを所有する Control。
        /// </summary>
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
            return "Control_" + Count;
        }

        protected override string GetKeyForItem(Control item)
        {
            // 名前が未設定ならばコレクションで命名します。
            if (string.IsNullOrEmpty(item.Name)) item.Name = GenerateKey(item);

            return item.Name;
        }

        protected override void InsertItem(int index, Control item)
        {
            // 子として追加可能かどうかを検査します。
            Validate(item);
            
            base.InsertItem(index, item);
            
            // 親を設定します。
            item.Parent = parent;
        }

        protected override void RemoveItem(int index)
        {
            // 除去される Control の親をリセットします。
            base[index].Parent = null;

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, Control item)
        {
            // 子として追加可能かどうかを検査します。
            Validate(item);

            // 除去される Control の親をリセットします。
            base[index].Parent = null;
            
            base.SetItem(index, item);

            // 親を設定します。
            item.Parent = parent;
        }

        protected override void ClearItems()
        {
            foreach (var item in this) item.Parent = null;

            base.ClearItems();
        }

        /// <summary>
        /// 指定の Control が、このコレクションに追加可能な状態であるかどうかを検査します。
        /// 追加不能な状態の場合には例外が発生します。
        /// </summary>
        /// <param name="control">このコレクションに追加しようとしている Control。</param>
        void Validate(Control control)
        {
            // 自身を子孫にはできない
            if (control.IsAncestorOf(parent)) throw new InvalidOperationException("Control can not be the descendant of one's own.");
        }
    }
}
