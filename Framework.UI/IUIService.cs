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
        /// 指定の名前で定義された Screen を表示します。
        /// </summary>
        /// <param name="screenName">Screen の名前。</param>
        void Show(string screenName);

        /// <summary>
        /// 次に表示する Screen を設定します。
        /// このメソッドで設定した Screen は、
        /// 現在表示対象の Screen の更新処理が終わってから表示対象へ設定されます。
        /// </summary>
        /// <param name="screen">次に表示する Screen。</param>
        void PrepareNextScreen(Screen nextScreen);
    }
}
