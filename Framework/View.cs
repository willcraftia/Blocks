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
        /// Update メソッドで更新されます。
        /// </summary>
        public Matrix Matrix = Matrix.Identity;

        /// <summary>
        /// Matrix プロパティの値が有効であるかどうかを示す値を取得します。
        /// Matrix プロパティの再計算を必要とするプロパティが変更されると、
        /// このプロパティが true に設定されます。
        /// このプロパティが true の場合にのみ、
        /// Update メソッドは Matrix プロパティの再計算を行います。
        /// サブクラスで MatrixDirty プロパティによる制御が不要な場合は
        /// MatrixDirty プロパティが常に true であるように実装します。
        /// </summary>
        public bool MatrixDirty { get; protected set; }

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

        /// <summary>
        /// Matrix プロパティを更新します。
        /// </summary>
        public void Update()
        {
            if (MatrixDirty) UpdateOverride();
        }

        /// <summary>
        /// MatrixDirty プロパティが true の場合に Update メソッドから呼び出されます。
        /// </summary>
        protected abstract void UpdateOverride();
    }
}
