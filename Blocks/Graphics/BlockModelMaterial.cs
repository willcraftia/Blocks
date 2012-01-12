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
        /// Diffuse 色を取得または設定します。
        /// </summary>
        public Vector3 DiffuseColor { get; set; }

        /// <summary>
        /// Emissive 色を取得または設定します。
        /// </summary>
        public Vector3 EmissiveColor { get; set; }

        /// <summary>
        /// Specular 色を取得または設定します。
        /// </summary>
        public Vector3 SpecularColor { get; set; }

        /// <summary>
        /// Specular 係数を取得または設定します。
        /// </summary>
        public float SpecularPower { get; set; }
    }
}
