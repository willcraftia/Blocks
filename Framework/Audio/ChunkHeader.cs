#region Using

using System;
using System.IO;
using System.Text;

#endregion

namespace Willcraftia.Xna.Framework.Audio
{
    /// <summary>
    /// Chunk のヘッダ部です。
    /// </summary>
    public struct ChunkHeader
    {
        /// <summary>
        /// Id。
        /// </summary>
        public UInt32 Id;

        /// <summary>
        /// Size。
        /// </summary>
        public Int32 Size;

        /// <summary>
        /// Id プロパティを文字列として取得します。
        /// </summary>
        public string IdString
        {
            get
            {
                return Encoding.ASCII.GetString(BitConverter.GetBytes(Id));
            }
        }

        /// <summary>
        /// ChunkHeader を読み込みます。
        /// </summary>
        /// <param name="reader">BinaryReader。</param>
        /// <returns>ChunkHeader。</returns>
        public static ChunkHeader ReadFrom(BinaryReader reader)
        {
            var result = new ChunkHeader();

            result.Id = reader.ReadUInt32();
            result.Size = reader.ReadInt32();

            return result;
        }
    }
}
