#region Using

using System;
using Willcraftia.Xna.Blocks.Graphics;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// IBlockMeshLoader のデフォルト実装クラスです。
    /// </summary>
    public sealed class DefaultBlockMeshLoader : IBlockMeshLoader
    {
        /// <summary>
        /// Block をロードするための IBlockLoader。
        /// </summary>
        IBlockLoader blockLoader;

        /// <summary>
        /// InterBlockMesh を生成するための InterBlockMeshFactory。
        /// </summary>
        InterBlockMeshFactory interBlockMeshFactory;

        /// <summary>
        /// 取得した Block から BlockMesh を生成するための BlockMeshFactory。
        /// </summary>
        BlockMeshFactory blockMeshFactory;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="blockLoader">
        /// Block をロードするための IBlockLoader。
        /// </param>
        /// <param name="interBlockMeshFactory">
        /// InterBlockMesh を生成するための InterBlockMeshFactory。
        /// </param>
        /// <param name="blockMeshFactory">
        /// 取得した Block から BlockMesh を生成するための BlockMeshFactory。
        /// </param>
        public DefaultBlockMeshLoader(
            IBlockLoader blockLoader,
            InterBlockMeshFactory interBlockMeshFactory,
            BlockMeshFactory blockMeshFactory)
        {
            if (blockLoader == null) throw new ArgumentNullException("blockLoader");
            if (interBlockMeshFactory == null) throw new ArgumentNullException("interBlockMeshFactory");
            if (blockMeshFactory == null) throw new ArgumentNullException("blockMeshFactory");
            this.blockLoader = blockLoader;
            this.interBlockMeshFactory = interBlockMeshFactory;
            this.blockMeshFactory = blockMeshFactory;
        }

        /// <summary>
        /// IBlockLoader.LoadBlock(string) の呼び出しで Block を取得し、
        /// それを InterBlockMeshFactory.Create(Block) へ渡して InterBlockMesh を生成し、
        /// それを BlockMeshFactory.Create(InterBlockMesh) へ渡して BlockMesh を生成します。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BlockMesh LoadBlockMesh(string name)
        {
            var block = blockLoader.LoadBlock(name);
            var interMesh = interBlockMeshFactory.Create(block);
            return blockMeshFactory.Create(interMesh);
        }
    }
}
