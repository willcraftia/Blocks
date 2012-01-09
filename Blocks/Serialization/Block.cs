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
    /// Block は、複数の BlockMesh で構成されます。
    /// </summary>
    [DataContract]
    public sealed class Block
    {
        /// <summary>
        /// BlockMesh が参照する Material のリストを取得または設定します。
        /// </summary>
        [DataMember]
        public List<Material> Materials { get; set; }

        /// <summary>
        /// Block を構成する BlockMesh のリストを取得または設定します。
        /// </summary>
        [DataMember]
        public List<BlockMesh> Meshes { get; set; }

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
            builder.Append("], Meshes=[");
            if (Meshes != null)
            {
                for (int i = 0; i < Meshes.Count; i++)
                {
                    builder.Append(Meshes[i]);
                    if (i < Meshes.Count - 1) builder.Append(", ");
                }
            }
            builder.Append("]");
            return builder.ToString();
        }

        #endregion
    }
}
