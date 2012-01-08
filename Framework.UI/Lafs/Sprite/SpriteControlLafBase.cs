#region Using

using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    /// <summary>
    /// スプライト イメージで描画する LaF の基礎クラスです。
    /// </summary>
    public abstract class SpriteControlLafBase : IControlLaf, IDisposable
    {
        /// <summary>
        /// SpriteControlLafSource を取得します。
        /// </summary>
        protected internal SpriteControlLafSource Source { get; internal set; }
        
        // I/F
        public abstract void Draw(Control control, IDrawContext drawContext);

        /// <summary>
        /// 初期化します。
        /// </summary>
        public virtual void Initialize()
        {
            LoadContent();
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

        ~SpriteControlLafBase()
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
