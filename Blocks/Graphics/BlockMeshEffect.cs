#region Using

using System;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    /// <summary>
    /// BlockMesh で用いられる IBlockEffect を管理するクラスです。
    /// </summary>
    public sealed class BlockMeshEffect : IDisposable
    {
        /// <summary>
        /// Subject のロードが完了した時に発生します。
        /// </summary>
        public event EventHandler Loaded = delegate { };

        /// <summary>
        /// IBlockEffect を取得します。
        /// IsLoaded プロパティが false を返す場合、
        /// このプロパティは null です。
        /// </summary>
        public IBlockEffect Effect { get; private set; }

        /// <summary>
        /// IBlockEffect のロードが完了しているかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (IBlockEffect のロードが完了している場合)、false (それ以外の場合)。
        /// </value>
        public bool IsLoaded { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        internal BlockMeshEffect() { }

        /// <summary>
        /// IBlockEffect を設定します。
        /// IsLoaded プロパティを true に設定し、Loaded イベントを発生させます。
        /// </summary>
        /// <param name="effect"></param>
        internal void PopulateEffect(IBlockEffect effect)
        {
            Effect = effect;

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

        ~BlockMeshEffect()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                if (Effect != null) Effect.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
