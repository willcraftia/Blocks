#region Using

using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization
{
    [DataContract]
    public sealed class Block
    {
        [DataMember]
        public List<Cube> Cubes { get; set; }

        #region ToString

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[Cubes=[");
            if (Cubes != null)
            {
                for (int i = 0; i < Cubes.Count; i++)
                {
                    builder.Append(Cubes[i]);
                    if (i < Cubes.Count - 1) builder.Append(", ");
                }
            }
            builder.Append("]");
            return builder.ToString();
        }

        #endregion
    }
}
