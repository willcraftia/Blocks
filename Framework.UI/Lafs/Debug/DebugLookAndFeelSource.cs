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
    /// デバッグ用の ILookAndFeelSource です。
    /// </summary>
    public class DebugLookAndFeelSource : LookAndFeelSourceBase
    {
        /// <summary>
        /// Control の型をキーとし、対応する LookAndFeelBase を値とするマップ。
        /// </summary>
        Dictionary<Type, LookAndFeelBase> lookAndFeelMap = new Dictionary<Type, LookAndFeelBase>();

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game">Game。</param>
        public DebugLookAndFeelSource(Game game)
            : base(game)
        {
            Register(typeof(Control), new DefaultLookAndFeel());
            Register(typeof(Controls.TextBlock), new TextBlockLookAndFeel());
        }

        /// <summary>
        /// LookAndFeelBase を登録します。
        /// </summary>
        /// <param name="type">Control の型。</param>
        /// <param name="lookAndFeel">LookAndFeelBase。</param>
        public void Register(Type type, LookAndFeelBase lookAndFeel)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (lookAndFeel == null) throw new ArgumentNullException("lookAndFeel");

            lookAndFeelMap[type] = lookAndFeel;
        }

        /// <summary>
        /// LookAndFeelBase の登録を解除します。
        /// </summary>
        /// <param name="type">Control の型。</param>
        public void Deregister(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            LookAndFeelBase lookAndFeel = null;
            if (lookAndFeelMap.TryGetValue(type, out lookAndFeel))
            {
                lookAndFeelMap.Remove(type);
            }
        }

        public override ILookAndFeel GetLookAndFeel(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");

            var type = control.GetType();

            LookAndFeelBase lookAndFeel = null;
            while (type != typeof(object))
            {
                if (lookAndFeelMap.TryGetValue(type, out lookAndFeel)) break;

                type = type.BaseType;
            }

            return lookAndFeel;
        }
    }
}
