#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    public sealed class WindowSpriteSheetTemplate : ISpriteSheetTemplate
    {
        public static readonly string TitleTopLeft = "TitleTopLeft";
        public static readonly string TitleTop = "TitleTop";
        public static readonly string TitleTopRight = "TitleTopRight";
        public static readonly string TopLeft = "TopLeft";
        public static readonly string Top = "Top";
        public static readonly string TopRight = "TopRight";
        public static readonly string Left = "Left";
        public static readonly string Fill = "Fill";
        public static readonly string Right = "Right";
        public static readonly string BottomLeft = "BottomLeft";
        public static readonly string Bottom = "Bottom";
        public static readonly string BottomRight = "BottomRight";

        /// <summary>
        /// ID と配置場所のマップ。
        /// </summary>
        Dictionary<string, Rectangle> sourceRectangles = new Dictionary<string, Rectangle>();

        // I/F
        public Rectangle this[string id]
        {
            get { return sourceRectangles[id]; }
        }

        /// <summary>
        /// スプライト イメージの幅を取得します。
        /// </summary>
        public int SpriteWidth { get; private set; }

        /// <summary>
        /// スプライト イメージの高さを取得します。
        /// </summary>
        public int SpriteHeight { get; private set; }

        /// <summary>
        /// SpriteSheet がタイトル用スプライト イメージを含むかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (SpriteSheet がタイトル用スプライト イメージを含む場合)、false (それ以外の場合)。
        /// </value>
        public bool ContainsTitle { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="spriteWidth">スプライト イメージの幅。</param>
        /// <param name="spriteHeight">スプライト イメージの高さ。</param>
        /// <param name="containsTitle">
        /// true (SpriteSheet がタイトル用スプライト イメージを含む場合)、false (それ以外の場合)。
        /// </param>
        public WindowSpriteSheetTemplate(int spriteWidth, int spriteHeight, bool containsTitle)
        {
            if (spriteWidth <= 0) throw new ArgumentOutOfRangeException("spriteWidth");
            if (spriteHeight <= 0) throw new ArgumentOutOfRangeException("spriteHeight");
            SpriteWidth = spriteWidth;
            SpriteHeight = spriteHeight;
            ContainsTitle = containsTitle;

            var w = spriteWidth;
            var h = spriteHeight;

            var offsetY = 0;
            if (containsTitle)
            {
                sourceRectangles[TitleTopLeft] = new Rectangle(0, offsetY, w, h);
                sourceRectangles[TitleTop] = new Rectangle(w, offsetY, w, h);
                sourceRectangles[TitleTopRight] = new Rectangle(w * 2, offsetY, w, h);
                offsetY += h;
            }
            sourceRectangles[TopLeft] = new Rectangle(0, offsetY, w, h);
            sourceRectangles[Top] = new Rectangle(w, offsetY, w, h);
            sourceRectangles[TopRight] = new Rectangle(w * 2, offsetY, w, h);
            offsetY += h;
            sourceRectangles[Left] = new Rectangle(0, offsetY, w, h);
            sourceRectangles[Fill] = new Rectangle(w, offsetY, w, h);
            sourceRectangles[Right] = new Rectangle(w * 2, offsetY, w, h);
            offsetY += h;
            sourceRectangles[BottomLeft] = new Rectangle(0, offsetY, w, h);
            sourceRectangles[Bottom] = new Rectangle(w, offsetY, w, h);
            sourceRectangles[BottomRight] = new Rectangle(w * 2, offsetY, w, h);
        }
    }
}
