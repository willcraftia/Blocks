using System;

namespace Willcraftia.Xna.Framework.Audio.StreamingWaveDemo
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリー ポイントです。
        /// </summary>
        static void Main(string[] args)
        {
            using (StreamingWaveDemo game = new StreamingWaveDemo())
            {
                game.Run();
            }
        }
    }
#endif
}

