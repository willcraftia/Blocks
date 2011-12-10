#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// GUI コントロールの基底クラスです。
    /// </summary>
    public class Control
    {
        /// <summary>
        /// 親 Control。
        /// </summary>
        Control parent;

        Control mouseOverControl;

        Control activatedControl;

        /// <summary>
        /// 親 Control を取得または設定します。
        /// </summary>
        public Control Parent
        {
            get { return parent; }
            set
            {
                parent = value;
            }
        }

        /// <summary>
        /// 子 Control のコレクションを取得します。
        /// </summary>
        public ControlCollection Children { get; private set; }

        /// <summary>
        /// 矩形サイズ (矩形座標は親 Control の矩形座標からの相対位置)。
        /// </summary>
        public Rectangle Bounds;

        public Appearance Appearance { get; set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public Control()
        {
            Children = new ControlCollection(this);
        }

        /// <summary>
        /// スクリーン上の絶対座標による矩形サイズを取得します。
        /// </summary>
        /// <returns>スクリーン上の絶対座標。</returns>
        public Rectangle GetAbsoluteBounds()
        {
            if (parent == null) return Bounds;

            var parentAbsoluteBounds = parent.GetAbsoluteBounds();

            var absoluteBounds = Bounds;

            absoluteBounds.X += parentAbsoluteBounds.X;
            absoluteBounds.Y += parentAbsoluteBounds.Y;

            return absoluteBounds;
        }

        internal void ProcessMouseMoved(int x, int y)
        {
            // x と y は親を基準としたカーソルの相対座標です。

            // 自分を基準としたカーソルの相対座標を算出します。
            int localX = x - Bounds.X;
            int localY = y - Bounds.Y;

            for (int i = Children.Count - 1; 0 <= i; i--)
            {
                var child = Children[i];
                if (child.Bounds.Contains(localX, localY))
                {
                    // 子をマウス オーバ状態にします。
                    switchMouseOverControl(child);
                    // 子にカーソル移動処理を転送します。
                    child.ProcessMouseMoved(localX, localY);
                    return;
                }
            }

            // マウス オーバ状態にできる子がいないならば、自分をマウス オーバ状態にします。
            switchMouseOverControl(this);
            OnMouseMoved(localX, localY);
        }

        internal void ProcessMouseLeave()
        {
            if (mouseOverControl == null) return;

            if (mouseOverControl != this)
            {
                // マウス オーバ状態の子へ処理を転送します。
                mouseOverControl.ProcessMouseLeave();
            }
            else
            {
                // 自分がマウス オーバ状態なのでイベント ハンドラを呼びます。
                OnMouseLeft();
            }

            // マウス オーバ状態を解除します。
            mouseOverControl = null;
        }

        /// <summary>
        /// マウス カーソルが移動した時に呼び出されます。
        /// </summary>
        /// <param name="x">この Control の矩形位置を基準としたカーソルの X 座標。</param>
        /// <param name="y">この Control の矩形位置を基準としたカーソルの Y 座標。</param>
        protected virtual void OnMouseMoved(int x, int y) { }

        /// <summary>
        /// マウス カーソルがこの Control に入った時 (この Control がマウス オーバ状態になった時) に呼び出されます。
        /// </summary>
        protected virtual void OnMouseEntered() { }

        /// <summary>
        /// マウス カーソルがこの Control から出た時 (この Control のマウス オーバ状態が解除された時) に呼び出されます。
        /// </summary>
        protected virtual void OnMouseLeft() { }

        /// <summary>
        /// マウス オーバ状態の Control を新しい Control へ切り替えます。
        /// </summary>
        /// <param name="newControl"></param>
        void switchMouseOverControl(Control newControl)
        {
            if (mouseOverControl == newControl) return;

            // これまでマウス オーバ状態にあった Control に変更を通知します。
            if (mouseOverControl != null) mouseOverControl.ProcessMouseLeave();

            // 新たにマウス オーバ状態となった Control を設定し、変更を通知します。
            mouseOverControl = newControl;
            mouseOverControl.OnMouseEntered();
        }
    }
}
