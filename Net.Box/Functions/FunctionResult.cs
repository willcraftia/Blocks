#region Using

using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class FunctionResult<T> where T : class
    {
        static XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

        public static T GetResult(WebResponse response, bool dumpXml)
        {
            using (var memoryStream = GetStreamAsMemoryStream(response))
            {
                if (dumpXml)
                {
                    var reader = new StreamReader(memoryStream);
                    Console.WriteLine("MemoryStream:");
                    Console.WriteLine(reader.ReadToEnd());

                    memoryStream.Position = 0;
                }

                // デシリアライズします。
                return xmlSerializer.Deserialize(memoryStream) as T;
            }
        }

        /// <summary>
        /// WebResponse の Stream の内容を MemoryStream に設定して返します。
        /// </summary>
        /// <param name="response">WebResponse。</param>
        /// <returns>MemoryStream。</returns>
        static MemoryStream GetStreamAsMemoryStream(WebResponse response)
        {
            using (var responseStream = response.GetResponseStream())
            {
                var memoryStream = new MemoryStream();

                byte[] bytes = new byte[1024];
                int byteCount = 0;

                while ((byteCount = responseStream.Read(bytes, 0, bytes.Length)) != 0)
                    memoryStream.Write(bytes, 0, byteCount);

                memoryStream.Flush();
                memoryStream.Position = 0;

                return memoryStream;
            }
        }
    }
}
