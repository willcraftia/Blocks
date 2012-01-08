﻿#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

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
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(Block));

                // シリアライズ
                serializer.WriteObject(stream, block);
                var json = Encoding.UTF8.GetString(stream.ToArray());
                Console.WriteLine(json);

                // デシリアライズ
                stream.Seek(0, SeekOrigin.Begin);
                var deserialized = serializer.ReadObject(stream) as Block;
                Console.WriteLine(deserialized);
            }
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
            block.Cubes = new List<Cube>();

            var cube_0_0_0 = new Cube()
            {
                Color = new CubeColor(255, 255, 255),
                Position = new CubePosition(0, 0, 0)
            };
            block.Cubes.Add(cube_0_0_0);

            var cube_0_0_16 = new Cube()
            {
                Color = new CubeColor(255, 255, 255),
                Position = new CubePosition(0, 0, 16)
            };
            block.Cubes.Add(cube_0_0_16);

            var cube_16_0_16 = new Cube()
            {
                Color = new CubeColor(255, 255, 255),
                Position = new CubePosition(16, 0, 16)
            };
            block.Cubes.Add(cube_16_0_16);

            var cube_16_0_0 = new Cube()
            {
                Color = new CubeColor(255, 255, 255),
                Position = new CubePosition(16, 0, 0)
            };
            block.Cubes.Add(cube_16_0_0);


            var cube_0_16_0 = new Cube()
            {
                Color = new CubeColor(255, 255, 255),
                Position = new CubePosition(0, 16, 0)
            };
            block.Cubes.Add(cube_0_16_0);

            var cube_0_16_16 = new Cube()
            {
                Color = new CubeColor(255, 255, 255),
                Position = new CubePosition(0, 16, 16)
            };
            block.Cubes.Add(cube_0_16_16);

            var cube_16_16_16 = new Cube()
            {
                Color = new CubeColor(255, 255, 255),
                Position = new CubePosition(16, 16, 16)
            };
            block.Cubes.Add(cube_16_16_16);

            var cube_16_16_0 = new Cube()
            {
                Color = new CubeColor(255, 255, 255),
                Position = new CubePosition(16, 16, 0)
            };
            block.Cubes.Add(cube_16_16_0);

            return block;
        }
    }
}
