#region Using

using System;
using System.Runtime.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization
{
    [DataContract]
    public struct CubeColor
    {
        [DataMember]
        public byte R;

        [DataMember]
        public byte G;

        [DataMember]
        public byte B;

        [DataMember]
        public byte A;

        public CubeColor(byte r, byte g, byte b) : this(r, g, b, 255) { }

        public CubeColor(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        #region ToString

        public override string ToString()
        {
            return "[" + R + ", " + G + ", " + B + ", " + A + "]";
        }

        #endregion
    }
}
