#region Using

using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization
{
    /// <summary>
    /// Block を表現するクラスです。
    /// </summary>
    public sealed class Block
    {
        /// <summary>
        /// Material のリストを取得または設定します。
        /// </summary>
        public List<Material> Materials { get; set; }

        /// <summary>
        /// Element のリストを取得または設定します。
        /// </summary>
        public List<Element> Elements { get; set; }

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
