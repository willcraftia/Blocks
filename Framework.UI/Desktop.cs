#region Using

using System;
using Microsoft.Xna.Framework;

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
        internal Desktop() { }

        /// <summary>
        /// Window をアクティブ化します。
        /// </summary>
        /// <param name="window">アクティブ化する Window。</param>
        internal void ActivateWindow(Window window)
        {
            if (!Children.Contains(window)) throw new InvalidOperationException("Window is the child of another contol.");

            ActivatedControl = window;

            // 最前面へ移動させます。
            Children.MoveToTopMost(window);
        }

        /// <summary>
        /// Window を非アクティブ化します。
        /// アクティブではない Window を指定した場合には何もしません。
        /// </summary>
        /// <param name="window">非アクティブ化する Window。</param>
        internal void DeactivateWindow(Window window)
        {
            if (ActivatedControl == window) ActivatedControl = null;
        }
    }
}
