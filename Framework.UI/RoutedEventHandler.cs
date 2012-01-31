#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// ルーティング イベントの到達で呼び出されるメソッドを表します。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="context"></param>
    public delegate void RoutedEventHandler(Control sender, ref RoutedEventContext context);
}
