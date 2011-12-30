#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    public class Overlay : Control
    {
        /// <summary>
        /// Overlay が閉じる前に発生します。
        /// </summary>
        public event EventHandler Closing;

        /// <summary>
        /// Overlay が閉じた後に発生します。
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public Overlay(Screen screen)
            : base(screen)
        {
            // デフォルト背景色は透明にします。
            BackgroundColor = Color.White * 0.0f;
        }

        /// <summary>
        /// Overlay を表示します。
        /// </summary>
        /// <param name="screen"></param>
        public void Show(Screen screen)
        {
            Margin = screen.Desktop.Margin;
            Width = screen.Desktop.Width;
            Height = screen.Desktop.Height;
            screen.Desktop.Children.Add(this);
        }

        /// <summary>
        /// Overlay を閉じます。
        /// </summary>
        public void Close()
        {
            // Closing イベントを発生させます。
            RaiseClosing();
            // Screen から登録を解除します。
            Screen.Desktop.Children.Remove(this);
            // Closed イベントを発生させます。
            RaiseClosed();
        }

        /// <summary>
        /// Closing イベントを発生させます。
        /// </summary>
        void RaiseClosing()
        {
            if (Closing != null) Closing(this, EventArgs.Empty);
        }

        /// <summary>
        /// Closed イベントを発生させます。
        /// </summary>
        void RaiseClosed()
        {
            if (Closed != null) Closed(this, EventArgs.Empty);
        }
    }
}
