#region Using

using System;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Window を表す Control です。
    /// </summary>
    public class Window : Control
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public Window() { }

        /// <summary>
        /// Window を表示します。
        /// </summary>
        /// <remarks>
        /// 親 Control に追加するだけでも Window は表示対象になりますが、
        /// その場合にはマウス カーソルの再判定処理が発生しません。
        /// マウス カーソルの再判定処理を伴わせないと、
        /// Window により覆われたにも関わらず Control がマウス オーバ状態を維持し続ける場合があります。
        /// この時、本来マウス入力を受けるべき Window ではなく、
        /// 不正にマウス オーバ状態を維持している Control にマウス入力が奪われます。
        /// この問題を避けるために、Show メソッドの呼び出しで Window を表示します。
        /// Show メソッドは、親 Control に Window を追加すると共に、
        /// Screen に対してマウス カーソルの再判定処理を実行させます。
        /// これにより、マウス オーバ状態の Control を覆うように Window が表示された場合に、
        /// 覆われた Control のマウス オーバ状態が正しく開放されます。
        /// </remarks>
        /// <param name="screen"></param>
        public void Show(Screen screen)
        {
            screen.Children.Add(this);
            screen.NotifyWindowShown();
        }

        /// <summary>
        /// Window を閉じます。
        /// </summary>
        public void Close()
        {
            // 親のリストから削除します。
            Parent.Children.Remove(this);
        }

        protected override void OnMouseButtonPressed(MouseButtons button)
        {
            Parent.Children.MoveToTopMost(this);
        }
    }
}
