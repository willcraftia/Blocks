using System;

namespace Willcraftia.Xna.Blocks.BlockEditor
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリー ポイントです。
        /// </summary>
        static void Main(string[] args)
        {
            using (BlockEditorGame game = new BlockEditorGame())
            {
                game.Run();
            }
        }
    }
#endif
}

