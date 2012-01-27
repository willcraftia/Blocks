#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// ILookAndFeel を提供するクラスのインタフェースです。
    /// </summary>
    public interface ILookAndFeelSource : IDisposable
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
        /// Control に対応する ILookAndFeel を取得します。
        /// </summary>
        /// <param name="control">Control。</param>
        /// <returns>
        /// ILookAndFeel。Control に対応する ILookAndFeel が存在しない場合は null。
        /// </returns>
        ILookAndFeel GetLookAndFeel(Control control);
    }
}
