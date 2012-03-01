#region Using

using System;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class FadeOverlay : Overlay
    {
        public FloatLerpAnimation OpacityAnimation { get; private set; }

        public FadeOverlay(Screen screen)
            : base(screen)
        {
            OpacityAnimation = new FloatLerpAnimation
            {
                Action = (current) => { Opacity = current; },
                To = 1
            };
            Animations.Add(OpacityAnimation);
        }

        public override void Show()
        {
            Opacity = OpacityAnimation.From;
            OpacityAnimation.Enabled = true;

            base.Show();
        }
    }
}
