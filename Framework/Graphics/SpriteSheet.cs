#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// 1 枚の Texture2D に纏められたスプライトを管理するクラスです。
    /// </summary>
    public sealed class SpriteSheet
    {
        /// <summary>
        /// ISpriteSheetTemplate を取得します。
        /// </summary>
        public ISpriteSheetTemplate Template { get; private set; }

        /// <summary>
        /// Texture2D を取得します。
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="template">ISpriteSheetTemplate。</param>
        /// <param name="texture">Texture2D。</param>
        public SpriteSheet(ISpriteSheetTemplate template, Texture2D texture)
        {
            if (template == null) throw new ArgumentNullException("template");
            if (texture == null) throw new ArgumentNullException("texture");
            Template = template;
            Texture = texture;
        }
    }
}
