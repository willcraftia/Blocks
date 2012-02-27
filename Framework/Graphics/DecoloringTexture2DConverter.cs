#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// A 成分が 0 ではない色を指定の色へ置換する ITexture2DConverter です。
    /// </summary>
    public class DecoloringTexture2DConverter : ITexture2DConverter
    {
        /// <summary>
        /// 置換色。
        /// </summary>
        Color color;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="color">置換色。</param>
        public DecoloringTexture2DConverter(Color color)
        {
            this.color = color;
        }

        public virtual Texture2D Convert(Texture2D texture)
        {
            if (texture == null) throw new ArgumentNullException("texture");

            var colors = new Color[texture.Width * texture.Height];
            texture.GetData(colors);

            for (int i = 0; i < colors.Length; i++)
            {
                var alpha = colors[i].ToVector4().W;
                if (alpha != 0) colors[i] = color;
            }
            var shadowTexture = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height, false, SurfaceFormat.Color);
            shadowTexture.SetData(colors);
            return shadowTexture;
        }
    }
}
