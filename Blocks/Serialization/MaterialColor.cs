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
    public struct MaterialColor : IEquatable<MaterialColor>
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

        /// <summary>
        /// Vector3 へ変換します。
        /// </summary>
        /// <returns>Vector3。</returns>
        public Vector3 ToVector3()
        {
            return new Vector3(R / 255.0f, G / 255.0f, B / 255.0f);
        }

        #region Equatable

        public static bool operator ==(MaterialColor o1, MaterialColor o2)
        {
            return o1.Equals(o2);
        }

        public static bool operator !=(MaterialColor o1, MaterialColor o2)
        {
            return !o1.Equals(o2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return Equals((MaterialColor) obj);
        }

        public bool Equals(MaterialColor other)
        {
            return R == other.R && G == other.G && B == other.B;
        }

        public override int GetHashCode()
        {
            return R.GetHashCode() ^ G.GetHashCode() ^ B.GetHashCode();
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            return "[" + R + ", " + G + ", " + B + "]";
        }

        #endregion
    }
}
