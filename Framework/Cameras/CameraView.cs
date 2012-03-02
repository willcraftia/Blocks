#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Cameras
{
    /// <summary>
    /// カメラの View 行列の更新を管理するクラスです。
    /// </summary>
    public abstract class CameraView
    {
        /// <summary>
        /// カメラの View 行列。
        /// Update() メソッドの呼び出しで更新されます。
        /// </summary>
        public Matrix Matrix = Matrix.Identity;

        /// <summary>
        /// カメラの位置を取得します。
        /// </summary>
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
        public abstract void Update();
    }
}
