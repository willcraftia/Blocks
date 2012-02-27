#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// 1 枚のテクスチャに纏められたスプライト イメージ、および、
    /// 個々のスプライト イメージの配置場所を管理するクラスです。
    /// </summary>
    public sealed class SpriteSheet
    {
        /// <summary>
        /// スプライト イメージの配置場所を管理する ISpriteSheetTemplate を取得します。
        /// </summary>
        public ISpriteSheetTemplate Template { get; private set; }

        /// <summary>
        /// テクスチャを取得します。
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="texture">テクスチャ。</param>
        public SpriteSheet(ISpriteSheetTemplate template, Texture2D texture)
        {
            if (template == null) throw new ArgumentNullException("template");
            if (texture == null) throw new ArgumentNullException("texture");
            Template = template;
            Texture = texture;
        }
    }
}
