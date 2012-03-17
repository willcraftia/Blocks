#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// BlockMesh のエフェクト情報を管理するクラスです。
    /// このクラスは、CPU 上で情報を管理します。
    /// </summary>
    public sealed class InterBlockEffect
    {
        /// <summary>
        /// Diffuse 色。
        /// </summary>
        public Vector3 DiffuseColor;

        /// <summary>
        /// Emissive 色。
        /// </summary>
        public Vector3 EmissiveColor;

        /// <summary>
        /// Specular 色。
        /// </summary>
        public Vector3 SpecularColor;

        /// <summary>
        /// Specular 係数。
        /// </summary>
        public float SpecularPower;
    }
}
