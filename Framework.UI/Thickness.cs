#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// 矩形の枠の太さを表す構造体です。
    /// </summary>
    public struct Thickness : IEquatable<Thickness>
    {
        /// <summary>
        /// 左辺の幅。
        /// </summary>
        public float Left;
        
        /// <summary>
        /// 上辺の幅。
        /// </summary>
        public float Top;

        /// <summary>
        /// 右辺の幅。
        /// </summary>
        public float Right;
        
        /// <summary>
        /// 下辺の幅。
        /// </summary>
        public float Bottom;

        /// <summary>
        /// 同じ幅の四辺としてインスタンスを生成します。
        /// </summary>
        /// <param name="uniformLength"></param>
        public Thickness(float uniformLength)
        {
            Left = uniformLength;
            Right = uniformLength;
            Top = uniformLength;
            Bottom = uniformLength;
        }

        /// <summary>
        /// 指定の四辺の幅でインスタンスを生成します。
        /// </summary>
        /// <param name="left">左辺の幅。</param>
        /// <param name="top">上辺の幅。</param>
        /// <param name="right">右辺の幅。</param>
        /// <param name="bottom">下辺の幅。</param>
        public Thickness(float left, float top, float right, float bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        #region Equatable

        public static bool operator ==(Thickness t1, Thickness t2)
        {
            return t1.Equals(t2);
        }

        public static bool operator !=(Thickness t1, Thickness t2)
        {
            return !t1.Equals(t2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return Equals((Thickness) obj);
        }

        public bool Equals(Thickness other)
        {
            return Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;
        }

        public override int GetHashCode()
        {
            return Left.GetHashCode() ^ Top.GetHashCode() ^ Right.GetHashCode() ^ Bottom.GetHashCode();
        }

        #endregion
    }
}
