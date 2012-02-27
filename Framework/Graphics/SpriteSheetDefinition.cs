#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// SpriteSheet の定義です。
    /// </summary>
    public class SpriteSheetDefinition
    {
        /// <summary>
        /// SpriteSheet に設定する ISpriteSheetTemplate を取得します。
        /// </summary>
        public ISpriteSheetTemplate Template { get; private set; }

        /// <summary>
        /// SpriteSheet に設定する Texture2D の位置を取得します。
        /// </summary>
        public string Location { get; private set; }

        /// <summary>
        /// SpriteSheet に Texture2D を設定する前に適用する ITexture2DConverter を取得します。
        /// null の場合、Location プロパティが示す Texture2D がそのまま SpriteSheet に設定されます。
        /// </summary>
        public ITexture2DConverter Converter { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// Converter プロパティは null に設定されます。
        /// </summary>
        /// <param name="template">SpriteSheet に設定する ISpriteSheetTemplate。</param>
        /// <param name="location">SpriteSheet に設定する Texture2D の位置。</param>
        public SpriteSheetDefinition(ISpriteSheetTemplate template, string location)
            : this(template, location, null)
        {
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="template">SpriteSheet に設定する ISpriteSheetTemplate。</param>
        /// <param name="location">SpriteSheet に設定する Texture2D の位置。</param>
        /// <param name="converter">SpriteSheet に Texture2D を設定する前に適用する ITexture2DConverter。</param>
        public SpriteSheetDefinition(ISpriteSheetTemplate template, string location, ITexture2DConverter converter)
        {
            if (template == null) throw new ArgumentNullException("template");
            if (location == null) throw new ArgumentNullException("location");
            Template = template;
            Location = location;
            Converter = converter;
        }
    }
}
