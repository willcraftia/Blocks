#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public class ButtonControl : Control
    {
        public bool MouseHovering { get; private set; }

        protected override void OnMouseEntered()
        {
            MouseHovering = true;
        }

        protected override void OnMouseLeft()
        {
            MouseHovering = false;
        }
    }
}
