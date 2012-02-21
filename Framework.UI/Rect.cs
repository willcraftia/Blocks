#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// 矩形を表す構造体です。
    /// </summary>
    public struct Rect : IEquatable<Rect>
    {
        /// <summary>
        /// 空を表す Rect を取得します。
        /// </summary>
        public static Rect Empty
        {
            get { return new Rect(float.PositiveInfinity, float.PositiveInfinity, float.NegativeInfinity, float.NegativeInfinity); }
        }

        /// <summary>
        /// X 座標。
        /// </summary>
        public float X;

        /// <summary>
        /// Y 座標。
        /// </summary>
        public float Y;
        
        /// <summary>
        /// 幅。
        /// </summary>
        public float Width;
        
        /// <summary>
        /// 高さ。
        /// </summary>
        public float Height;

        /// <summary>
        /// 左辺の位置を取得します。
        /// </summary>
        public float Left
        {
            get { return X; }
        }

        /// <summary>
        /// 上辺の位置を取得します。
        /// </summary>
        public float Top
        {
            get { return Y; }
        }

        /// <summary>
        /// 右辺の位置を取得します。
        /// </summary>
        public float Right
        {
            get { return X + Width; }
        }

        /// <summary>
        /// 下辺の位置を取得します。
        /// </summary>
        public float Bottom
        {
            get { return Y + Height; }
        }

        /// <summary>
        /// 左上の位置を取得します。
        /// </summary>
        public Vector2 TopLeft
        {
            get { return new Vector2(Left, Top); }
        }

        /// <summary>
        /// 右上の位置を取得します。
        /// </summary>
        public Vector2 TopRight
        {
            get { return new Vector2(Right, Top); }
        }

        /// <summary>
        /// 左下の位置を取得します。
        /// </summary>
        public Vector2 BottomLeft
        {
            get { return new Vector2(Left, Bottom); }
        }

        /// <summary>
        /// 右下の位置を取得します。
        /// </summary>
        public Vector2 BottomRight
        {
            get { return new Vector2(Right, Bottom); }
        }

        /// <summary>
        /// サイズを取得します。
        /// </summary>
        public Size Size
        {
            get { return new Size(Width, Height); }
        }

        /// <summary>
        /// X と Y が 0 で指定のサイズを持つインスタンスを生成します。
        /// </summary>
        /// <param name="size">サイズ。</param>
        public Rect(Size size) : this(0, 0, size.Width, size.Height) { }

        /// <summary>
        /// 指定の位置とサイズを持つインスタンスを生成します。
        /// </summary>
        /// <param name="point">左上の位置。</param>
        /// <param name="size">サイズ。</param>
        public Rect(Vector2 point, Size size) : this(point.X, point.Y, size.Width, size.Height) { }

        /// <summary>
        /// 指定の位置とサイズを持つインスタンスを生成します。
        /// </summary>
        /// <param name="x">X 座標。</param>
        /// <param name="y">Y 座標。</param>
        /// <param name="width">幅。</param>
        /// <param name="height">高さ。</param>
        public Rect(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// 指定の Rect が空を表すかどうかを判定します。
        /// </summary>
        /// <param name="rect">判定する Rect。</param>
        /// <returns>
        /// true (空を表す場合)、false (それ以外の場合)。
        /// </returns>
        public static bool IsEmpty(Rect rect)
        {
            return float.IsPositiveInfinity(rect.X) && float.IsPositiveInfinity(rect.Y)
                && float.IsNegativeInfinity(rect.Width) && float.IsNegativeInfinity(rect.Height);
        }

        /// <summary>
        /// Microsoft.Xna.Framework.Rectangle から Rect を生成します。
        /// </summary>
        /// <param name="rectangle">Microsoft.Xna.Framework.Rectangle。</param>
        /// <returns>生成された Rect。</returns>
        public static Rect FromXnaRectangle(Microsoft.Xna.Framework.Rectangle rectangle)
        {
            return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// 指定の点が領域に含まれるかどうかを判定します。
        /// </summary>
        /// <param name="point">点。</param>
        /// <returns>
        /// true (領域に含まれる場合)、false (それ以外の場合)。
        /// </returns>
        public bool Contains(Vector2 point)
        {
            return Contains(point.X, point.Y);
        }

        /// <summary>
        /// 指定の座標が領域に含まれるかどうかを判定します。
        /// </summary>
        /// <param name="x">X 座標。</param>
        /// <param name="y">Y 座標。</param>
        /// <returns>
        /// true (領域に含まれる場合)、false (それ以外の場合)。
        /// </returns>
        public bool Contains(float x, float y)
        {
            return Left <= x && x <= Right && Top <= y && y <= Bottom;
        }

        /// <summary>
        /// 指定の Rect が領域に完全に含まれるかどうかを判定します。
        /// </summary>
        /// <param name="rect">Rect。</param>
        /// <returns>
        /// true (領域に含まれる場合)、false (それ以外の場合)。
        /// </returns>
        public bool Contains(Rect rect)
        {
            return Contains(rect.TopLeft) && Contains(rect.BottomRight);
        }

        /// <summary>
        /// Rectangle を生成します。
        /// </summary>
        /// <returns>生成された Rectangle。</returns>
        public Rectangle ToXnaRectangle()
        {
            return new Rectangle((int) X, (int) Y, (int) Width, (int) Height);
        }

        #region Equatable

        public static bool operator ==(Rect r1, Rect r2)
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(Rect r1, Rect r2)
        {
            return !r1.Equals(r2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return Equals((Rect) obj);
        }

        public bool Equals(Rect other)
        {
            return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();
        }

        #endregion
    }
}
