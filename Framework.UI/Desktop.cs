#region Using

using System;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Screen で管理する全ての Control のルートとなる Control です。
    /// </summary>
    public sealed class Desktop : ContentControl, IFocusScopeControl
    {
        #region InternalWindowCollection

        /// <summary>
        /// Desktop の Windows プロパティのクラスです。
        /// </summary>
        class InternalWindowCollection : WindowCollection
        {
            /// <summary>
            /// インスタンスを生成します。
            /// </summary>
            /// <param name="desktop">インスタンスを所持する Desktop。</param>
            internal InternalWindowCollection(Desktop desktop)
                : base(desktop)
            {
            }

            protected override void InsertItem(int index, Window item)
            {
                base.InsertItem(index, item);

                Desktop.AddChildInternal(item);
            }

            protected override void RemoveItem(int index)
            {
                var removedItem = this[index];
                base.RemoveItem(index);

                Desktop.RemoveChildInternal(removedItem);
            }

            protected override void SetItem(int index, Window item)
            {
                var removedItem = this[index];
                base.SetItem(index, item);

                Desktop.AddChildInternal(item);
                Desktop.RemoveChildInternal(removedItem);
            }
        }

        #endregion

        /// <summary>
        /// アクティブ化された時に発生します。
        /// </summary>
        public event EventHandler Activated = delegate { };

        /// <summary>
        /// 非アクティブ化された時に発生します。
        /// </summary>
        public event EventHandler Deactivated = delegate { };

        /// <summary>
        /// true (アクティブ化されている場合)、false (それ以外の場合)。
        /// </summary>
        bool active;

        // I/F
        public FocusScope FocusScope { get; private set; }

        /// <summary>
        /// 管理している Window のコレクションを取得します。
        /// </summary>
        public WindowCollection Windows { get; private set; }

        /// <summary>
        /// アクティブ化されているかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (アクティブ化されている場合)、false (それ以外の場合)。
        /// </value>
        public bool Active
        {
            get { return active; }
            internal set
            {
                if (active == value) return;

                active = value;

                if (active)
                {
                    OnActivated();
                }
                else
                {
                    OnDeactivated();
                }
            }
        }

        /// <summary>
        /// Content に Control が設定されているならば Window 数 + 1 を返し、
        /// それ以外ならば Window 数を返します。
        /// </summary>
        protected override int ChildrenCount
        {
            get { return Windows.Count + base.ChildrenCount; }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        internal Desktop(Screen screen)
            : base(screen)
        {
            Windows = new InternalWindowCollection(this);
            FocusScope = new FocusScope(this);
        }

        /// <summary>
        /// アクティブ化します。
        /// </summary>
        public void Activate()
        {
            Screen.ActivateDesktop(this);
        }

        /// <summary>
        /// 最前面の Window を取得します。
        /// Window が存在しない場合には null を返します。
        /// </summary>
        /// <returns>最前面の Window。Window が存在しない場合には null。</returns>
        public Window GetTopMostWindow()
        {
            if (Windows.Count == 0) return null;
            return Windows[Windows.Count - 1];
        }

        /// <summary>
        /// Content プロパティが null ではない場合、
        /// index = 0 は Content プロパティに設定された Control を指し示します。
        /// また、Windows プロパティにある Window は、index + 1 で指し示されます。
        /// Content プロパティが null の場合、
        /// index = 0 は Windows プロパティにある Window のインデックスそのものとなります。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override Control GetChild(int index)
        {
            if (index < 0 || ChildrenCount <= index)
                throw new ArgumentOutOfRangeException("index");

            if (Content == null) return Windows[index];
            return (index == 0)  ? Content : Windows[index - 1];
        }

        protected override void OnPreviewKeyDown(ref RoutedEventContext context)
        {
            base.OnPreviewKeyDown(ref context);

            // フォーカス移動のキーを優先して処理します。
            if (Screen.FocusedControl != null)
            {
                var focusScope = FocusScope.GetFocusScope(Screen.FocusedControl);
                if (focusScope != null)
                {
                    bool focusMoved = false;
                    if (Screen.KeyboardDevice.IsKeyPressed(Keys.Up))
                    {
                        focusScope.MoveFocus(FocusNavigationDirection.Up);
                        focusMoved = true;
                    }
                    else if (Screen.KeyboardDevice.IsKeyPressed(Keys.Down))
                    {
                        focusScope.MoveFocus(FocusNavigationDirection.Down);
                        focusMoved = true;
                    }
                    else if (Screen.KeyboardDevice.IsKeyPressed(Keys.Left))
                    {
                        focusScope.MoveFocus(FocusNavigationDirection.Left);
                        focusMoved = true;
                    }
                    else if (Screen.KeyboardDevice.IsKeyPressed(Keys.Right))
                    {
                        focusScope.MoveFocus(FocusNavigationDirection.Right);
                        focusMoved = true;
                    }

                    if (focusMoved)
                    {
                        context.Handled = true;
                    }
                }
            }
        }

        /// <summary>
        /// InternalWindowCollection へ Control を追加した時に呼び出され、
        /// 追加された Control を Desktop の子として関連付けます。
        /// </summary>
        /// <param name="child"></param>
        internal void AddChildInternal(Control child)
        {
            AddChild(child);
        }

        /// <summary>
        /// InternalWindowCollection から Control を削除した時に呼び出され、
        /// Desktop から削除された子 Control の関連付けを削除します。
        /// </summary>
        /// <param name="child"></param>
        internal void RemoveChildInternal(Control child)
        {
            RemoveChild(child);
        }

        /// <summary>
        /// アクティブ化された時に呼び出されます。
        /// Activated イベントを発生させます。
        /// </summary>
        void OnActivated()
        {
            if (Activated != null) Activated(this, EventArgs.Empty);
        }

        /// <summary>
        /// 非アクティブ化された時に呼び出されます。
        /// Deactivated イベントを発生させます。
        /// </summary>
        void OnDeactivated()
        {
            if (Deactivated != null) Deactivated(this, EventArgs.Empty);
        }
    }
}
