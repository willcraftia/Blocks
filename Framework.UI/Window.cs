#region Using

using System;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Window を表す Control です。
    /// </summary>
    public class Window : Control
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public Window() { }

        /// <summary>
        /// Window を閉じます。
        /// </summary>
        public void Close()
        {
            // 親のリストから削除します。
            Parent.Children.Remove(this);
        }

        protected override void OnMouseButtonPressed(MouseButtons button)
        {
            Parent.Children.MoveToTopMost(this);
        }
    }
}
