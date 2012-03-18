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
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice。</param>
        public BlockMeshFactory(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null) throw new ArgumentNullException("graphicsDevice");
            GraphicsDevice = graphicsDevice;
        }

        /// <summary>
        /// InterBlockMesh の頂点情報とエフェクト情報をもとに BlockMesh を生成します。
        /// </summary>
        /// <param name="interMesh">頂点情報とエフェクト情報を提供する InterBlockMesh。</param>
        /// <returns>生成された BlockMesh。</returns>
        public BlockMesh Create(InterBlockMesh interMesh)
        {
            var lodCount = interMesh.MeshLods.Length;

            var mesh = new BlockMesh();
            mesh.SetMeshMaterials(interMesh.MeshMaterials);

            mesh.AllocateMeshLods(lodCount);

            for (int lod = 0; lod < lodCount; lod++)
            {
                var interMeshLod = interMesh.MeshLods[lod];
                var meshLod = mesh.MeshLods[lod];

                var meshPartCount = interMeshLod.MeshParts.Length;
                meshLod.AllocateMeshParts(GraphicsDevice, meshPartCount);

                for (int i = 0; i < meshPartCount; i++)
                {
                    var interMeshPart = interMeshLod.MeshParts[i];
                    var meshPart = meshLod.MeshParts[i];

                    meshPart.MeshMaterial = mesh.MeshMaterials[interMeshPart.MeshMaterialIndex];
                    meshPart.PopulateVertices(interMeshPart.Vertices);
                    meshPart.PopulateIndices(interMeshPart.Indices);
                }
            }

            return mesh;
        }
    }
}
