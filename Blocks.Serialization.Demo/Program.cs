#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //var block = CreateSimpleBlock();
            var block = CreateOctahedronLikeBlock();

            SerializeAndDeserializeAsJson(block);

            // XML は用いませんが、自分へのサンプル コードとして記述しています。
            SerializeAndDeserializeAsXml(block);

            Console.ReadLine();
        }

        static void SaveAsJson(Block block)
        {
            var dir = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(dir, "Block.json");

            var stream = new FileStream(filePath, FileMode.Create);
            JsonHelper.WriteObject(block, stream);
            stream.Close();
        }

        /// <summary>
        /// Block を JSON 記法でシリアライズし、それをデシリアライズします。
        /// </summary>
        /// <param name="block">Block。</param>
        static void SerializeAndDeserializeAsJson(Block block)
        {
            // シリアライズ
            string json = JsonHelper.ToJson<Block>(block);
            Console.WriteLine(json);
            SaveAsJson(block);

            // デシリアライズ
            var deserialized = JsonHelper.FromJson<Block>(json);
            Console.WriteLine(deserialized);
        }

        /// <summary>
        /// Block を XML 形式でシリアライズし、それをデシリアライズします。
        /// </summary>
        /// <param name="block">Block。</param>
        static void SerializeAndDeserializeAsXml(Block block)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof(Block));

                // シリアライズ
                serializer.Serialize(stream, block);
                var xml = Encoding.UTF8.GetString(stream.ToArray());
                Console.WriteLine(xml);

                // デシリアライズ
                stream.Seek(0, SeekOrigin.Begin);
                var deserialized = serializer.Deserialize(stream) as Block;
                Console.WriteLine(deserialized);
            }
        }

        /// <summary>
        /// 簡単な Block を生成します。
        /// </summary>
        /// <returns>生成された Block。</returns>
        static Block CreateSimpleBlock()
        {
            var block = new Block();
            block.Materials = new List<Material>();
            block.Elements = new List<Element>();

            var material = new Material()
            {
                DiffuseColor = new MaterialColor(63, 127, 255)
            };
            block.Materials.Add(material);

            block.Elements.Add(new Element() { Position = new Position( 0,  0,  0), MaterialIndex = 0 });
            block.Elements.Add(new Element() { Position = new Position( 0,  0, 16), MaterialIndex = 0 });
            block.Elements.Add(new Element() { Position = new Position(16,  0, 16), MaterialIndex = 0 });
            block.Elements.Add(new Element() { Position = new Position(16,  0,  0), MaterialIndex = 0 });
            block.Elements.Add(new Element() { Position = new Position( 0, 16,  0), MaterialIndex = 0 });
            block.Elements.Add(new Element() { Position = new Position( 0, 16, 16), MaterialIndex = 0 });
            block.Elements.Add(new Element() { Position = new Position(16, 16, 16), MaterialIndex = 0 });
            block.Elements.Add(new Element() { Position = new Position(16, 16,  0), MaterialIndex = 0 });

            return block;
        }

        /// <summary>
        /// 正八面体風のデータを定義する Block を作成します。
        /// </summary>
        /// <returns>作成された Block。</returns>
        static Block CreateOctahedronLikeBlock()
        {
            var block = new Block();
            block.Materials = new List<Material>();
            block.Elements = new List<Element>();

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
                materials[i] = new Material();
                materials[i].DiffuseColor = diffuses[i];
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

                            var element = new Element();
                            element.Position = new Position(x, y, z);
                            element.MaterialIndex = materialIndex;
                            block.Elements.Add(element);
                        }
                    }
                }
            }

            return block;
        }
    }
}
