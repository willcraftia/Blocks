#region Using

using System;
using System.IO;

#endregion

namespace Willcraftia.Xna.Framework.Serialization
{
    using DotNetXmlSerializer = System.Xml.Serialization.XmlSerializer;

    /// <summary>
    /// XML の ISerializer です。
    /// </summary>
    /// <typeparam name="T">シリアライズとデシリアライズの対象となる型。</typeparam>
    public sealed class XmlSerializer<T> : ISerializer<T>
    {
        /// <summary>
        /// 機能の実体。
        /// </summary>
        DotNetXmlSerializer dotNetXmlSerializer;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public XmlSerializer()
        {
            dotNetXmlSerializer = new DotNetXmlSerializer(typeof(T));
        }

        // I/F
        public T Deserialize(Stream stream)
        {
            return (T) dotNetXmlSerializer.Deserialize(stream);
        }

        // I/F
        public void Serialize(Stream stream, T o)
        {
            dotNetXmlSerializer.Serialize(stream, o);
        }
    }
}
