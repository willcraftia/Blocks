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
            var block = CreateSimpleBlock();

            SerializeAndDeserializeAsJson(block);

            // XML は用いませんが、自分へのサンプル コードとして記述しています。
            SerializeAndDeserializeAsXml(block);

            Console.ReadLine();
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
    }
}
