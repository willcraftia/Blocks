#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// UI サービスのインタフェースです。
    /// </summary>
    public interface IUIService
    {
        /// <summary>
        /// Screen を取得または設定します。
        /// </summary>
        /// <remarks>
        /// Screen プロパティに設定した Screen は即座に表示対象となります。
        /// </remarks>
        Screen Screen { get; set; }

        /// <summary>
        /// 指定の名前で定義された Screen を表示します。
        /// </summary>
        /// <param name="screenName">Screen の名前。</param>
        void Show(string screenName);
    }
}
