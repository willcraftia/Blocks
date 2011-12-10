#region Using

using System;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// ウィンドウを表す Control です。
    /// </summary>
    public class Window : Control
    {
        protected override void OnMouseButtonPressed(MouseButtons button)
        {
            Parent.Children.MoveToTopMost(this);
        }
    }
}
