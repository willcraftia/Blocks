#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// IControlLaf を提供するクラスのインタフェースです。
    /// </summary>
    public interface IControlLafSource : IDisposable
    {
        /// <summary>
        /// Initialize メソッドが呼び出されたかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (Initialize メソッドが呼び出された場合)、false (それ以外の場合)。
        /// </value>
        bool Initialized { get; }

        /// <summary>
        /// 初期化します。
        /// </summary>
        void Initialize();

        /// <summary>
        /// 指定の Control の Leek & Feel の描画に使用する IControlLaf を取得します。
        /// </summary>
        /// <param name="control">Control。</param>
        /// <returns>指定の Control の Leek & Feel の描画に使用する IControlLaf。</returns>
        IControlLaf GetControlLaf(Control control);
    }
}
