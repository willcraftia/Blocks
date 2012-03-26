#region Using

using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization
{
    /// <summary>
    /// Material の色を表すための構造体です。
    /// </summary>
    public struct MaterialColor
    {
        /// <summary>
        /// R 値。
        /// </summary>
        [XmlIgnore]
        public byte R;

        /// <summary>
        /// G 値。
        /// </summary>
        [XmlIgnore]
        public byte G;

        /// <summary>
        /// B 値。
        /// </summary>
        [XmlIgnore]
        public byte B;

        /// <summary>
        /// パック値を取得または設定します。
        /// </summary>
        [XmlText]
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
        /// Color へ変換します。
        /// </summary>
        /// <returns>Color。</returns>
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
