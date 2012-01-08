#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs
{
    /// <summary>
    /// IControlLafSource 実装の基礎実装を提供するクラスです。
    /// </summary>
    public abstract class ControlLafSourceBase : IControlLafSource
    {
        // I/F
        public bool Initialized { get; private set; }

        /// <summary>
        /// Game を取得します。
        /// </summary>
        public Game Game { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game">Game。</param>
        protected ControlLafSourceBase(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");
            Game = game;
        }

        // I/F
        public void Initialize()
        {
            LoadContent();

            Initialized = true;
        }

        // I/F
        public abstract IControlLaf GetControlLaf(Control control);

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

        ~ControlLafSourceBase()
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
