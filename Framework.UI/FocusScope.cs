#region Using

using System;
using Microsoft.Xna.Framework.Audio;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// 論理フォーカスの範囲を管理するクラスです。
    /// </summary>
    public sealed class FocusScope
    {
        /// <summary>
        /// FocusScope を管理する Control。
        /// </summary>
        Control owner;

        /// <summary>
        /// 論理フォーカスを取得します。
        /// </summary>
        public Control FocusedControl { get; private set; }

        /// <summary>
        /// フォーカスの移動方法を取得または設定します。
        /// </summary>
        public FocusNavigationMode FocusNavigationMode { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="owner">FocusScope を管理する Control。</param>
        public FocusScope(Control owner)
        {
            if (owner == null) throw new ArgumentNullException("owner");
            this.owner = owner;

            FocusNavigationMode = FocusNavigationMode.Cycle;
        }

        /// <summary>
        /// control が属する FocusScope を取得します。
        /// </summary>
        /// <param name="control">Control。</param>
        /// <returns>
        /// control が属する FocusScope。属する FocusScope が存在しない場合は null。
        /// </returns>
        public static FocusScope GetFocusScope(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");

            var focusScopeControl = control as IFocusScopeControl;
            if (focusScopeControl != null) return focusScopeControl.FocusScope;

            if (control.Parent != null) return GetFocusScope(control.Parent);

            return null;
        }

        /// <summary>
        /// control へフォーカスの設定を試みます。
        /// </summary>
        /// <param name="control">フォーカスを設定する Control。</param>
        /// <returns>
        /// true (フォーカスの設定に成功した場合)、false (それ以外の場合)。
        /// </returns>
        public bool MoveFocusTo(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            if (!control.IsDescendantOf(owner))
                throw new InvalidOperationException("control is not a descendant of the focus scope owner.");

            // 論理フォーカスを持っていた Control を解除します。
            if (FocusedControl != null) FocusedControl.LogicalFocused = false;

            // 論理フォーカスを設定します。
            FocusedControl = control;
            FocusedControl.LogicalFocused = true;

            // Screen へフォーカス設定を依頼します。
            return owner.Screen.MoveFocusTo(FocusedControl);
        }

        /// <summary>
        /// 指定の方向にある Control へフォーカスの設定を試みます。
        /// </summary>
        /// <param name="direction">フォーカスの移動方向。</param>
        /// <returns>
        /// true (フォーカスの設定に成功した場合)、false (それ以外の場合)。
        /// </returns>
        public bool MoveFocus(FocusNavigationDirection direction)
        {
            var next = owner.GetFocusableControl(direction, FocusNavigationMode);
            if (next == null) return false;

            var result = MoveFocusTo(next);
            
            if (result)
            {
                var sound = owner.Screen.GetSound(SoundKey.FocusNavigation);
                if (sound != null)
                {
                    if (sound.State != SoundState.Stopped) sound.Stop();
                    sound.Play();
                }
            }

            return result;
        }
    }
}
