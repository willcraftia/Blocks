#region Using

using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization
{
    public sealed class ContentSerializer<T>
    {
        static XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

        public XmlWriterSettings WriterSettings { get; private set; }

        public XmlReaderSettings ReaderSettings { get; private set; }

        public ContentSerializer()
        {
            WriterSettings = new XmlWriterSettings
            {
                Indent = false
            };
            ReaderSettings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true
            };
        }

        public void Serialize(Stream stream, T content)
        {
            var writer = XmlWriter.Create(stream, WriterSettings);
            xmlSerializer.Serialize(writer, content);
        }

        public T Deserialize(Stream stream)
        {
            var reader = XmlReader.Create(stream, ReaderSettings);
            return (T) xmlSerializer.Deserialize(reader);
        }
    }
}
