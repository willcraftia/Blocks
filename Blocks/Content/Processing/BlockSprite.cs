#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Blocks.Content.Processing
{
    public sealed class BlockSprite
    {
        Color[,] pixels;

        public int Width { get; private set; }

        public int Height { get; private set; }

        public BlockSprite(int width, int height)
        {
            Width = width;
            Height = height;
            pixels = new Color[width, height];
        }

        public Color GetPixel(int x, int y)
        {
            return pixels[x, y];
        }

        public void SetPixel(int x, int y, Color color)
        {
            pixels[x, y] = color;
        }
    }
}
