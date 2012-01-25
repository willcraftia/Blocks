#region Using

using System;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

#endregion

namespace Willcraftia.Xna.Framework.Content.Pipeline.Localization
{
    [ContentProcessor(DisplayName = "Localized Font Processor")]
    public sealed class LocalizedFontProcessor : ContentProcessor<LocalizedFontDescription, SpriteFontContent>
    {
        public override SpriteFontContent Process(LocalizedFontDescription input, ContentProcessorContext context)
        {
            foreach (string resourceFile in input.ResourceFiles)
            {
                var fullPath = Path.GetFullPath(resourceFile);
                if (!File.Exists(fullPath)) throw new InvalidContentException("Can't find " + fullPath);

                var document = new XmlDocument();
                document.Load(fullPath);

                foreach (XmlNode node in document.SelectNodes("root/data/value"))
                {
                    foreach (var c in node.InnerText) input.Characters.Add(c);
                }

                context.AddDependency(fullPath);
            }

            return context.Convert<FontDescription, SpriteFontContent>(input, "FontDescriptionProcessor");
        }
    }
}
