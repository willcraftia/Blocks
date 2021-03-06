﻿#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework
{
    /// <summary>
    /// View 行列を管理するクラスです。
    /// </summary>
    public abstract class View
    {
        /// <summary>
        /// View 行列。
        /// </summary>
        public Matrix Matrix = Matrix.Identity;

        /// <summary>
        /// Matrix プロパティを更新します。
        /// </summary>
        public abstract void Update();
    }
}
