#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public interface IUIService
    {
        /// <summary>
        /// Screen を取得または設定します。
        /// </summary>
        Screen Screen { get; set; }

        void Show(string screenName);
    }
}
