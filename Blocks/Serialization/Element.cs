#region Using

using System;
using System.Runtime.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization
{
    /// <summary>
    /// Block の最小構成要素を表すクラスです。
    /// </summary>
    public sealed class Element
    {
        /// <summary>
        /// グリッド内での位置を取得または設定します。
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// 参照する Material のインデックスを取得または設定します。
        /// </summary>
        public int MaterialIndex { get; set; }

        #region ToString

        public override string ToString()
        {
            return "[Position=" + Position + ", MaterialIndex=" + MaterialIndex + "]";
        }

        #endregion
    }
}
