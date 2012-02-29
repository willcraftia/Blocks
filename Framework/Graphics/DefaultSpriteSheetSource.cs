#region Using

using System;
using System.Collections.Generic;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// ISpriteSheetSource のデフォルト実装です。
    /// </summary>
    public class DefaultSpriteSheetSource : ISpriteSheetSource
    {
        /// <summary>
        /// SpriteSheet の ID をキーに SpriteSheet を値とするマップを取得します。
        /// </summary>
        public Dictionary<string, SpriteSheet> SpriteSheetMap { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public DefaultSpriteSheetSource()
        {
            SpriteSheetMap = new Dictionary<string, SpriteSheet>();
        }

        // I/F
        public SpriteSheet GetSpriteSheet(string id)
        {
            if (id == null) throw new ArgumentNullException("id");

            SpriteSheet spriteSheet;
            SpriteSheetMap.TryGetValue(id, out spriteSheet);
            return spriteSheet;
        }
    }
}
