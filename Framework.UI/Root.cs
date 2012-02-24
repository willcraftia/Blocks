#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Control 木構造のルート ノードです。
    /// </summary>
    internal sealed class Root : Control
    {
        /// <summary>
        /// Desktop を取得します。
        /// </summary>
        internal Desktop Desktop { get; private set; }

        /// <summary>
        /// 管理している Window のコレクションを取得します。
        /// ただし、Desktop はこのコレクションに含まれません。
        /// </summary>
        internal WindowCollection Windows { get; private set; }

        /// <summary>
        /// Windows.Count + 1 を返します。
        /// </summary>
        protected override int ChildrenCount
        {
            get { return Windows.Count + 1; }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        internal Root(Screen screen)
            : base(screen)
        {
            Desktop = new Desktop(screen);
            // Desktop を子として関連付けます。
            AddChild(Desktop);

            Windows = new WindowCollection(this);
        }

        /// <summary>
        /// index = 0 は Desktop プロパティを指し示します。
        /// また、Windows プロパティにある Window は、index + 1 で指し示されます。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override Control GetChild(int index)
        {
            if (index < 0 || ChildrenCount <= index)
                throw new ArgumentOutOfRangeException("index");

            if (index == 0) return Desktop;
            return Windows[index - 1];
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
        /// レイアウトを更新します。
        /// </summary>
        internal void UpdateLayout()
        {
            // 測定を開始します。
            Measure(new Size(Desktop.Width, Desktop.Height));

            // 配置を開始します。
            var margin = Desktop.Margin;
            Arrange(new Rect(margin.Left, margin.Top, Desktop.Width, Desktop.Height));
        }

        /// <summary>
        /// 最前面の Window を取得します。
        /// 最前面の Window が存在しない場合には null を返します。
        /// </summary>
        /// <remarks>
        /// 最前面の Window が必ずしもアクティブ Window であるとは限りません。
        /// </remarks>
        /// <returns>
        /// 最前面の Window。
        /// 最前面の Window が存在しない場合には null。
        /// </returns>
        internal Window GetTopMostWindow()
        {
            for (int i = Windows.Count - 1; 0 <= i; i--)
            {
                var window = Windows[i];
                if (window.Visible) return window;
            }

            if (Desktop.Visible) return Desktop;
            return null;
        }

        /// <summary>
        /// アクティブ Window を取得します。
        /// アクティブ Window が存在しない場合には null を返します。
        /// </summary>
        /// <returns>
        /// アクティブ Window。
        /// アクティブ Window が存在しない場合には null。
        /// </returns>
        internal Window GetActiveWindow()
        {
            if (Desktop.Active) return Desktop;

            foreach (var window in Windows)
            {
                if (window.Active) return window;
            }

            return null;
        }

        /// <summary>
        /// 指定の Window を表示します。
        /// </summary>
        /// <param name="window">表示する Window。</param>
        internal void ShowWindow(Window window)
        {
            if (window == null) throw new ArgumentNullException("window");

            // 未登録ならば追加します。
            if (!(window is Desktop) && !Windows.Contains(window)) Windows.Add(window);

            // アクティブ化します。
            ActivateWindow(window);
        }

        /// <summary>
        /// Window をアクティブ化します。
        /// Window の Visible プロパティが true に設定されます。
        /// </summary>
        /// <param name="window">アクティブ化する Window。</param>
        internal void ActivateWindow(Window window)
        {
            if (window == null) throw new ArgumentNullException("window");
            EnsureWindow(window);

            // 既にアクティブな場合には処理を終えます。
            if (window.Active) return;

            // アクティブな Window を非アクティブ化します。
            var activeWindow = GetActiveWindow();
            if (activeWindow != null) activeWindow.Active = false;

            // 通常 Window ならば最前面へ移動させます。
            if (!(window is Desktop))
            {
                Windows.Remove(window);
                Windows.Add(window);
            }

            window.Active = true;
            window.Visible = true;

            // 論理フォーカスにフォーカスを設定します。
            Screen.FocusedControl = window.FocusScope.FocusedControl;
        }

        /// <summary>
        /// 指定の Window を非表示にします。
        /// </summary>
        /// <param name="window">非表示にする Window。</param>
        internal void HideWindow(Window window)
        {
            if (window == null) throw new ArgumentNullException("window");
            EnsureWindow(window);

            window.Visible = false;

            // 最前面の Window をアクティブ化します。
            ActivateTopMostWindow();
        }

        /// <summary>
        /// 指定の Window を閉じます。
        /// </summary>
        /// <remarks>
        /// Desktop を指定した場合には例外が発生します (Desktop を閉じることはできません)。
        /// </remarks>
        /// <param name="window">閉じる Window。</param>
        internal void CloseWindow(Window window)
        {
            if (window == null) throw new ArgumentNullException("window");
            EnsureWindow(window);

            if (window is Desktop)
                throw new InvalidOperationException("Desktop can never be closed.");

            // 非表示にしてから削除します。
            HideWindow(window);
            Windows.Remove(window);
        }

        /// <summary>
        /// 最前面の Window をアクティブ化します。
        /// </summary>
        void ActivateTopMostWindow()
        {
            var topMostWindow = GetTopMostWindow();
            if (topMostWindow != null) ActivateWindow(topMostWindow);
        }

        /// <summary>
        /// 指定の Window が追加済みではない場合に例外を発生させます。
        /// </summary>
        /// <param name="window">Window。</param>
        void EnsureWindow(Window window)
        {
            if (Desktop != window && !Windows.Contains(window))
                throw new InvalidOperationException(
                    "The specified window could not be found in this screen.");
        }
    }
}
