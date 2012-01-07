#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Control の描画処理へのアクセスを提供するインタフェースです。
    /// </summary>
    public interface IDrawContext
    {
        /// <summary>
        /// Control の描画に使用する SpriteBatch を取得します。
        /// </summary>
        SpriteBatch SpriteBatch { get; }

        /// <summary>
        /// 描画処理が指定する Control の描画領域を取得します。
        /// </summary>
        Rectangle Bounds { get; }

        /// <summary>
        /// 描画処理が指定するControl の透明度を取得します。
        /// </summary>
        float Opacity { get; }

        /// <summary>
        /// Control の描画処理を反映させます。
        /// </summary>
        void Flush();
    }
}
