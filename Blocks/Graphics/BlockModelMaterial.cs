#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    /// <summary>
    /// BlockModel の Material を表すクラスです。
    /// </summary>
    public sealed class BlockModelMaterial
    {
        /// <summary>
        /// Diffuse 色を取得します。
        /// </summary>
        public Vector3 DiffuseColor { get; internal set; }

        /// <summary>
        /// Emissive 色を取得します。
        /// </summary>
        public Vector3 EmissiveColor { get; internal set; }

        /// <summary>
        /// Specular 色を取得します。
        /// </summary>
        public Vector3 SpecularColor { get; internal set; }

        /// <summary>
        /// Specular 係数を取得します。
        /// </summary>
        public float SpecularPower { get; internal set; }

        /// <summary>
        /// Alpha を取得します。
        /// </summary>
        public float Alpha { get; internal set; }
    }
}
