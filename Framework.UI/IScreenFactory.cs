#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Screen の名前に対応する Screen インスタンスを生成するクラスです。
    /// </summary>
    public interface IScreenFactory : IDisposable
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
        /// 指定された Screen の名前に対応する Screen インスタンスを生成します。
        /// </summary>
        /// <param name="screenName">Screen の名前。</param>
        /// <returns>生成された Screen インスタンス。</returns>
        Screen CreateScreen(string screenName);
    }
}
