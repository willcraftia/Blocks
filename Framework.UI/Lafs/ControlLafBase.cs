#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs
{
    public abstract class ControlLafBase : IControlLaf
    {
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        protected ControlLafBase() { }

        // I/F
        public virtual void Initialize()
        {
            LoadContent();
        }

        // I/F
        public abstract void Draw(Control control);

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

        ~ControlLafBase()
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
