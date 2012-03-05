#region Using

using System;
using System.Collections.Generic;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// 複数の ILookAndFeelSource の 1 つを選択して実体とする ILookAndFeelSource です。
    /// </summary>
    public class SelectableLookAndFeelSource : ILookAndFeelSource
    {
        int selectedIndex = -1;

        /// <summary>
        /// 選択可能な ILookAndFeelSource のリストを取得します。
        /// </summary>
        public List<ILookAndFeelSource> Items { get; private set; }

        /// <summary>
        /// 選択している ILookAndFeelSource のインデックスを取得または設定します。
        /// </summary>
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

        /// <summary>
        /// 選択している ILookAndFeelSource を取得します。
        /// Items プロパティが空の場合、あるいは、
        /// SelectedIndex プロパティが -1 の場合は null を返します。
        /// </summary>
        public ILookAndFeelSource SelectedItem
        {
            get
            {
                if (selectedIndex == -1 || Items.Count == 0) return null;
                return selectedIndex < Items.Count ? Items[selectedIndex] : Items[Items.Count - 1];
            }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public SelectableLookAndFeelSource()
        {
            Items = new List<ILookAndFeelSource>();
        }

        // I/F
        public ILookAndFeel GetLookAndFeel(Control control)
        {
            if (SelectedItem == null) return null;
            return SelectedItem.GetLookAndFeel(control);
        }
    }
}
