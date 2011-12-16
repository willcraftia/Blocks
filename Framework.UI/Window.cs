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
        /// Window が閉じる前に発生します。
        /// </summary>
        public event EventHandler Closing;

        /// <summary>
        /// Window が閉じた後に発生します。
        /// </summary>
        public event EventHandler Closed;

        Window owner;

        /// <summary>
        /// この Window を所有する Window を取得あるいは設定します。
        /// </summary>
        /// <remarks>
        /// Window は、Owner で設定した Window が閉じられると自動的に閉じます。
        /// </remarks>
        public Window Owner
        {
            get { return owner; }
            set
            {
                if (owner == value) return;

                if (owner != null)
                {
                    owner.Closing -= new EventHandler(OnOwnerClosing);
                }

                owner = value;

                if (owner != null)
                {
                    owner.Closing += new EventHandler(OnOwnerClosing);
                }
            }
        }

        /// <summary>
        /// この Window を所有する Window が閉じられる前に呼び出され、この Window を閉じます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnOwnerClosing(object sender, EventArgs e)
        {
            Close();
        }

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
            screen.Container.Children.Add(this);
            screen.NotifyWindowShown();
        }

        /// <summary>
        /// Window を閉じます。
        /// </summary>
        public void Close()
        {
            // Closing イベントを発生させます。
            RaiseClosing();
            // 親のリストから削除します。
            Parent.Children.Remove(this);
            // Closed イベントを発生させます。
            RaiseClosed();
        }

        protected override void OnMouseButtonPressed(MouseButtons button)
        {
            Parent.Children.MoveToTopMost(this);
        }

        /// <summary>
        /// Closing イベントを発生させます。
        /// </summary>
        void RaiseClosing()
        {
            if (Closing != null) Closing(this, EventArgs.Empty);
        }

        /// <summary>
        /// Closed イベントを発生させます。
        /// </summary>
        void RaiseClosed()
        {
            if (Closed != null) Closed(this, EventArgs.Empty);
        }
    }
}
