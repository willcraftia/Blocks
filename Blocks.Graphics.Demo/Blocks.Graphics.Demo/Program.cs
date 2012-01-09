using System;

namespace Willcraftia.Xna.Blocks.Graphics.Demo
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリー ポイントです。
        /// </summary>
        static void Main(string[] args)
        {
            using (BlockModelViewGame game = new BlockModelViewGame())
            {
                game.Run();
            }
        }
    }
#endif
}

