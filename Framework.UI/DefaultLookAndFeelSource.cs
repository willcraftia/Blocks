#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public class DefaultLookAndFeelSource : ILookAndFeelSource
    {
        /// <summary>
        /// Game を取得します。
        /// </summary>
        public Game Game { get; private set; }

        /// <summary>
        /// Control の型をキーに ILookAndFeel を値とするマップを取得します。
        /// </summary>
        public Dictionary<Type, ILookAndFeel> LookAndFeelMap { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game">Game。</param>
        public DefaultLookAndFeelSource(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");
            Game = game;
            LookAndFeelMap = new Dictionary<Type, ILookAndFeel>();
        }

        // I/F
        public virtual ILookAndFeel GetLookAndFeel(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");

            var type = control.GetType();

            ILookAndFeel lookAndFeel = null;
            while (type != typeof(object))
            {
                if (LookAndFeelMap.TryGetValue(type, out lookAndFeel)) break;

                type = type.BaseType;
            }

            return lookAndFeel;
        }
    }
}
