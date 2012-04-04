#region Using

using System;
using System.IO;
using System.Text;
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
                Indent = false,
                Encoding = Encoding.UTF8,
            };
            ReaderSettings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true
            };
        }

        public XmlWriter CreateXmlWriter(Stream stream)
        {
            return XmlWriter.Create(stream, WriterSettings);
        }

        public XmlReader CreateXmlReader(Stream stream)
        {
            return XmlReader.Create(stream, ReaderSettings);
        }

        public void Serialize(Stream stream, T content)
        {
            var writer = CreateXmlWriter(stream);
            xmlSerializer.Serialize(writer, content);
        }

        public T Deserialize(Stream stream)
        {
            var reader = CreateXmlReader(stream);
            return (T) xmlSerializer.Deserialize(reader);
        }
    }
}
