#region Using

using System;
using System.Runtime.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization
{
    /// <summary>
    /// BlockMesh の Material を表すクラスです。
    /// </summary>
    [DataContract]
    public sealed class Material
    {
        /// <summary>
        /// Diffuse 色。
        /// </summary>
        [DataMember]
        public MaterialColor DiffuseColor { get; set; }

        /// <summary>
        /// Emissive 色。
        /// </summary>
        [DataMember]
        public MaterialColor EmissiveColor { get; set; }

        /// <summary>
        /// Specular 色。
        /// </summary>
        [DataMember]
        public MaterialColor SpecularColor { get; set; }

        /// <summary>
        /// Specular 係数。
        /// </summary>
        [DataMember]
        public float SpecularPower { get; set; }

        /// <summary>
        /// Alpha 値。
        /// </summary>
        [DataMember]
        public float Alpha { get; set; }

        #region ToString

        public override string ToString()
        {
            return "[DiffuseColor=" + DiffuseColor +
                ", EmissiveColor=" + EmissiveColor +
                ", SpecularColor=" + SpecularColor +
                ", SpecularPower=" + SpecularPower + "]";
        }

        #endregion
    }
}
