#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs
{
    public class ListBoxItemLookAndFeel : ILookAndFeel
    {
        /// <summary>
        /// Control.Focused プロパティが true の場合に描画する Texture2D を取得または設定します。
        /// </summary>
        public Texture2D FocusTexture { get; set; }

        /// <summary>
        /// Control.MouseOver プロパティが true の場合に描画する Texture2D を取得または設定します。
        /// </summary>
        public Texture2D MouseOverTexture { get; set; }

        /// <summary>
        /// ListBoxItem.IsSelected プロパティが true の場合に描画する Texture2D を取得または設定します。
        /// </summary>
        public Texture2D SelectionTexture { get; set; }

        /// <summary>
        /// FocusTexture の描画で着色する色を取得します。
        /// このプロパティは、FocusTexture プロパティが null の場合には、
        /// Control.Focused プロパティが true の場合に描画する背景色として利用されます。
        /// </summary>
        public Color FocusColor { get; set; }

        /// <summary>
        /// MouseOverTexture の描画で着色する色を取得します。
        /// このプロパティは、MouseOverTexture プロパティが null の場合には、
        /// Control.MouseOver プロパティが true の場合に描画する背景色として利用されます。
        /// </summary>
        public Color MouseOverColor { get; set; }

        /// <summary>
        /// SelectionTexture の描画で着色する色を取得します。
        /// このプロパティは、SelectionTexture プロパティが null の場合には、
        /// ListBoxItem.IsSelected プロパティが true の場合に描画する背景色として利用されます。
        /// </summary>
        public Color SelectionColor { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public ListBoxItemLookAndFeel()
        {
            FocusColor = Color.White;
            MouseOverColor = Color.White;
        }

        // I/F
        public virtual void Draw(Control control, IDrawContext drawContext)
        {
            if (control.Focused)
            {
                if (FocusTexture != null)
                {
                    drawContext.DrawTexture(new Rect(control.RenderSize), FocusTexture, FocusColor);
                }
                else
                {
                    drawContext.DrawRectangle(new Rect(control.RenderSize), FocusColor);
                }
            }

            if (control.MouseOver)
            {
                if (MouseOverTexture != null)
                {
                    drawContext.DrawTexture(new Rect(control.RenderSize), MouseOverTexture, MouseOverColor);
                }
                else
                {
                    drawContext.DrawRectangle(new Rect(control.RenderSize), MouseOverColor);
                }
            }

            var listBoxItem = control as ListBoxItem;
            if (listBoxItem != null && listBoxItem.IsSelected)
            {
                if (SelectionTexture != null)
                {
                    drawContext.DrawTexture(new Rect(control.RenderSize), SelectionTexture, SelectionColor);
                }
                else
                {
                    drawContext.DrawRectangle(new Rect(control.RenderSize), SelectionColor);
                }
            }
        }
    }
}
