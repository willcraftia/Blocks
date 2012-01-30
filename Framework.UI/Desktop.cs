#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Screen で管理する Control のルートとなる Control です。
    /// </summary>
    public sealed class Desktop : ContentControl
    {
        #region InternalWindowCollection

        class InternalWindowCollection : WindowCollection
        {
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

        public WindowCollection Windows { get; private set; }

        public override int ChildrenCount
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

        /// <summary>
        /// アクティブ Window を取得します。
        /// Window が存在しない場合には null を返します。
        /// </summary>
        /// <returns>アクティブ Window。</returns>
        public Window GetActiveWindow()
        {
            foreach (var window in Windows)
            {
                if (window.Active) return window;
            }

            return null;
        }

        protected override Control GetChild(int index)
        {
            if (index < 0 || ChildrenCount <= index) throw new ArgumentOutOfRangeException("index");

            if (Content == null) return Windows[index];
            return (index == 0)  ? Content : Windows[index - 1];
        }

        internal void AddChildInternal(Control child)
        {
            AddChild(child);
        }

        internal void RemoveChildInternal(Control child)
        {
            RemoveChild(child);
        }

        /// <summary>
        /// 指定の Window を表示します。
        /// </summary>
        /// <param name="window">表示する Window。</param>
        internal void ShowWindow(Window window)
        {
            if (Windows.Contains(window)) throw new InvalidOperationException("Window is already the child of desktop.");

            Windows.Add(window);

            // Active プロパティの変更順は、Window でのフォーカス状態の記憶と密に関係します。
            var previousActiveWindow = GetActiveWindow();
            if (previousActiveWindow != null) previousActiveWindow.Active = false;
            window.Active = true;
        }

        /// <summary>
        /// 指定の Window を閉じます。
        /// </summary>
        /// <param name="window">閉じる Window。</param>
        internal void CloseWindow(Window window)
        {
            if (window == null) throw new ArgumentNullException("window");
            if (!Windows.Contains(window)) throw new InvalidOperationException("Window is the child of another contol.");

            Windows.Remove(window);

            if (window.Active)
            {
                window.Active = false;
                var newActiveWindow = GetTopMostWindow();
                if (newActiveWindow != null) newActiveWindow.Active = true;
            }
        }

        /// <summary>
        /// Window をアクティブ化します。
        /// </summary>
        /// <param name="window">アクティブ化する Window。</param>
        internal void ActivateWindow(Window window)
        {
            if (window == null) throw new ArgumentNullException("window");
            if (!Windows.Contains(window)) throw new InvalidOperationException("Window is the child of another contol.");

            // 既にアクティブな場合には処理を終えます。
            if (window.Active) return;

            // 最前面へ移動させます。
            Windows.Remove(window);
            Windows.Add(window);

            // Active プロパティの変更順は、Window でのフォーカス状態の記憶と密に関係します。
            var previousActiveWindow = GetActiveWindow();
            if (previousActiveWindow != null) previousActiveWindow.Active = false;
            window.Active = true;
        }

        /// <summary>
        /// 最前面の Window を取得します。
        /// Window が存在しない場合には null を返します。
        /// </summary>
        /// <returns></returns>
        Window GetTopMostWindow()
        {
            for (int i = Windows.Count - 1; 0 <= i; i--)
            {
                var window = Windows[i];
                if (window != null) return window;
            }

            return null;
        }
    }
}
