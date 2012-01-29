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
    public class ControlCollection : Collection<Control>
    {
        /// <summary>
        /// このコレクションを所有する Control。
        /// </summary>
        protected Control Parent { get; private set; }

        /// <summary>
        /// parent で指定した Control の子を管理するためのインスタンスを生成します。
        /// </summary>
        /// <param name="parent">このコレクションを所有する Control。</param>
        public ControlCollection(Control parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            Parent = parent;
        }
    }
}
