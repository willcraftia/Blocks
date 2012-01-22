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
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        internal Desktop(Screen screen) : base(screen)  { }

        /// <summary>
        /// アクティブ Window を取得します。
        /// Window が存在しない場合には null を返します。
        /// </summary>
        /// <returns>アクティブ Window。</returns>
        public Window GetActiveWindow()
        {
            foreach (var control in Children)
            {
                var window = control as Window;
                if (window != null && window.Active) return window;
            }

            return null;
        }

        /// <summary>
        /// 指定の Window を表示します。
        /// </summary>
        /// <param name="window">表示する Window。</param>
        internal void ShowWindow(Window window)
        {
            if (Children.Contains(window)) throw new InvalidOperationException("Window is already the child of desktop.");

            Children.Add(window);

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
            if (!Children.Contains(window)) throw new InvalidOperationException("Window is the child of another contol.");

            Children.Remove(window);

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
            if (!Children.Contains(window)) throw new InvalidOperationException("Window is the child of another contol.");

            // 既にアクティブな場合には処理を終えます。
            if (window.Active) return;

            // 最前面へ移動させます。
            Children.Remove(window);
            Children.Add(window);

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
            for (int i = Children.Count - 1; 0 <= i; i--)
            {
                var window = Children[i] as Window;
                if (window != null) return window;
            }

            return null;
        }
    }
}
