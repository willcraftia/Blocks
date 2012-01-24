#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    /// <summary>
    /// スプライト デバッグ用に Control の LaF を描画するための IControlLafSource です。
    /// </summary>
    public class DebugControlLafSource : ControlLafSourceBase
    {
        /// <summary>
        /// Control の型をキーとし、その Control の LaF を値とするディクショナリ。
        /// </summary>
        Dictionary<Type, DebugControlLafBase> controlLafs = new Dictionary<Type, DebugControlLafBase>();

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game">Game。</param>
        public DebugControlLafSource(Game game)
            : base(game)
        {
            // デフォルトの ControlLafBase を設定しておきます。
            RegisterControlLaf(typeof(Control), new DefaultControlLaf());
            RegisterControlLaf(typeof(Controls.TextBlock), new TextBlockLaf());
        }

        /// <summary>
        /// LaF を登録します。
        /// </summary>
        /// <param name="type">Control の型。</param>
        /// <param name="controlLaf">DebugControlLafBase。</param>
        public void RegisterControlLaf(Type type, DebugControlLafBase controlLaf)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (controlLaf == null) throw new ArgumentNullException("controlLaf");

            controlLafs[type] = controlLaf;
            controlLaf.Source = this;
        }

        /// <summary>
        /// LaF の登録を解除します。
        /// </summary>
        /// <param name="type">Control の型。</param>
        public void DeregisterControlLaf(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            DebugControlLafBase controlLaf = null;
            if (controlLafs.TryGetValue(type, out controlLaf))
            {
                controlLafs.Remove(type);
            }
        }

        public override IControlLaf GetControlLaf(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");

            var type = control.GetType();

            DebugControlLafBase controlLaf = null;
            while (type != typeof(object))
            {
                if (controlLafs.TryGetValue(type, out controlLaf)) break;

                type = type.BaseType;
            }

            return controlLaf;
        }
    }
}
