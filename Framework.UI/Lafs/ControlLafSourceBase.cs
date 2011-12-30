#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs
{
    public abstract class ControlLafSourceBase : IControlLafSource
    {
        /// <summary>
        /// IServiceProvider (ContentManager 生成に利用)。
        /// </summary>
        IServiceProvider serviceProvider;

        /// <summary>
        /// Content プロパティの構築で ContentManager に設定する RootDirectory プロパティ。
        /// </summary>
        string contentRootDirectory;

        /// <summary>
        /// ControlLafSource 専用の ContentManager を取得します。
        /// </summary>
        public ContentManager Content { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="serviceProvider">
        /// IServiceProvider (ContentManager 生成に利用)。
        /// </param>
        /// <param name="contentRootDirectory">
        /// Content プロパティの構築で ContentManager に設定する RootDirectory プロパティ。
        /// </param>
        protected ControlLafSourceBase(IServiceProvider serviceProvider, string contentRootDirectory)
        {
            if (serviceProvider == null) throw new ArgumentNullException("serviceProvider");
            this.serviceProvider = serviceProvider;
            this.contentRootDirectory = contentRootDirectory;
        }

        // I/F
        public virtual void Initialize()
        {
            // この LaF のための ContentManager を生成します。
            Content = new ContentManager(serviceProvider);
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
