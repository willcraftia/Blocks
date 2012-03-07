#region Using

using System;
using System.IO;
using System.Text;

#endregion

namespace Willcraftia.Xna.Framework.Audio
{
    /// <summary>
    /// Wave ファイルの 'fmt' chunk。
    /// </summary>
    public struct WaveFormatChunk
    {
        /// <summary>
        /// ヘッダ部。
        /// </summary>
        public ChunkHeader Header;

        /// <summary>
        /// Format。
        /// </summary>
        public Int16 Format;

        /// <summary>
        /// Channels。
        /// </summary>
        public UInt16 Channels;

        /// <summary>
        /// SampleRate。
        /// </summary>
        public UInt32 SampleRate;

        /// <summary>
        /// BytePerSec。
        /// </summary>
        public UInt32 BytePerSec;

        /// <summary>
        /// BlockAlign。
        /// </summary>
        public UInt16 BlockAlign;

        /// <summary>
        /// BitsWidth。
        /// </summary>
        public UInt16 BitsWidth;

        /// <summary>
        /// ExtendedSize。
        /// </summary>
        public UInt16 ExtendedSize;

        /// <summary>
        /// Extended。
        /// </summary>
        public Byte[] Extended;

        /// <summary>
        /// FormatChunk を読み込みます。
        /// </summary>
        /// <param name="reader">BinaryReader。</param>
        /// <returns>FormatChunk。</returns>
        public static WaveFormatChunk ReadFrom(BinaryReader reader)
        {
            var result = new WaveFormatChunk();

            result.Header = ChunkHeader.ReadFrom(reader);
            result.Format = reader.ReadInt16();
            result.Channels = reader.ReadUInt16();
            result.SampleRate = reader.ReadUInt32();
            result.BytePerSec = reader.ReadUInt32();
            result.BlockAlign = reader.ReadUInt16();
            result.BitsWidth = reader.ReadUInt16();

            // extension
            if (result.Format == 18)
            {
                result.ExtendedSize = reader.ReadUInt16();
                result.Extended = reader.ReadBytes(result.ExtendedSize);
            }

            return result;
        }
    }
}
