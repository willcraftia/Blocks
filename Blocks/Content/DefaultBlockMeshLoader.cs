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
        /// 取得した Block から BlockMesh を生成するための BlockMeshFactory。
        /// </summary>
        BlockMeshFactory blockMeshFactory;

        /// <summary>
        /// Block をロードするための IBlockLoader。
        /// </summary>
        IBlockLoader blockLoader;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="blockLoader">
        /// Block をロードするための IBlockLoader。
        /// </param>
        /// <param name="blockMeshFactory">
        /// 取得した Block から BlockMesh を生成するための BlockMeshFactory。
        /// </param>
        public DefaultBlockMeshLoader(IBlockLoader blockLoader, BlockMeshFactory blockMeshFactory)
        {
            if (blockLoader == null) throw new ArgumentNullException("blockLoader");
            if (blockMeshFactory == null) throw new ArgumentNullException("blockMeshFactory");
            this.blockLoader = blockLoader;
            this.blockMeshFactory = blockMeshFactory;
        }

        /// <summary>
        /// IBlockLoader.LoadBlock(string) の呼び出しで Block を取得し、
        /// それを BlockMeshFactory.CreateBlockMesh(Block) へ渡して BlockMesh を生成します。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BlockMesh LoadBlockMesh(string name)
        {
            var block = blockLoader.LoadBlock(name);
            return blockMeshFactory.CreateBlockMesh(block);
        }
    }
}
