#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Models
{
    public sealed class Section
    {
        const int gridSize = Workspace.GridSize;

        const int halfGridSize = gridSize / 2;

        Workspace workspace;

        SectionOrientation orientation;

        int index;

        Element[,] elements = new Element[gridSize, gridSize];

        public SectionOrientation Orientation
        {
            get { return orientation; }
            set
            {
                if (orientation == value) return;

                orientation = value;
                FetchElements();
            }
        }

        public int Index
        {
            get { return index; }
            set
            {
                if (value < 0 || gridSize < value) throw new ArgumentOutOfRangeException("value");
                if (index == value) return;

                index = value;
                FetchElements();
            }
        }

        public Section(Workspace workspace)
        {
            if (workspace == null) throw new ArgumentNullException("workspace");
            this.workspace = workspace;

            Orientation = SectionOrientation.Z;
        }

        public void ChangeOrientation()
        {
            switch (Orientation)
            {
                case SectionOrientation.Z:
                    Orientation = SectionOrientation.Y;
                    break;
                case SectionOrientation.Y:
                    Orientation = SectionOrientation.X;
                    break;
                case SectionOrientation.X:
                    Orientation = SectionOrientation.Z;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public Material GetMaterial(int x, int y)
        {
            var element = GetElement(x, y);
            if (element == null) return null;

            return workspace.Block.Materials[element.MaterialIndex];
        }

        public void SetMaterial(int x, int y)
        {
            SetElement(x, y, workspace.SelectedMaterialIndex);
        }

        public void SetElement(int x, int y, int materialIndex)
        {
            if (x < 0 || gridSize <= x) throw new ArgumentOutOfRangeException("x");
            if (y < 0 || gridSize <= y) throw new ArgumentOutOfRangeException("y");
            if (materialIndex < 0 || workspace.Block.Materials.Count <= materialIndex)
                throw new ArgumentOutOfRangeException("materialIndex");

            var element = elements[x, y];

            if (element == null)
            {
                element = new Element
                {
                    Position = ResolvePosition(x, y),
                    MaterialIndex = materialIndex
                };

                workspace.Block.Elements.Add(element);
                elements[x, y] = element;
            }
            else
            {
                element.MaterialIndex = materialIndex;
            }
        }

        public Element GetElement(int x, int y)
        {
            if (x < 0 || gridSize <= x) throw new ArgumentOutOfRangeException("x");
            if (y < 0 || gridSize <= y) throw new ArgumentOutOfRangeException("y");

            return elements[x, y];
        }

        public void RemoveElement(int x, int y)
        {
            if (x < 0 || gridSize <= x) throw new ArgumentOutOfRangeException("x");
            if (y < 0 || gridSize <= y) throw new ArgumentOutOfRangeException("y");

            workspace.Block.Elements.Remove(elements[x, y]);
            elements[x, y] = null;
        }

        internal void FetchElements()
        {
            Array.Clear(elements, 0, elements.Length);

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
            var z = index - halfGridSize;
            foreach (var element in workspace.Block.Elements)
            {
                if (element.Position.Z == z)
                {
                    int x = element.Position.X + halfGridSize;
                    int y = element.Position.Y + halfGridSize;
                    elements[x, gridSize - 1 - y] = element;
                }
            }
        }

        void FetchElementsByY()
        {
            var y = index - halfGridSize;
            foreach (var element in workspace.Block.Elements)
            {
                if (element.Position.Y == y)
                {
                    int x = element.Position.X + halfGridSize;
                    int z = element.Position.Z + halfGridSize;
                    elements[x, gridSize - 1 - z] = element;
                }
            }
        }

        void FetchElementsByX()
        {
            var x = index - halfGridSize;
            foreach (var element in workspace.Block.Elements)
            {
                if (element.Position.X == x)
                {
                    int z = element.Position.Z + halfGridSize;
                    int y = element.Position.Y + halfGridSize;
                    elements[z, gridSize - 1 - y] = element;
                }
            }
        }

        Position ResolvePosition(int x, int y)
        {
            y = gridSize - 1 - y;

            switch (Orientation)
            {
                case SectionOrientation.Z:
                    return new Position
                    {
                        X = x - halfGridSize,
                        Y = y - halfGridSize,
                        Z = index - halfGridSize
                    };
                case SectionOrientation.Y:
                    return new Position
                    {
                        X = x - halfGridSize,
                        Y = index - halfGridSize,
                        Z = y - halfGridSize
                    };
                case SectionOrientation.X:
                    return new Position
                    {
                        X = index - halfGridSize,
                        Y = y - halfGridSize,
                        Z = x - halfGridSize
                    };
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
