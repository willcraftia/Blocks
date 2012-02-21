#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Screen で管理する全ての Control のルートとなる Control です。
    /// </summary>
    public sealed class Desktop : Window
    {
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        internal Desktop(Screen screen)
            : base(screen)
        {
            var viewportBounds = screen.GraphicsDevice.Viewport.TitleSafeArea;
            BackgroundColor = Color.CornflowerBlue;
            Margin = new Thickness(viewportBounds.Left, viewportBounds.Top, 0, 0);
            Width = viewportBounds.Width;
            Height = viewportBounds.Height;
        }
    }
}
