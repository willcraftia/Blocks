#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    /// <summary>
    /// Content プロパティにあらかじめ TextBlock が設定されている Button です。
    /// </summary>
    public class TextButton : Button
    {
        /// <summary>
        /// Content プロパティに設定されている TextBlock を取得します。
        /// </summary>
        /// <remarks>
        /// Content プロパティに明示的に TextBlock 以外のインスタンスを設定した場合、
        /// このプロパティへのアクセスで例外が発生します。
        /// </remarks>
        public TextBlock TextBlock
        {
            get { return Content as TextBlock; }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        public TextButton(Screen screen)
            : base(screen)
        {
            Content = new TextBlock(screen);
        }
    }
}
