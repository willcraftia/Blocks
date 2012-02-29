#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public class DefaultLookAndFeelSource : ILookAndFeelSource
    {
        // I/F
        public bool Initialized { get; private set; }

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
        public void Initialize()
        {
            LoadContent();

            Initialized = true;
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

        /// <summary>
        /// Initialize メソッドから呼び出されます。
        /// </summary>
        protected virtual void LoadContent() { }

        /// <summary>
        /// Dispose メソッドから呼び出されます。
        /// </summary>
        protected virtual void UnloadContent() { }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        bool disposed;

        ~DefaultLookAndFeelSource()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing) UnloadContent();

            disposed = true;
        }

        #endregion
    }
}
