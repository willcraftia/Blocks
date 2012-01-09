#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// 点を表す構造体です。
    /// </summary>
    public struct Point : IEquatable<Point>
    {
        /// <summary>
        /// X 座標。
        /// </summary>
        public float X;

        /// <summary>
        /// Y 座標。
        /// </summary>
        public float Y;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="x">X 座標。</param>
        /// <param name="y">Y 座標。</param>
        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        #region Equatable

        public static bool operator ==(Point p1, Point p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !p1.Equals(p2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return Equals((Point) obj);
        }

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        #endregion
    }
}
