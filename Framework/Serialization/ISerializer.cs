#region Using

using System;
using System.IO;

#endregion

namespace Willcraftia.Xna.Framework.Serialization
{
    /// <summary>
    /// オブジェクトのシリアライズとデシリアライズを担うクラスへのインタフェースです。
    /// </summary>
    /// <typeparam name="T">シリアライズとデシリアライズの対象となる型。</typeparam>
    public interface ISerializer<T>
    {
        /// <summary>
        /// デシリアライズします。
        /// </summary>
        /// <param name="stream">シリアライズされたオブジェクトを提供する Stream。</param>
        /// <returns>デシリアライズされたオブジェクト。</returns>
        T Deserialize(Stream stream);

        /// <summary>
        /// シリアライズします。
        /// </summary>
        /// <param name="stream">オブジェクトのシリアライズ先となる Stream。</param>
        /// <param name="o">シリアライズするオブジェクト。</param>
        void Serialize(Stream stream, T o);
    }
}
