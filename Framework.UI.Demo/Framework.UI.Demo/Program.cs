using System;

namespace Willcraftia.Xna.Framework.UI.Demo
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリー ポイントです。
        /// </summary>
        static void Main(string[] args)
        {
            using (UIDemoGame game = new UIDemoGame())
            {
                game.Run();
            }
        }
    }
#endif
}

