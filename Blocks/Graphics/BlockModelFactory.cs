#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    /// <summary>
    /// 永続化用データ表現の Block から 3D モデル描画のための BlockModel を生成するクラスです。
    /// </summary>
    public sealed class BlockModelFactory
    {
        /// <summary>
        /// CubeVertexSourceFactory を取得します。
        /// </summary>
        public ColoredCubeVertexSourceFactory CubeVertexSourceFactory { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public BlockModelFactory()
        {
            CubeVertexSourceFactory = new ColoredCubeVertexSourceFactory();
        }

        /// <summary>
        /// BlockModel を生成します。
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice。</param>
        /// <param name="block">Block。</param>
        /// <returns>生成された BlockModel。</returns>
        public BlockModel CreateBlockModel(GraphicsDevice graphicsDevice, Block block)
        {
            GeometricPrimitive cubePrimitive;
            using (var source = CubeVertexSourceFactory.CreateVertexSource())
            {
                cubePrimitive = GeometricPrimitive.Create(graphicsDevice, source);
            }

            var model = new BlockModel();

            foreach (var material in block.Materials)
            {
                var modelMaterial = CreateBlockModelMaterial(material);
                model.InternalMaterials.Add(modelMaterial);
            }

            foreach (var mesh in block.Meshes)
            {
                var modelMesh = CreateBlockModelMesh(mesh, cubePrimitive);
                model.InternalMeshes.Add(modelMesh);
            }

            return model;
        }

        /// <summary>
        /// BlockModelMaterial を生成します。
        /// </summary>
        /// <param name="material">Material。</param>
        /// <returns>生成された BlockModelMaterial。</returns>
        BlockModelMaterial CreateBlockModelMaterial(Material material)
        {
            var modelMaterial = new BlockModelMaterial();

            modelMaterial.DiffuseColor = material.DiffuseColor.ToColor().ToVector3();
            modelMaterial.EmissiveColor = material.EmissiveColor.ToColor().ToVector3();
            modelMaterial.SpecularColor = material.SpecularColor.ToColor().ToVector3();
            modelMaterial.SpecularPower = material.SpecularPower;
            modelMaterial.Alpha = material.Alpha;

            return modelMaterial;
        }

        /// <summary>
        /// BlockModelMesh を生成します。
        /// </summary>
        /// <param name="mesh">BlockMesh。</param>
        /// <param name="geometricPrimitive">GeometricPrimitive (Cube Primitive)。</param>
        /// <returns>生成された BlockModelMesh。</returns>
        BlockModelMesh CreateBlockModelMesh(BlockMesh mesh, GeometricPrimitive geometricPrimitive)
        {
            var modelMesh = new BlockModelMesh(geometricPrimitive);

            Matrix transform;
            CreateTransform(ref mesh.Position, out transform);
            modelMesh.Transform = transform;

            modelMesh.MaterialIndex = mesh.MaterialIndex;

            return modelMesh;
        }

        /// <summary>
        /// Position から変換行列を生成します。
        /// </summary>
        /// <param name="position">Position。</param>
        /// <param name="transform">生成された変換行列。</param>
        void CreateTransform(ref Position position, out Matrix transform)
        {
            var v = new Vector3();
            v.X = CubeVertexSourceFactory.Size * (float) position.X;
            v.Y = CubeVertexSourceFactory.Size * (float) position.Y;
            v.Z = CubeVertexSourceFactory.Size * (float) position.Z;

            Matrix.CreateTranslation(ref v, out transform);
        }
    }
}
