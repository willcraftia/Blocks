#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework
{
    /// <summary>
    /// カメラの Projection 行列を管理するクラスです。
    /// </summary>
    public abstract class Projection
    {
        /// <summary>
        /// カメラの Projection 行列。
        /// </summary>
        public Matrix Matrix = Matrix.Identity;
    }
}
