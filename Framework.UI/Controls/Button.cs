﻿#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    /// <summary>
    /// Button として振る舞う Control です。
    /// </summary>
    public class Button : ContentControl
    {
        /// <summary>
        /// クリックされた時に発生します。
        /// </summary>
        public static readonly string ClickEvent = "Click";

        /// <summary>
        /// Click イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        /// <summary>
        /// マウス ボタンが押された状態かどうかを示す値。
        /// </summary>
        /// <value>
        /// true (マウス ボタンが押された状態の場合)、false (それ以外の場合)。
        /// </value>
        bool pressedByMouse;

        /// <summary>
        /// Enter キーが押された状態かどうかを示す値。
        /// </summary>
        /// <value>
        /// true (Enter キーが押された状態の場合)、false (それ以外の場合)。
        /// </value>
        bool pressedByKey;

        /// <summary>
        /// Button が押された状態にあるかどうかを取得します。
        /// </summary>
        /// <value>true (Button が押された状態にある場合)、false (それ以外の場合)。</value>
        public bool Pressed
        {
            get { return MouseDirectlyOver && (pressedByMouse || pressedByKey); }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <pparam name="screen">Screen。</pparam>
        public Button(Screen screen)
            : base(screen)
        {
            Click += CreateRoutedEventHandler(OnClick);

            Enabled = true;
            Focusable = true;
        }

        protected override void OnMouseEnter(ref RoutedEventContext context)
        {
            pressedByMouse = (Screen.MouseDevice.MouseState.LeftButton == ButtonState.Pressed);
            context.Handled = true;

            base.OnMouseEnter(ref context);
        }

        protected override void OnMouseLeave(ref RoutedEventContext context)
        {
            pressedByMouse = false;
            context.Handled = true;

            base.OnMouseLeave(ref context);
        }

        /// <summary>
        /// Content 内部から発生した PreviewMouseDown を捕捉し、
        /// RoutedEventContext の Handled プロパティを true に設定します。
        /// </summary>
        /// <param name="context"></param>
        protected override void OnPreviewMouseDown(ref RoutedEventContext context)
        {
            base.OnPreviewMouseDown(ref context);
            if (context.Handled) return;

            // 機能が無効に設定されているならば、イベントを無視します。
            if (!Enabled) return;

            pressedByMouse = Screen.MouseDevice.IsButtonPressed(MouseButtons.Left);
            context.Handled = true;
        }

        /// <summary>
        /// Content 内部から発生した PreviewMouseUp を捕捉し、Click イベントを発生させ、
        /// RoutedEventContext の Handled プロパティを true に設定します。
        /// </summary>
        /// <param name="context"></param>
        protected override void OnPreviewMouseUp(ref RoutedEventContext context)
        {
            base.OnPreviewMouseUp(ref context);
            if (context.Handled) return;

            // Button が押された状態で機能が無効に設定される場合を考慮し、機能が有効かどうかに関わらず処理を進めます。
            if (Screen.MouseDevice.IsButtonReleased(MouseButtons.Left))
            {
                pressedByMouse = false;
                if (Enabled && !Pressed) RaiseEvent(null, ClickEvent);

                context.Handled = true;
            }
        }

        protected override void OnPreviewKeyDown(ref RoutedEventContext context)
        {
            base.OnPreviewKeyDown(ref context);
            if (context.Handled) return;

            // 機能が無効に設定されているならば、イベントを無視します。
            if (!Enabled) return;

            if (Screen.KeyboardDevice.IsKeyPressed(Keys.Enter) ||
                Screen.KeyboardDevice.IsKeyPressed(Keys.Space))
            {
                pressedByKey = true;
            }
        }

        protected override void OnPreviewKeyUp(ref RoutedEventContext context)
        {
            base.OnPreviewKeyUp(ref context);
            if (context.Handled) return;

            if (Screen.KeyboardDevice.IsKeyReleased(Keys.Enter) ||
                Screen.KeyboardDevice.IsKeyReleased(Keys.Space))
            {
                pressedByKey = false;
                if (Enabled && !Pressed)
                {
                    var sound = Screen.GetSound(SoundKey.Click);
                    if (sound != null)
                    {
                        if (sound.State != SoundState.Stopped) sound.Stop();
                        sound.Play();
                    }

                    RaiseEvent(null, ClickEvent);
                }
            }
        }

        /// <summary>
        /// Click イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnClick(ref RoutedEventContext context) { }
    }
}
