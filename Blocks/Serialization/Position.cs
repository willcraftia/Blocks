#region Using

using System;
using System.Runtime.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization
{
    /// <summary>
    /// Block 内グリッドにおける BlockMesh の位置を表すための構造体です。
    /// </summary>
    [DataContract]
    public struct Position : IEquatable<Position>
    {
        /// <summary>
        /// X 座標。
        /// </summary>
        [DataMember]
        public int X;

        /// <summary>
        /// Y 座標。
        /// </summary>
        [DataMember]
        public int Y;

        /// <summary>
        /// Z 座標。
        /// </summary>
        [DataMember]
        public int Z;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="x">X 座標。</param>
        /// <param name="y">Y 座標。</param>
        /// <param name="z">Z 座標。</param>
        public Position(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #region Equatable

        public static bool operator ==(Position c1, Position c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(Position c1, Position c2)
        {
            return !c1.Equals(c2);
        }

        // I/F
        public bool Equals(Position other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return Equals((Position) obj);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            return "[" + X + ", " + Y + ", " + Z + "]";
        }

        #endregion
    }
}
