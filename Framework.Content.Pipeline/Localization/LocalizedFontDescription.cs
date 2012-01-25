#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.Content.Pipeline.Localization
{
    /// <summary>
    /// .resx ファイルの定義を含む .spritefont を表すクラスです。
    /// </summary>
    public sealed class LocalizedFontDescription : FontDescription
    {
        /// <summary>
        /// .resx ファイルのリストを取得します。
        /// </summary>
        [ContentSerializer(Optional = true, CollectionItemName = "Resx")]
        public List<string> ResourceFiles { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public LocalizedFontDescription()
            : base("Arial", 14, 0)
        {
            ResourceFiles = new List<string>();
        }
    }
}
