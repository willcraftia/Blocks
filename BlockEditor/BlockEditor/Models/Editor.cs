#region Using

using System;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Models
{
    public sealed class Editor
    {
        Workspace workspace;

        int sectionIndex;

        Element[,] elements = new Element[16, 16];

        Block Block
        {
            get { return workspace.Block; }
        }

        public SectionOrientation Orientation { get; set; }

        public int SectionIndex
        {
            get { return sectionIndex; }
            set
            {
                if (value < 0 || 16 < value) throw new ArgumentOutOfRangeException("value");
                if (sectionIndex == value) return;

                sectionIndex = value;
                FetchElements();
            }
        }

        public Editor(Workspace workspace)
        {
            if (workspace == null) throw new ArgumentNullException("workspace");
            this.workspace = workspace;

            workspace.BlockChanged += new EventHandler(OnWorkspaceBlockChanged);

            Orientation = SectionOrientation.Z;
            sectionIndex = 0;
            FetchElements();
        }

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

        public Material GetMaterial(int materialIndex)
        {
            if (materialIndex < 0 || Block.Materials.Count <= materialIndex)
                throw new ArgumentOutOfRangeException("materialIndex");

            return Block.Materials[materialIndex];
        }

        public void SetElement(int x, int y, int materialIndex)
        {
            if (x < 0 || 16 <= x) throw new ArgumentOutOfRangeException("x");
            if (y < 0 || 16 <= y) throw new ArgumentOutOfRangeException("y");
            if (materialIndex < 0 || Block.Materials.Count <= materialIndex)
                throw new ArgumentOutOfRangeException("materialIndex");

            if (elements[x, y] == null)
            {
                elements[x, y] = new Element
                {
                    Position = ResolvePosition(x, y),
                    MaterialIndex = materialIndex
                };
            }
            else
            {
                elements[x, y].MaterialIndex = materialIndex;
            }
        }

        public Element GetElement(int x, int y)
        {
            if (x < 0 || 16 <= x) throw new ArgumentOutOfRangeException("x");
            if (y < 0 || 16 <= y) throw new ArgumentOutOfRangeException("y");

            return elements[x, y];
        }

        public void RemoveElement(int x, int y)
        {
            if (x < 0 || 16 <= x) throw new ArgumentOutOfRangeException("x");
            if (y < 0 || 16 <= y) throw new ArgumentOutOfRangeException("y");

            Block.Elements.Remove(elements[x, y]);
            elements[x, y] = null;
        }

        void OnWorkspaceBlockChanged(object sender, EventArgs e)
        {
            FetchElements();
        }

        void FetchElements()
        {
            Array.Clear(elements, 0, elements.Length);

            if (Block == null) return;

            switch (Orientation)
            {
                case SectionOrientation.Z:
                    FetchElementsByZ();
                    break;
                case SectionOrientation.Y:
                    FetchElementsByY();
                    break;
                case SectionOrientation.X:
                    FetchElementsByX();
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        void FetchElementsByZ()
        {
            var z = sectionIndex - 8;
            foreach (var element in Block.Elements)
            {
                if (element.Position.Z == z)
                {
                    int x = element.Position.X + 8;
                    int y = element.Position.Y + 8;
                    elements[x, y] = element;
                }
            }
        }

        void FetchElementsByY()
        {
            var y = sectionIndex - 8;
            foreach (var element in Block.Elements)
            {
                if (element.Position.Y == y)
                {
                    int x = element.Position.X + 8;
                    int z = element.Position.Z + 8;
                    elements[x, z] = element;
                }
            }
        }

        void FetchElementsByX()
        {
            var x = sectionIndex - 8;
            foreach (var element in Block.Elements)
            {
                if (element.Position.X == x)
                {
                    int z = element.Position.Z + 8;
                    int y = element.Position.Y + 8;
                    elements[z, y] = element;
                }
            }
        }

        Position ResolvePosition(int x, int y)
        {
            switch (Orientation)
            {
                case SectionOrientation.Z:
                    return new Position
                    {
                        X = x - 8,
                        Y = y - 8,
                        Z = sectionIndex - 8
                    };
                case SectionOrientation.Y:
                    return new Position
                    {
                        X = x - 8,
                        Y = sectionIndex - 8,
                        Z = y - 8
                    };
                case SectionOrientation.X:
                    return new Position
                    {
                        X = sectionIndex - 8,
                        Y = y - 8,
                        Z = x - 8
                    };
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
