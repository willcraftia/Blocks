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
        /// オブジェクトを JSON 文字列へ変換します。
        /// 文字列のエンコーディングは UTF-8 であることを仮定します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="graph">オブジェクト。</param>
        /// <returns>オブジェクトの JSON 文字列。</returns>
        public static string ToJson<T>(T graph)
        {
            return ToJson<T>(graph, Encoding.UTF8);
        }

        /// <summary>
        /// オブジェクトを JSON 文字列へ変換します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="graph">オブジェクト。</param>
        /// <param name="encoding">エンコーディング。</param>
        /// <returns>オブジェクトの JSON 文字列。</returns>
        public static string ToJson<T>(T graph, Encoding encoding)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, graph);
                return encoding.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// JSON 文字列をオブジェクトへ変換します。
        /// 文字列のエンコーディングは UTF-8 であることを仮定します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="json">オブジェクトの JSON 文字列。</param>
        /// <returns>オブジェクト。</returns>
        public static T FromJson<T>(string json)
        {
            return FromJson<T>(json, Encoding.UTF8);
        }

        /// <summary>
        /// JSON 文字列をオブジェクトへ変換します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="json">オブジェクトの JSON 文字列。</param>
        /// <param name="encoding">エンコーディング。</param>
        /// <returns>オブジェクト。</returns>
        public static T FromJson<T>(string json, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(json);
            using (var stream = new MemoryStream(bytes))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T) serializer.ReadObject(stream);
            }
        }
    }
}
