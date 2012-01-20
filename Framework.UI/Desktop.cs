#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Screen で管理する Control のルートとなる Control です。
    /// </summary>
    public sealed class Desktop : Control
    {
        /// <summary>
        /// アクティブ Window を取得します。
        /// Window が存在しない場合、あるいは、非 Window が最前面にある場合は null を返します。
        /// </summary>
        public Window ActiveWindow
        {
            get
            {
                if (Children.Count == 0) return null;
                return Children[Children.Count - 1] as Window;
            }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        internal Desktop(Screen screen) : base(screen)  { }

        /// <summary>
        /// 指定の Window を表示します。
        /// </summary>
        /// <param name="window">表示する Window。</param>
        internal void ShowWindow(Window window)
        {
            if (Children.Contains(window)) throw new InvalidOperationException("Window is already the child of desktop.");

            var previousActiveWindow = ActiveWindow;

            Children.Add(window);

            if (previousActiveWindow != null) previousActiveWindow.OnDeactivated();
            window.OnActivated();
        }

        /// <summary>
        /// 指定の Window を閉じます。
        /// </summary>
        /// <param name="window">閉じる Window。</param>
        internal void CloseWindow(Window window)
        {
            if (!Children.Contains(window)) throw new InvalidOperationException("Window is the child of another contol.");

            Children.Remove(window);

            window.OnDeactivated();

            var activeWindow = ActiveWindow;
            if (activeWindow != null) activeWindow.OnActivated();
        }

        /// <summary>
        /// Window をアクティブ化します。
        /// </summary>
        /// <param name="window">アクティブ化する Window。</param>
        internal void ActivateWindow(Window window)
        {
            if (!Children.Contains(window)) throw new InvalidOperationException("Window is the child of another contol.");

            var previousActiveWindow = ActiveWindow;

            // 既にアクティブ化されている場合はスキップします。
            if (previousActiveWindow == window) return;

            Children.Remove(window);
            Children.Add(window);

            if (previousActiveWindow != null) previousActiveWindow.OnDeactivated();
            window.OnActivated();
        }
    }
}
