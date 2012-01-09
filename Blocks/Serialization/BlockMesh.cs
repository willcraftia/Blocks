#region Using

using System;
using System.Runtime.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization
{
    /// <summary>
    /// Block を構成する Mesh を表すクラスです。
    /// </summary>
    [DataContract]
    public sealed class BlockMesh
    {
        /// <summary>
        /// Block 内グリッドにおける位置。
        /// </summary>
        [DataMember]
        public Position Position;

        /// <summary>
        /// 参照する Material のインデックスを取得または設定します。
        /// </summary>
        [DataMember]
        public int MaterialIndex { get; set; }

        #region ToString

        public override string ToString()
        {
            return "[Position=" + Position + ", MaterialIndex=" + MaterialIndex + "]";
        }

        #endregion
    }
}
