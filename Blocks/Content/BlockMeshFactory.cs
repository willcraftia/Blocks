#region Using

using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Blocks.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// CPU 上で頂点情報とエフェクト情報を管理する InterBlockMesh から、
    /// グラフィックス リソースとしての BlockMesh を生成するクラスです。
    /// </summary>
    public sealed class BlockMeshFactory
    {
        /// <summary>
        /// GraphicsDevice を取得します。
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// IBlockEffectFactory を取得します。
        /// </summary>
        public IBlockEffectFactory BlockEffectFactory { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice。</param>
        /// <param name="blockEffectFactory">IBlockEffectFactory。</param>
        public BlockMeshFactory(GraphicsDevice graphicsDevice, IBlockEffectFactory blockEffectFactory)
        {
            if (graphicsDevice == null) throw new ArgumentNullException("graphicsDevice");
            if (blockEffectFactory == null) throw new ArgumentNullException("blockEffectFactory");
            GraphicsDevice = graphicsDevice;
            BlockEffectFactory = blockEffectFactory;
        }

        /// <summary>
        /// InterBlockMesh の頂点情報とエフェクト情報をもとに BlockMesh を生成します。
        /// </summary>
        /// <param name="interMesh">頂点情報とエフェクト情報を提供する InterBlockMesh。</param>
        /// <returns>生成された BlockMesh。</returns>
        public BlockMesh Create(InterBlockMesh interMesh)
        {
            var mesh = new BlockMesh(interMesh.LevelOfDetailSize);

            var effects = new IBlockEffect[interMesh.Effects.Count];
            for (int i = 0; i < effects.Length; i++)
            {
                // IBlockEffect の生成を IBlockEffectFactory へ委譲します。
                var effect = BlockEffectFactory.CreateBlockEffect();

                var interEffect = interMesh.Effects[i];
                effect.DiffuseColor = interEffect.DiffuseColor;
                effect.EmissiveColor = interEffect.EmissiveColor;
                effect.SpecularColor = interEffect.SpecularColor;
                effect.SpecularPower = interEffect.SpecularPower;

                effects[i] = effect;
            }
            mesh.SetEffectArray(effects);

            for (int lod = 0; lod < interMesh.LevelOfDetailSize; lod++)
            {
                interMesh.LevelOfDetail = lod;

                var meshParts = new BlockMeshPart[interMesh.MeshParts.Count];
                for (int i = 0; i < interMesh.MeshParts.Count; i++)
                {
                    var interMeshPart = interMesh.MeshParts[i];

                    meshParts[i] = BlockMeshPart.Create(GraphicsDevice, interMeshPart.Vertices, interMeshPart.Indices);
                    meshParts[i].Effect = effects[interMeshPart.EffectIndex];
                }
                mesh.SetLODMeshPartArray(lod, meshParts);
            }

            return mesh;
        }
    }
}
