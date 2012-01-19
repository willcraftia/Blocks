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
        /// Window をアクティブ化します。
        /// </summary>
        /// <param name="window">アクティブ化する Window。</param>
        internal void ActivateWindow(Window window)
        {
            if (!Children.Contains(window)) throw new InvalidOperationException("Window is the child of another contol.");

            // 最前面へ移動させます。
            Children.MoveToTopMost(window);
        }
    }
}
