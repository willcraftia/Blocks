#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    /// <summary>
    /// Overlay を利用したダイアログの基底クラスです。
    /// Show() メソッドの呼び出しで Desktop を Overlay で覆うことで、
    /// 擬似モーダル ダイアログを作ることができます。
    /// </summary>
    public class OverlayDialogBase : Window
    {
        /// <summary>
        /// ダイアログの背景を覆う Overlay を取得します。
        /// </summary>
        public Overlay Overlay { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        protected OverlayDialogBase(Screen screen)
            : base(screen)
        {
            Overlay = new Overlay(Screen)
            {
                Owner = this
            };
        }

        public override void Show()
        {
            Overlay.Show();
            base.Show();
        }
    }
}
