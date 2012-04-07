#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Cameras;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Blocks.Serialization;
using Willcraftia.Xna.Blocks.BlockEditor.Models;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.ViewModels
{
    public sealed class BlockEditViewModel
    {
        GraphicsDevice graphicsDevice;

        BasicEffect basicEffect;

        GeometricPrimitive cube;

        Block block;

        GridView gridView;

        PerspectiveFov projection;

        public BlockEditViewModel(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;

            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.EnableDefaultLighting();

            cube = CreateCubePrimitive();

            block = CreateOctahedronLikeBlock();

            gridView = new GridView
            {
                Distance = 23.5f
            };

            projection = new PerspectiveFov
            {
                NearPlaneDistance = 0.01f,
                FarPlaneDistance = 100
            };
        }

        public void MoveLeft()
        {
            gridView.MoveLeft();
        }

        public void MoveRight()
        {
            gridView.MoveRight();
        }

        public void MoveUp()
        {
            gridView.MoveUp();
        }

        public void MoveDown()
        {
            gridView.MoveDown();
        }

        public void MoveForward()
        {
            gridView.MoveForward();
        }

        public void MoveBackward()
        {
            gridView.MoveBackward();
        }

        public void Draw()
        {
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Projection を更新します。
            projection.AspectRatio = graphicsDevice.Viewport.AspectRatio;
            projection.Update();

            // CameraView を更新します。
            gridView.Update();

            foreach (var element in block.Elements)
            {
                var position = new Vector3
                {
                    X = element.Position.X,
                    Y = element.Position.Y,
                    Z = element.Position.Z
                };
                Matrix translation;
                Matrix.CreateTranslation(ref position, out translation);

                var material = block.Materials[element.MaterialIndex];

                basicEffect.World = translation;
                basicEffect.View = gridView.Matrix;
                basicEffect.Projection = projection.Matrix;
                basicEffect.DiffuseColor = material.DiffuseColor.ToColor().ToVector3();
                basicEffect.EmissiveColor = material.EmissiveColor.ToColor().ToVector3();
                basicEffect.SpecularColor = material.SpecularColor.ToColor().ToVector3();
                basicEffect.SpecularPower = material.SpecularPower;

                cube.Draw(basicEffect);
            }
        }

        /// <summary>
        /// 立方体の GeometricPrimitive を生成します。
        /// </summary>
        /// <returns>生成された立方体の GeometricPrimitive。</returns>
        GeometricPrimitive CreateCubePrimitive()
        {
            var sourceFactory = new CubeVertexSourceFactory();
            var source = sourceFactory.CreateVertexSource();
            return GeometricPrimitive.Create(graphicsDevice, source);
        }

        #region For Demo

        /// <summary>
        /// 正八面体風のデータを定義する Block を作成します。
        /// </summary>
        /// <returns>作成された Block。</returns>
        static Block CreateOctahedronLikeBlock()
        {
            var block = new Block();

            MaterialColor[] diffuses =
            {
                new MaterialColor(255, 255, 255),
                new MaterialColor(255,   0,   0),
                new MaterialColor(  0, 255,   0),
                new MaterialColor(  0,   0, 255),
                new MaterialColor(127, 127,   0),
                new MaterialColor(127,   0, 127),
                new MaterialColor(  0, 127, 127),
                new MaterialColor(  0,   0,   0),
            };
            Material[] materials = new Material[8];
            for (int i = 0; i < 8; i++)
            {
                materials[i] = new Material
                {
                    DiffuseColor = diffuses[i]
                };
                block.Materials.Add(materials[i]);
            }

            int materialIndex;
            for (int x = -8; x < 8; x++)
            {
                for (int y = -8; y < 8; y++)
                {
                    for (int z = -8; z < 8; z++)
                    {
                        int testX = (x < 0) ? -x : x + 1;
                        int testY = (y < 0) ? -y : y + 1;
                        int testZ = (z < 0) ? -z : z + 1;

                        if (testX + testY + testZ <= 10)
                        {
                            materialIndex = 0;
                            if (x < 0) materialIndex |= 1;
                            if (y < 0) materialIndex |= 2;
                            if (z < 0) materialIndex |= 4;

                            var element = new Element
                            {
                                Position = new Position(x, y, z),
                                MaterialIndex = materialIndex
                            };
                            block.Elements.Add(element);
                        }
                    }
                }
            }

            return block;
        }

        #endregion
    }
}
