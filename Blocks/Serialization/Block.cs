#region Using

using System;
using System.Text;
using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization
{
    /// <summary>
    /// Block を表現するクラスです。
    /// </summary>
    [XmlRoot("B")]
    public sealed class Block
    {
        public const string Extension = ".block";

        /// <summary>
        /// Material のリストを取得または設定します。
        /// </summary>
        [XmlArray("ML")]
        [XmlArrayItem("M")]
        public List<Material> Materials { get; set; }

        /// <summary>
        /// Element のリストを取得または設定します。
        /// </summary>
        [XmlArray("EL")]
        [XmlArrayItem("E")]
        public List<Element> Elements { get; set; }

        public Block()
        {
            Materials = new List<Material>();
            Elements = new List<Element>();
        }

        public static string ResolveFileName(string name)
        {
            return name + Extension;
        }

        #region ToString

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[Materials=[");
            if (Materials != null)
            {
                for (int i = 0; i < Materials.Count; i++)
                {
                    builder.Append(Materials[i]);
                    if (i < Materials.Count - 1) builder.Append(", ");
                }
            }
            builder.Append("], Elements=[");
            if (Elements != null)
            {
                for (int i = 0; i < Elements.Count; i++)
                {
                    builder.Append(Elements[i]);
                    if (i < Elements.Count - 1) builder.Append(", ");
                }
            }
            builder.Append("]");
            return builder.ToString();
        }

        #endregion
    }
}
