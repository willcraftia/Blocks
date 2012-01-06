#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs
{
    public abstract class ControlLafSourceBase : IControlLafSource
    {
        /// <summary>
        /// Game を取得します。
        /// </summary>
        public Game Game { get; private set; }

        /// <summary>
        /// GraphicsDevice。
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// ControlLafSource 専用の ContentManager を取得します。
        /// </summary>
        public ContentManager Content { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game">Game。</param>
        protected ControlLafSourceBase(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");
            Game = game;

            Content = new ContentManager(Game.Services);
            GraphicsDevice = game.GraphicsDevice;
        }

        // I/F
        public void Initialize()
        {
            LoadContent();
        }

        // I/F
        public abstract IControlLaf GetControlLaf(Control control);

        /// <summary>
        /// Initialize メソッドから呼び出されます。
        /// </summary>
        /// <remarks>
        /// このメソッドが呼び出される段階では、Content プロパティが利用可能な状態に構築されています。
        /// </remarks>
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
