#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework
{
    /// <summary>
    /// カメラの View 行列を管理するクラスです。
    /// </summary>
    public abstract class View
    {
        /// <summary>
        /// カメラの View 行列。
        /// </summary>
        public Matrix Matrix = Matrix.Identity;

        /// <summary>
        /// カメラの位置を取得します。
        /// </summary>
        /// <remarks>
        /// View 行列の逆行列を算出してから、
        /// その Translation プロパティを取得しているため、
        /// 繰り返し処理の中で頻繁に呼び出す場合には注意が必要です。
        /// </remarks>
        public Vector3 Position
        {
            get
            {
                Matrix inverse;
                Matrix.Invert(ref Matrix, out inverse);
                return inverse.Translation;
            }
        }
    }
}
