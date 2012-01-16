#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    public class Overlay : Window
    {
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public Overlay()
        {
            // デフォルト背景色は透明にします。
            BackgroundColor = Color.White * 0.0f;
            // デフォルトは透明度の継承を行いません。
            OpacityInherited = false;
        }

        /// <summary>
        /// Overlay を表示します。
        /// </summary>
        /// <param name="screen"></param>
        public override void Show(Screen screen)
        {
            Margin = screen.Desktop.Margin;
            Width = screen.Desktop.Width;
            Height = screen.Desktop.Height;
            base.Show(screen);
        }
    }
}
