#region Using

using System;
using System.IO;
using System.Runtime.Serialization.Json;
using Willcraftia.Xna.Framework.Serialization;

#endregion

namespace Willcraftia.Xna.Win.Framework.Serialization
{
    /// <summary>
    /// JSON の ISerializer です。
    /// </summary>
    /// <typeparam name="T">シリアライズとデシリアライズの対象となる型。</typeparam>
    public sealed class JsonSerializer<T> : ISerializer<T>
    {
        /// <summary>
        /// 機能の実体。
        /// </summary>
        DataContractJsonSerializer serializer;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public JsonSerializer()
        {
            serializer = new DataContractJsonSerializer(typeof(T));
        }

        // I/F
        public T Deserialize(Stream stream)
        {
            return (T) serializer.ReadObject(stream);
        }

        // I/F
        public void Serialize(Stream stream, T o)
        {
            serializer.WriteObject(stream, o);
        }
    }
}
