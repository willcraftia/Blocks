#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    /// <summary>
    /// BlockMesh の Effect を制御するクラスのインタフェースです。
    /// </summary>
    public interface IBlockEffect : IEffectMatrices, IEffectLights, IEffectFog
    {
        /// <summary>
        /// Diffuse 色を取得または設定します。
        /// </summary>
        Vector3 DiffuseColor { get; set; }

        /// <summary>
        /// Alpha 値を取得または設定します。
        /// </summary>
        float Alpha { get; set; }

        /// <summary>
        /// Emissive 色を取得または設定します。
        /// </summary>
        Vector3 EmissiveColor { get; set; }

        /// <summary>
        /// Specular 色を取得または設定します。
        /// </summary>
        Vector3 SpecularColor { get; set; }

        /// <summary>
        /// Specular 係数を取得または設定します。
        /// </summary>
        float SpecularPower { get; set; }

        /// <summary>
        /// Pass を取得または設定します。
        /// </summary>
        /// <remarks>
        /// IBlockEffect は単一の Technique およびその単一の Pass のみに対応します。
        /// </remarks>
        EffectPass Pass { get; }
    }
}
