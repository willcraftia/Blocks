﻿#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs
{
    public class ButtonLookAndFeel : ILookAndFeel
    {
        /// <summary>
        /// Control にフォーカスが設定されている場合に描画する Texture2D を取得または設定します。
        /// </summary>
        public Texture2D FocusTexture { get; set; }

        /// <summary>
        /// Control にマウスがある場合に描画する Texture2D を取得または設定します。
        /// </summary>
        public Texture2D MouseOverTexture { get; set; }

        /// <summary>
        /// FocusTexture の描画で着色する色を取得します。
        /// このプロパティは、FocusTexture プロパティが null の場合には、
        /// Control にフォーカスが設定されている場合に描画する背景色として利用されます。
        /// </summary>
        public Color FocusColor { get; set; }

        /// <summary>
        /// MouseOverTexture の描画で着色する色を取得します。
        /// このプロパティは、MouseOverTexture プロパティが null の場合には、
        /// Control にマウスがある場合に描画する背景色として利用されます。
        /// </summary>
        public Color MouseOverColor { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public ButtonLookAndFeel()
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
        }
    }
}
