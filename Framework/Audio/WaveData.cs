#region Using

using System;
using System.IO;

#endregion

namespace Willcraftia.Xna.Framework.Audio
{
    /// <summary>
    /// Wave ファイルのデータ構造です。
    /// </summary>
    public struct WaveData
    {
        /// <summary>
        /// 'riff' chunk。
        /// </summary>
        public RiffChunk Riff;

        /// <summary>
        /// 'format' chunk。
        /// </summary>
        public WaveFormatChunk Format;

        /// <summary>
        /// 'data' chunk。
        /// </summary>
        public WaveDataChunk Data;

        /// <summary>
        /// WaveData を読み込みます。
        /// </summary>
        /// <param name="reader">Wave ファイルの BinaryReader。</param>
        /// <returns>WaveData。</returns>
        public static WaveData ReadFrom(BinaryReader reader)
        {
            var result = new WaveData();

            result.Riff = RiffChunk.ReadFrom(reader);
            result.Format = WaveFormatChunk.ReadFrom(reader);
            result.Data = WaveDataChunk.ReadFrom(reader);

            return result;
        }
    }
}
