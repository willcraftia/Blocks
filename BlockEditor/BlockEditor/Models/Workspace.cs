#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Blocks.Content;
using Willcraftia.Xna.Blocks.Graphics;
using Willcraftia.Xna.Blocks.Serialization;
using Willcraftia.Xna.Blocks.Storage;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Models
{
    public sealed class Workspace : IDisposable
    {
        public const int GridSize = 16;

        public const float CellSize = 0.1f;

        public event EventHandler BlockChanged = delegate { };

        int selectedMaterialIndex;

        public Game Game { get; private set; }

        public GraphicsDevice GraphicsDevice { get; private set; }

        public IStorageBlockService StorageBlockService { get; private set; }

        public Block Block { get; private set; }

        public Scene Scene { get; private set; }

        public CubeMesh CubeMesh { get; private set; }

        public Section Section { get; private set; }

        public int SelectedMaterialIndex
        {
            get { return selectedMaterialIndex; }
            set
            {
                if (selectedMaterialIndex == value) return;
                if (value < 0 || Block.Materials.Count <= value)
                    throw new ArgumentOutOfRangeException("value");

                selectedMaterialIndex = value;
            }
        }

        public Workspace(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");
            Game = game;

            GraphicsDevice = game.GraphicsDevice;

            StorageBlockService = game.Services.GetRequiredService<IStorageBlockService>();

            Scene = new Scene(this);

            CubeMesh = new CubeMesh(GraphicsDevice, CellSize);
            CubeMesh.Effect.EnableDefaultLighting();

            Section = new Section(this);

            // todo: code for demo
            Block = CreateOctahedronLikeBlock();
            Section.FetchElements();
        }

        // todo: Block にメソッドを作る？
        public int CreateMaterial(
            MaterialColor diffuseColor,
            MaterialColor emissiveColor,
            MaterialColor specularColor, float specularPower)
        {
            var materials = Block.Materials;

            for (int i = 0; i < materials.Count; i++)
            {
                var m = materials[i];
                if (m.DiffuseColor == diffuseColor &&
                    m.EmissiveColor == emissiveColor &&
                    m.SpecularColor == specularColor && m.SpecularPower == specularPower)
                {
                    return i;
                }
            }

            var material = new Material
            {
                DiffuseColor = diffuseColor,
                EmissiveColor = emissiveColor,
                SpecularColor = specularColor,
                SpecularPower = specularPower
            };
            materials.Add(material);

            return materials.Count - 1;
        }

        public void Update(GameTime gameTime)
        {
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

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool disposed;

        ~Workspace()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
            }

            disposed = true;
        }

        #endregion
    }
}
