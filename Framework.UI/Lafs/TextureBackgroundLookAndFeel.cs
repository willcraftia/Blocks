#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs
{
    /// <summary>
    /// Control 領域に指定の Texture2D を描画する ILookAndFeel です。
    /// </summary>
    public class TextureBackgroundLookAndFeel : ILookAndFeel
    {
        /// <summary>
        /// Control 領域に描画する Texture2D を取得します。
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Texture2D の描画で着色する色を取得します。
        /// デフォルトは Color.White です。
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public TextureBackgroundLookAndFeel()
        {
            Color = Color.White;
        }

        // I/F
        public virtual void Draw(Control control, IDrawContext drawContext)
        {
            if (Texture != null)
                drawContext.DrawTexture(new Rect(control.RenderSize), Texture, Color);
        }
    }
}
