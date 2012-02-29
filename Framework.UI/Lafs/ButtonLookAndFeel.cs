#region Using

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
        /// Texture2D の描画で着色する色を取得します。
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public ButtonLookAndFeel()
        {
            Color = Color.White;
        }

        // I/F
        public virtual void Draw(Control control, IDrawContext drawContext)
        {
            if (FocusTexture != null && control.Focused)
                drawContext.DrawTexture(new Rect(control.RenderSize), FocusTexture, Color);
        }
    }
}
