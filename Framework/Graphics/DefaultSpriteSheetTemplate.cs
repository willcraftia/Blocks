#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    public class DefaultSpriteSheetTemplate : ISpriteSheetTemplate
    {
        // I/F
        public Rectangle this[string id]
        {
            get { return SourceRectangles[id]; }
        }

        /// <summary>
        /// ID と配置場所のマップを取得します。
        /// </summary>
        public Dictionary<string, Rectangle> SourceRectangles { get; private set; }

        public DefaultSpriteSheetTemplate()
        {
            SourceRectangles = new Dictionary<string, Rectangle>();
        }
    }
}
