#region Using

using System;
using System.IO;

#endregion

namespace Willcraftia.Xna.Framework.Audio
{
    /// <summary>
    /// Wave ファイルの 'data' chunk。
    /// </summary>
    public struct WaveDataChunk
    {
        /// <summary>
        /// ChunkHeader。
        /// </summary>
        public ChunkHeader Header;

        /// <summary>
        /// Data。
        /// </summary>
        public Byte[] Data;

        /// <summary>
        /// DataChunk を読み込みます。
        /// </summary>
        /// <param name="reader">BinaryReader。</param>
        /// <returns>DataChunk。</returns>
        public static WaveDataChunk ReadFrom(BinaryReader reader)
        {
            var result = new WaveDataChunk();

            result.Header = ChunkHeader.ReadFrom(reader);
            result.Data = reader.ReadBytes(result.Header.Size);

            return result;
        }
    }
}
