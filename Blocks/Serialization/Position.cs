#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization
{
    /// <summary>
    /// Block 内グリッドにおける BlockMesh の位置を表すための構造体です。
    /// </summary>
    public struct Position : IEquatable<Position>
    {
        /// <summary>
        /// X 座標。
        /// </summary>
        public int X;

        /// <summary>
        /// Y 座標。
        /// </summary>
        public int Y;

        /// <summary>
        /// Z 座標。
        /// </summary>
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

        /// <summary>
        /// Vector3 へ変換します。
        /// </summary>
        /// <returns>Vector3。</returns>
        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }

        #region Equatable

        public static bool operator ==(Position p1, Position p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(Position p1, Position p2)
        {
            return !p1.Equals(p2);
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
