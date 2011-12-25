#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// サイズを表す構造体です。
    /// </summary>
    public struct Size : IEquatable<Size>
    {
        /// <summary>
        /// 空を表す Size を取得します。
        /// </summary>
        public static Size Empty
        {
            get { return new Size(float.NegativeInfinity, float.NegativeInfinity); }
        }

        /// <summary>
        /// 幅。
        /// </summary>
        public float Width;

        /// <summary>
        /// 高さ。
        /// </summary>
        public float Height;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="width">幅。</param>
        /// <param name="height">高さ。</param>
        public Size(float width, float height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// 指定の Size が空を表すかどうかを判定します。
        /// </summary>
        /// <param name="size">判定する Size。</param>
        /// <returns>
        /// true (空を表す場合)、false (それ以外の場合)。
        /// </returns>
        public static bool IsEmpty(Size size)
        {
            return float.IsNegativeInfinity(size.Width) && float.IsNegativeInfinity(size.Height);
        }

        public static bool operator ==(Size s1, Size s2)
        {
            return s1.Equals(s2);
        }

        public static bool operator !=(Size s1, Size s2)
        {
            return !s1.Equals(s2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return Equals((Size) obj);
        }

        public bool Equals(Size other)
        {
            return Width == other.Width && Height == other.Height;
        }

        public override int GetHashCode()
        {
            return Width.GetHashCode() ^ Height.GetHashCode();
        }
    }
}
