#region Using

using System;
using System.IO;
using System.Drawing;
using Willcraftia.Xna.Blocks.Content.Processing;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.BuildBlockSprite
{
    using XnaColor = Microsoft.Xna.Framework.Color;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: BuildBlockSprite <SpritePath> <DestinationPath>");
                return;
            }

            var spritePath = args[0];
            var destinationPath = args[1];

            BlockSprite blockSprite;

            using (var stream = new FileStream(spritePath, FileMode.Open))
            {
                var bitmap = new Bitmap(stream);
                var w = bitmap.Width;
                var h = bitmap.Height;

                if (w != 16 || h != 16)
                    throw new InvalidOperationException("The size of a sprite is invalid.");

                blockSprite = new BlockSprite(w, h);

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        var c = bitmap.GetPixel(x, y);
                        blockSprite.SetPixel(x, y, new XnaColor(c.R, c.G, c.B, c.A));
                    }
                }
            }

            var processor = new BlockSpriteProcessor();
            var block = processor.Process(blockSprite);

            var destinationDir = Path.GetDirectoryName(destinationPath);
            if (!Directory.Exists(destinationDir)) Directory.CreateDirectory(destinationDir);

            var serializer = new ContentSerializer<Block>();
            using (var stream = new FileStream(destinationPath, FileMode.Create))
            {
                serializer.Serialize(stream, block);
            }
        }
    }
}
