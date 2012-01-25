using System;

namespace Willcraftia.Xna.Blocks.BlockViewer
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリー ポイントです。
        /// </summary>
        static void Main(string[] args)
        {
            using (BlockViewerGame game = new BlockViewerGame())
            {
                game.Run();
            }
        }
    }
#endif
}

