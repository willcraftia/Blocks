#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    /// <summary>
    /// Desktop 全体を覆う Window です。
    /// </summary>
    public class Overlay : Window
    {
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        public Overlay(Screen screen)
            : base(screen)
        {
            Opacity = 0;
            BackgroundColor = Color.Black;
        }

        /// <summary>
        /// Overlay を表示します。
        /// </summary>
        public override void Show()
        {
            Margin = Screen.Root.Desktop.Margin;
            Width = Screen.Root.Desktop.Width;
            Height = Screen.Root.Desktop.Height;
            base.Show();
        }
    }
}
