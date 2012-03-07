#region Using

using System;
using System.IO;
using System.Text;

#endregion

namespace Willcraftia.Xna.Framework.Audio
{
    /// <summary>
    /// 'riff' chunk。
    /// </summary>
    public struct RiffChunk
    {
        /// <summary>
        /// ヘッダ部。
        /// </summary>
        public ChunkHeader Header;

        /// <summary>
        /// Type。
        /// </summary>
        public UInt32 Type;

        /// <summary>
        /// Type プロパティを文字列として取得します。
        /// </summary>
        public string TypeString
        {
            get
            {
                return Encoding.ASCII.GetString(BitConverter.GetBytes(Type));
            }
        }

        /// <summary>
        /// RiffChunk を読み込みます。
        /// </summary>
        /// <param name="reader">BinaryReader。</param>
        /// <returns>RiffChunk。</returns>
        public static RiffChunk ReadFrom(BinaryReader reader)
        {
            var result = new RiffChunk();

            result.Header = ChunkHeader.ReadFrom(reader);
            result.Type = reader.ReadUInt32();

            return result;
        }
    }
}
