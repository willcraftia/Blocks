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

        /// <summary>
        /// この Window を所有する Window。
        /// </summary>
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
        /// モーダル Window であるかどうかを判定します。
        /// </summary>
        /// <value>
        /// true (モーダル Window である場合)、false (それ以外の場合)。
        /// </value>
        public bool Modal { get; private set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public Window() { }

        /// <summary>
        /// Window を表示します。
        /// </summary>
        /// <param name="screen"></param>
        public void Show(Screen screen)
        {
            // Screen へ登録します。
            screen.Children.Add(this);
        }

        /// <summary>
        /// モーダル Window を表示します。
        /// </summary>
        /// <param name="screen"></param>
        public void ShowDialog(Screen screen)
        {
            // モーダルに設定します。
            Modal = true;
            // Screen へ登録します。
            screen.Children.Add(this);
        }

        /// <summary>
        /// Window を閉じます。
        /// </summary>
        public void Close()
        {
            // Closing イベントを発生させます。
            RaiseClosing();
            // Screen から登録を解除します。
            Screen.Children.Remove(this);
            // Closed イベントを発生させます。
            RaiseClosed();
        }

        /// <summary>
        /// マウス ボタンがこの Control で押された時に呼び出されます。
        /// </summary>
        /// <remarks>
        /// Window 上でマウス ボタンが押下されると、Window は自身を最前面へ移動させます。
        /// </remarks>
        /// <param name="button"></param>
        protected override void OnMouseButtonPressed(MouseButtons button)
        {
            Parent.Children.MoveToTopMost(this);
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
