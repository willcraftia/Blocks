#region Using

using System;
using System.Collections.Generic;
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
        /// スプライト イメージの配置場所を定義するディクショナリを取得します。
        /// </summary>
        public Dictionary<string, Rectangle> SourceRectangles { get; private set; }

        /// <summary>
        /// テクスチャを取得します。
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="texture">テクスチャ。</param>
        public SpriteSheet(Texture2D texture)
        {
            if (texture == null) throw new ArgumentNullException("texture");
            Texture = texture;
            SourceRectangles = new Dictionary<string, Rectangle>();
        }
    }
}
