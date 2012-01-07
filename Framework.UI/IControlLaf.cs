﻿#region Uging

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Control の Look & Feel を描画するクラスのインタフェースです。
    /// </summary>
    public interface IControlLaf : IDisposable
    {
        /// <summary>
        /// Control の Look & Feel を描画します。
        /// </summary>
        /// <param name="control">Control。</param>
        /// <param name="drawContext">現在の IDrawContext。</param>
        void Draw(Control control, IDrawContext drawContext);
    }
}
