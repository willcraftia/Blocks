#region Using

using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    /// <summary>
    /// LOD ごとの BlockMeshPart を管理するクラスです。
    /// </summary>
    public sealed class BlockMeshLod : IDisposable
    {
        /// <summary>
        /// ロードが完了した時に発生します。
        /// </summary>
        public event EventHandler Loaded = delegate { };

        /// <summary>
        /// BlockMeshPart のリスト。
        /// </summary>
        ReadOnlyCollection<BlockMeshPart> meshParts;

        /// <summary>
        /// ロードが完了しているかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (ロードが完了している場合)、false (それ以外の場合)。
        /// </value>
        public bool IsLoaded { get; private set; }

        /// <summary>
        /// このインスタンスが担う LOD を取得します。
        /// </summary>
        public int LevelOfDetail { get; private set; }

        /// <summary>
        /// BlockMeshPart のリストを取得します。
        /// </summary>
        public ReadOnlyCollection<BlockMeshPart> MeshParts
        {
            get { return meshParts; }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="levelOfDetail">このインスタンスが担う LOD。</param>
        internal BlockMeshLod(int levelOfDetail)
        {
            LevelOfDetail = levelOfDetail;
        }

        public void Draw()
        {
            foreach (var meshPart in MeshParts) meshPart.Draw();
        }

        internal void AllocateMeshParts(GraphicsDevice graphicsDevice, int count)
        {
            var array = new BlockMeshPart[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = new BlockMeshPart(graphicsDevice);
                array[i].Loaded += OnMeshPartLoaded;
            }
            
            meshParts = new ReadOnlyCollection<BlockMeshPart>(array);
        }

        void OnMeshPartLoaded(object sender, EventArgs e)
        {
            foreach (var meshPart in meshParts)
            {
                if (!meshPart.IsLoaded) return;
            }

            IsLoaded = true;
            Loaded(this, EventArgs.Empty);
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool disposed;

        ~BlockMeshLod()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                foreach (var meshPart in meshParts) meshPart.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
