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
    }
}
