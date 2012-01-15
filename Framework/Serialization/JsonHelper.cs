#region Using

using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

#endregion

namespace Willcraftia.Xna.Framework.Serialization
{
    /// <summary>
    /// JSON に関するヘルパ メソッドを提供するクラスです。
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// オブジェクトを JSON へ変換します。
        /// 文字列のエンコーディングは UTF-8 であることを仮定します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="graph">オブジェクト。</param>
        /// <returns>オブジェクトの JSON。</returns>
        public static string ToJson<T>(T graph)
        {
            return ToJson<T>(graph, Encoding.UTF8);
        }

        /// <summary>
        /// オブジェクトを JSON へ変換します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="graph">オブジェクト。</param>
        /// <param name="encoding">エンコーディング。</param>
        /// <returns>オブジェクトの JSON。</returns>
        public static string ToJson<T>(T graph, Encoding encoding)
        {
            using (var stream = new MemoryStream())
            {
                WriteObject<T>(graph, stream);
                return encoding.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// オブジェクトの JSON を指定の Stream へ書き込みます。
        /// </summary>
        /// <remarks>
        /// Stream の Close は呼び出し側の責務とします。
        /// </remarks>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="graph">JSON として書き込むオブジェクト。</param>
        /// <param name="stream">JSON の書き込み先の Stream。</param>
        public static void WriteObject<T>(T graph, Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            serializer.WriteObject(stream, graph);
        }

        /// <summary>
        /// JSON をオブジェクトへ変換します。
        /// 文字列のエンコーディングは UTF-8 であることを仮定します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="json">オブジェクトの JSON。</param>
        /// <returns>オブジェクト。</returns>
        public static T FromJson<T>(string json)
        {
            return FromJson<T>(json, Encoding.UTF8);
        }

        /// <summary>
        /// JSON をオブジェクトへ変換します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="json">オブジェクトの JSON。</param>
        /// <param name="encoding">エンコーディング。</param>
        /// <returns>オブジェクト。</returns>
        public static T FromJson<T>(string json, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(json);
            using (var stream = new MemoryStream(bytes))
            {
                return ReadObject<T>(stream);
            }
        }

        /// <summary>
        /// していの Stream から JSON を読み込み、オブジェクトへ変換します。
        /// </summary>
        /// <remarks>
        /// Stream の Close は呼び出し側の責務とします。
        /// </remarks>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="stream">JSON の読み込み元の Stream。</param>
        /// <returns>オブジェクト。</returns>
        public static T ReadObject<T>(Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            return (T) serializer.ReadObject(stream);
        }
    }
}
