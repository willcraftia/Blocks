#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Content.Processing
{
    public sealed class BlockSpriteProcessor
    {
        public Block Process(BlockSprite blockSprite)
        {
            var block = new Block();

            var w = blockSprite.Width;
            var h = blockSprite.Height;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var color = blockSprite.GetPixel(x, y);

                    // 透明はその位置に要素がないことを表します。
                    if (color.A == 0) continue;

                    var materialIndex = ProcessMaterial(ref block, ref color);

                    // Block は [-8, 7] で 16x16 になっています。

                    var element = new Element
                    {
                        MaterialIndex = materialIndex,
                        Position = new Position
                        {
                            X = x - 8,
                            Y = 7 - y
                        }
                    };

                    block.Elements.Add(element);
                }
            }

            return block;
        }

        int ProcessMaterial(ref Block block, ref Color color)
        {
            Material material = null;
            int materialIndex = -1;
            for (int i = 0; i < block.Materials.Count; i++)
            {
                var m = block.Materials[i];
                var mColor = m.DiffuseColor;

                // RGB で比較し、A が異なっても同じ Material とします。
                if (mColor.R == color.R && mColor.G == color.G && mColor.B == color.B)
                {
                    material = m;
                    materialIndex = i;
                    break;
                }
            }

            if (material == null)
            {
                material = new Material
                {
                    DiffuseColor = new MaterialColor
                    {
                        R = color.R,
                        G = color.G,
                        B = color.B
                    }
                };
                block.Materials.Add(material);
                materialIndex = block.Materials.Count - 1;
            }

            return materialIndex;
        }
    }
}
