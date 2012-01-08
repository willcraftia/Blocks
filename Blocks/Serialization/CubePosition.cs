#region Using

using System;
using System.Runtime.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization
{
    [DataContract]
    public struct CubePosition : IEquatable<CubePosition>
    {
        [DataMember]
        public int X;

        [DataMember]
        public int Y;

        [DataMember]
        public int Z;

        public CubePosition(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #region Equatable

        public static bool operator ==(CubePosition c1, CubePosition c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(CubePosition c1, CubePosition c2)
        {
            return !c1.Equals(c2);
        }

        // I/F
        public bool Equals(CubePosition other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return Equals((CubePosition) obj);
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
