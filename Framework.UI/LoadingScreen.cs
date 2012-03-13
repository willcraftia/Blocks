#region Using

using System;
using System.Threading;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// ロード中画面を表示し、次に表示する Screen を非同期でロードするクラスです。
    /// </summary>
    public class LoadingScreen : Screen, IScreenFactoryAware
    {
        /// <summary>
        /// ロードされた Screen。
        /// </summary>
        Screen loadedScreen;

        // I/F
        public IScreenFactory ScreenFactory { get; set; }

        /// <summary>
        /// ロードされた Screen を取得します。
        /// まだロードされていない場合には null を返します。
        /// </summary>
        public Screen LoadedScreen
        {
            get
            {
                lock (this)
                {
                    return loadedScreen;
                }
            }
        }

        /// <summary>
        /// ロードする Screan の名前を取得または設定します。
        /// </summary>
        public string LoadedScreenName { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game"></param>
        public LoadingScreen(Game game) : base(game) { }

        /// <summary>
        /// Screen の非同期ロードを開始します。
        /// </summary>
        protected override void LoadContent()
        {
            //
            // MEMO
            //
            // Delegate の BeginInvoke(...) は、
            // .NET Compact Framework ではサポートされていません。
            // .NET Compact Framework for Xbox 360 はそのサブセットであるため、
            // 同様にサポートされていません。
            //

            // 非同期 Screen ローディングを開始します。
            ThreadPool.QueueUserWorkItem(LoadScreenWaitCallback);

            base.LoadContent();
        }

        /// <summary>
        /// LoadedScreenName プロパティが示す Screen をロードします。
        /// このメソッドは非同期に呼び出されます。
        /// </summary>
        /// <returns>ロードされた Screen。</returns>
        protected virtual Screen LoadScreen()
        {
            return ScreenFactory.CreateScreen(LoadedScreenName);
        }

        void LoadScreenWaitCallback(object state)
        {
            var screen = LoadScreen();

            lock (this)
            {
                loadedScreen = screen;
            }
        }
    }
}
