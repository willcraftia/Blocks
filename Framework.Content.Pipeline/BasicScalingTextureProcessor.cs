#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

#endregion

namespace Willcraftia.Xna.Framework.Content.Pipeline
{
    [ContentProcessor(DisplayName = "Basic Scaling Texture Processor")]
    public class BasicScalingTextureProcessor : TextureProcessor
    {
        public float Scale { get; set; }

        public BasicScalingTextureProcessor()
        {
            Scale = 1;
        }

        public override TextureContent Process(TextureContent input, ContentProcessorContext context)
        {
            input.ConvertBitmapType(typeof(PixelBitmapContent<Color>));
            foreach (var mipmapChain in input.Faces)
            {
                for (int i = 0; i < mipmapChain.Count; i++)
                {
                    var bitmap = mipmapChain[i] as PixelBitmapContent<Color>;

                    int newWidth = (int) ((float) bitmap.Width * Scale);
                    int newHeight = (int) ((float) bitmap.Height * Scale);

                    var newBitmap = new PixelBitmapContent<Color>(newWidth, newHeight);

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            for (int scaleY = 0; scaleY < Scale; scaleY++)
                            {
                                for (int scaleX = 0; scaleX < Scale; scaleX++)
                                {
                                    newBitmap.SetPixel(
                                        (int) (x * Scale) + scaleX,
                                        (int) (y * Scale) + scaleY,
                                        bitmap.GetPixel(x, y));
                                }
                            }
                        }
                    }

                    mipmapChain[i] = newBitmap;
                }
            }

            return base.Process(input, context);
        }
    }
}
