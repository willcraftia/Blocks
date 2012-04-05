#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
//using Newtonsoft.Json;
using Willcraftia.Xna.Framework.Text;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization.Demo
{
    class Program
    {
        static ContentSerializer<Block> blockSerializer = new ContentSerializer<Block>();

        static ContentSerializer<Description> descriptionSerializer = new ContentSerializer<Description>();

        static void Main(string[] args)
        {
            //var block = CreateSimpleBlock();
            var block = CreateOctahedronLikeBlock();

            // シリアライズとデシリアライズのテスト
            SerializeAndDeserialize(block);

            // 他のアプリケーションで利用するためのデータの作成
            var description = new Description
            {
                Name = "Demo Block"
            };
            Save(Directory.GetCurrentDirectory(), "DemoBlock", description, block);

            //DoJsonDemo(block);

            Console.WriteLine();
            Console.WriteLine("Press enter key to exit.");
            Console.ReadLine();
        }

        //static void DoJsonDemo(Block block)
        //{
        //    Console.WriteLine();
        //    Console.WriteLine("Convert between json and xml:");

        //    using (var stream = new MemoryStream())
        //    {
        //        // シリアライズ
        //        blockSerializer.Serialize(stream, block);
        //        var xml = Encoding.UTF8.GetString(stream.ToArray());
        //        Console.WriteLine("XML:");
        //        Console.WriteLine(xml);

        //        // XML -> JSON 変換
        //        stream.Position = 0;
        //        var doc = new XmlDocument();
        //        var xmlReader = blockSerializer.CreateXmlReader(stream);
        //        doc.Load(xmlReader);
        //        var json = JsonConvert.SerializeXmlNode(doc);
        //        Console.WriteLine("JSON:");
        //        Console.WriteLine(json);

        //        // JSON -> XML 変換
        //        stream.Position = 0;
        //        doc = JsonConvert.DeserializeXmlNode(json);
        //        var writerSettings = new XmlWriterSettings
        //        {
        //            Indent = false,
        //            Encoding = Encoding.UTF8,
        //        };
        //        var writer = XmlWriter.Create(stream, writerSettings);
        //        doc.Save(writer);
        //        xml = Encoding.UTF8.GetString(stream.ToArray());
        //        Console.WriteLine("XML:");
        //        Console.WriteLine(xml);

        //        // デシリアライズ
        //        stream.Position = 0;
        //        var deserialized = blockSerializer.Deserialize(stream);
        //        Console.WriteLine("Block:");
        //        Console.WriteLine(deserialized);
        //    }
        //}

        /// <summary>
        /// Block をシリアライズし、それをデシリアライズします。
        /// </summary>
        /// <param name="block">Block。</param>
        static void SerializeAndDeserialize(Block block)
        {
            using (var stream = new MemoryStream())
            {
                // シリアライズ
                blockSerializer.Serialize(stream, block);
                var serialized = Encoding.UTF8.GetString(stream.ToArray());
                Console.WriteLine("XML:");
                Console.WriteLine(serialized);

                // デシリアライズ
                stream.Position = 0;
                var deserialized = blockSerializer.Deserialize(stream);
                Console.WriteLine("Block:");
                Console.WriteLine(deserialized);
            }
        }

        /// <summary>
        /// Block を指定のパスで保存します。
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="block"></param>
        static void Save(string directoryPath, string name, Description description, Block block)
        {
            var descriptionPath = Description.ResolveFileName(Path.Combine(directoryPath, name));
            using (var stream = new FileStream(descriptionPath, FileMode.Create))
            {
                descriptionSerializer.Serialize(stream, description);
            }

            var blockPath = Block.ResolveFileName(Path.Combine(directoryPath, name));
            using (var stream = new FileStream(blockPath, FileMode.Create))
            {
                blockSerializer.Serialize(stream, block);
            }
        }

        #region Generate Test Data

        /// <summary>
        /// 簡単な Block を生成します。
        /// </summary>
        /// <returns>生成された Block。</returns>
        static Block CreateSimpleBlock()
        {
            var block = new Block
            {
                Materials = new List<Material>(),
                Elements = new List<Element>()
            };

            var material = new Material
            {
                DiffuseColor = new MaterialColor(63, 127, 255)
            };
            block.Materials.Add(material);

            block.Elements.Add(new Element { Position = new Position(-8, -8, -8), MaterialIndex = 0 });
            block.Elements.Add(new Element { Position = new Position(-8, -8,  7), MaterialIndex = 0 });
            block.Elements.Add(new Element { Position = new Position( 7, -8,  7), MaterialIndex = 0 });
            block.Elements.Add(new Element { Position = new Position( 7, -8, -8), MaterialIndex = 0 });
            block.Elements.Add(new Element { Position = new Position(-8,  7, -8), MaterialIndex = 0 });
            block.Elements.Add(new Element { Position = new Position(-8,  7,  7), MaterialIndex = 0 });
            block.Elements.Add(new Element { Position = new Position( 7,  7,  7), MaterialIndex = 0 });
            block.Elements.Add(new Element { Position = new Position( 7,  7, -8), MaterialIndex = 0 });

            return block;
        }

        /// <summary>
        /// 正八面体風のデータを定義する Block を作成します。
        /// </summary>
        /// <returns>作成された Block。</returns>
        static Block CreateOctahedronLikeBlock()
        {
            var block = new Block
            {
                Materials = new List<Material>(),
                Elements = new List<Element>()
            };

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
