#region Using

using System;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization
{
    /// <summary>
    /// Material の色を表すための構造体です。
    /// </summary>
    [DataContract]
    public struct MaterialColor
    {
        /// <summary>
        /// R 値。
        /// </summary>
        public byte R;

        /// <summary>
        /// G 値。
        /// </summary>
        public byte G;

        /// <summary>
        /// B 値。
        /// </summary>
        public byte B;

        /// <summary>
        /// パック値を取得または設定します。
        /// </summary>
        [DataMember]
        public uint PackedValue
        {
            get { return (uint) B + (uint) (G << 8) + (uint) (R << 16); }
            set
            {
                B = (byte) (value);
                G = (byte) (value >> 8);
                R = (byte) (value >> 16);
            }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="r">R 値。</param>
        /// <param name="g">G 値。</param>
        /// <param name="b">B 値。</param>
        public MaterialColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Color 構造体へ変換します。
        /// </summary>
        /// <returns>Color 構造体。</returns>
        public Color ToColor()
        {
            return new Color(R, G, B);
        }

        #region ToString

        public override string ToString()
        {
            return "[" + R + ", " + G + ", " + B + "]";
        }

        #endregion
    }
}
