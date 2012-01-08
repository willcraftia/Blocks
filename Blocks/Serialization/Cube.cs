#region Using

using System;
using System.Runtime.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization
{
    [DataContract]
    public sealed class Cube
    {
        [DataMember]
        public CubeColor Color;

        [DataMember]
        public CubePosition Position;

        #region ToString

        public override string ToString()
        {
            return "[Color=" + Color + ", Position=" + Position + "]";
        }

        #endregion
    }
}
