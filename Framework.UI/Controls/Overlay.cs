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
        /// <param name="screen">Screen。</param>
        public Overlay(Screen screen)
            : base(screen)
        {
            // デフォルト背景色は透明にします。
            BackgroundColor = Color.White * 0.0f;
            // デフォルトは透明度の継承を行いません。
            OpacityInherited = false;
        }

        /// <summary>
        /// Overlay を表示します。
        /// </summary>
        public override void Show()
        {
            Margin = Screen.Desktop.Margin;
            Width = Screen.Desktop.Width;
            Height = Screen.Desktop.Height;
            base.Show();
        }
    }
}
