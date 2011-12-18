#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

#endregion

namespace Willcraftia.Xna.Framework.UI.Visuals
{
    public abstract class ControlLafSourceBase : IControlLafSource
    {
        /// <summary>
        /// Content プロパティの構築で ContentManager に設定する RootDirectory プロパティ。
        /// </summary>
        string contentRootDirectory;

        // I/F
        public IUIContext UIContext { get; set; }

        /// <summary>
        /// ControlLafSource 専用の ContentManager を取得します。
        /// </summary>
        public ContentManager Content { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="contentRootDirectory">
        /// Content プロパティの構築で ContentManager に設定する RootDirectory プロパティ。
        /// </param>
        protected ControlLafSourceBase(string contentRootDirectory)
        {
            this.contentRootDirectory = contentRootDirectory;
        }

        // I/F
        public virtual void Initialize()
        {
            // この LaF のための ContentManager を生成します。
            Content = UIContext.CreateContentManager();
            if (!string.IsNullOrEmpty(contentRootDirectory)) Content.RootDirectory = contentRootDirectory;

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
