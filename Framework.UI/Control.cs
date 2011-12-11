#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.Input;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Visuals;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// GUI コントロールの基底クラスです。
    /// </summary>
    public class Control
    {
        /// <summary>
        /// Enabled プロパティが変更された時に発生します。
        /// </summary>
        public event EventHandler EnabledChanged;

        /// <summary>
        /// Visible プロパティが変更された時に発生します。
        /// </summary>
        public event EventHandler VisibleChanged;

        /// <summary>
        /// 矩形サイズ (矩形座標は親 Control の矩形座標からの相対位置)。
        /// </summary>
        public Rectangle Bounds;

        /// <summary>
        /// 背景色。
        /// </summary>
        public Color BackgroundColor = Color.White;

        /// <summary>
        /// 親 Control。
        /// </summary>
        Control parent;

        /// <summary>
        /// 自身あるいは子についてのマウス オーバ状態の Control。
        /// </summary>
        Control mouseOverControl;

        /// <summary>
        /// true (Control が有効な場合)、false (それ以外の場合)。
        /// </summary>
        bool enabled;

        /// <summary>
        /// true (Control が可視の場合)、false (それ以外の場合)。
        /// </summary>
        bool visible;

        /// <summary>
        /// 親 Control を取得または設定します。
        /// </summary>
        public Control Parent
        {
            get { return parent; }
            internal set
            {
                if (parent == value) return;

                parent = value;

                if (parent != null)
                {
                    // 親と同じ UIContext にバインドします。
                    if (parent.UIContext != null) parent.UIContext.Bind(this);
                }
                else
                {
                    // 親を失ったので UIContext からアンバインドします。
                    if (UIContext != null) UIContext.Unbind(this);
                }
            }
        }

        /// <summary>
        /// UIContext を取得します。
        /// </summary>
        public IUIContext UIContext { get; internal set; }

        /// <summary>
        /// 子 Control のコレクションを取得します。
        /// </summary>
        public ControlCollection Children { get; private set; }

        /// <summary>
        /// Control が有効かどうかを取得または設定します。
        /// </summary>
        /// <value>true (Control が有効な場合)、false (それ以外の場合)。</value>
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled == value) return;

                enabled = value;

                // イベントを発生させます。
                RaiseEnabledChanged();
            }
        }

        /// <summary>
        /// Control が表示されるかどうかを取得または設定します。
        /// </summary>
        /// <value>true (Control が表示される場合)、false (それ以外の場合)。</value>
        public bool Visible
        {
            get { return visible; }
            set
            {
                if (visible == value) return;

                visible = value;

                // イベントを発生させます。
                OnVisibleChanged();
            }
        }

        /// <summary>
        /// Control がフォーカスを得られるかどうかを取得または設定します。
        /// </summary>
        /// <value>true (Control がフォーカスを得られる場合)、false (それ以外の場合)。</value>
        public bool Focusable { get; set; }

        /// <summary>
        /// Control がフォーカスを得ているかどうかを取得します。
        /// </summary>
        /// <value>true (Control がフォーカスを得ている場合)、false (それ以外の場合)。</value>
        public bool Focused
        {
            get { return UIContext != null && UIContext.HasFocus(this); }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public Control()
        {
            Children = new ControlCollection(this);
            Visible = true;
            Focusable = true;
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

        public void Focus()
        {
            if (UIContext == null || !Enabled || !Visible || !Focusable) return;

            UIContext.Focus(this);
        }

        public void Defocus()
        {
            if (UIContext == null || !UIContext.HasFocus(this)) return;

            UIContext.Defocus(this);
        }

        public virtual void Draw()
        {
            // ここでは IControlLaf のみで描画します。
            // 異なる方法で Control を描画したい場合には、そのための Control のサブクラスと IControlLaf の実装を作るか、
            // あるいは Draw メソッドをオーバライドして描画ロジックを記述します。

            if (UIContext == null || !Visible) return;

            // IControlLaf を取得します。
            var laf = UIContext.GetControlLaf(this);
            // IControlLaf に描画を委譲します。
            if (laf != null) laf.Draw(this);
        }

        internal void ProcessMouseMoved(int x, int y)
        {
            // 不可視の場合は処理しません。
            if (!Visible) return;

            // x と y は親を基準としたカーソルの相対座標です。

            // 自分を基準としたカーソルの相対座標を算出します。
            int localX = x - Bounds.X;
            int localY = y - Bounds.Y;

            for (int i = Children.Count - 1; 0 <= i; i--)
            {
                var child = Children[i];
                if (child.Visible && child.Bounds.Contains(localX, localY))
                {
                    // 子をマウス オーバ状態にします。
                    SwitchMouseOverControl(child);
                    // 子にカーソル移動処理を転送します。
                    child.ProcessMouseMoved(localX, localY);
                    return;
                }
            }

            // マウス オーバ状態にできる子がいないならば、自分をマウス オーバ状態にします。
            SwitchMouseOverControl(this);
            OnMouseMoved(localX, localY);
        }

        internal void ProcessMouseLeft()
        {
            // 不可視の場合は処理しません。
            if (!Visible) return;

            if (mouseOverControl == null) return;

            if (mouseOverControl != this)
            {
                // マウス オーバ状態の子へ処理を転送します。
                mouseOverControl.ProcessMouseLeft();
            }
            else
            {
                // 自分がマウス オーバ状態なのでイベント ハンドラを呼びます。
                OnMouseLeft();
            }

            // マウス オーバ状態を解除します。
            mouseOverControl = null;
        }

        internal bool ProcessMouseButtonPressed(MouseButtons button)
        {
            // 不可視の場合は処理しません。
            if (!Visible) return false;

            // 子がマウス オーバ状態ならば処理を転送します。
            if (mouseOverControl != this) return mouseOverControl.ProcessMouseButtonPressed(button);

            // フォーカスを得ます。
            Focus();

            // マウス ボタンが押されたことを通知します。
            OnMouseButtonPressed(button);
            return true;
        }

        internal void ProcessMouseButtonReleased(MouseButtons button)
        {
            // 不可視の場合は処理しません。
            if (!Visible) return;

            // 子がマウス オーバ状態ならば処理を転送します。
            if (mouseOverControl != this)
            {
                mouseOverControl.ProcessMouseButtonReleased(button);
            }
            else
            {
                // マウス ボタンが離されたことを通知します。
                OnMouseButtonReleased(button);
            }
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
        /// マウス ボタンがこの Control で押された時に呼び出されます。
        /// </summary>
        /// <param name="button"></param>
        protected virtual void OnMouseButtonPressed(MouseButtons button) { }

        /// <summary>
        /// マウス ボタンがこの Control で離された時に呼び出されます。
        /// </summary>
        /// <param name="button"></param>
        protected virtual void OnMouseButtonReleased(MouseButtons button) { }

        /// <summary>
        /// Enabled プロパティが変更された時に呼び出されます。
        /// </summary>
        protected virtual void OnEnabledChanged() { }

        /// <summary>
        /// Visible プロパティが変更された時に呼び出されます。
        /// </summary>
        protected virtual void OnVisibleChanged() { }

        /// <summary>
        /// マウス オーバ状態の Control を新しい Control へ切り替えます。
        /// </summary>
        /// <param name="newControl"></param>
        void SwitchMouseOverControl(Control newControl)
        {
            if (mouseOverControl == newControl) return;

            // これまでマウス オーバ状態にあった Control に変更を通知します。
            if (mouseOverControl != null) mouseOverControl.ProcessMouseLeft();

            // 新たにマウス オーバ状態となった Control を設定し、変更を通知します。
            mouseOverControl = newControl;
            mouseOverControl.OnMouseEntered();
        }

        /// <summary>
        /// EnabledChanged イベントを発生させます。
        /// </summary>
        void RaiseEnabledChanged()
        {
            OnEnabledChanged();
            if (EnabledChanged != null) EnabledChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// VisibleChanged イベントを発生させます。
        /// </summary>
        void RaiseVisibleChanged()
        {
            OnVisibleChanged();
            if (VisibleChanged != null) VisibleChanged(this, EventArgs.Empty);
        }
    }
}
