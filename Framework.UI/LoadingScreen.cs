#region Using

using System;
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
        /// 非同期 Screen ローディングが完了した場合に発生します。
        /// </summary>
        public event EventHandler ScreenLoadCompleted;

        /// <summary>
        /// 非同期 Screen ローディングの delegate。
        /// </summary>
        /// <returns>ロードが完了した次 Screen。</returns>
        delegate Screen LoadScreenAsyncCaller();

        /// <summary>
        /// Screen の非同期呼び出し。
        /// </summary>
        LoadScreenAsyncCaller loadScreenAsyncCaller;

        // I/F
        public IScreenFactory ScreenFactory { get; set; }

        /// <summary>
        /// ロードされた Screen を取得します。
        /// </summary>
        public Screen LoadedScreen { get; private set; }

        /// <summary>
        /// ロードする Screan の名前を取得または設定します。
        /// </summary>
        public string LoadingScreenName { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game"></param>
        public LoadingScreen(Game game) : base(game) { }

        protected override void LoadContent()
        {
            // 非同期 Screen ローディングを開始します。
            LoadScreenAsync();

            base.LoadContent();
        }

        /// <summary>
        /// LoadingScreenName が示す Screen をロードします。
        /// </summary>
        /// <returns>ロードされた Screen。</returns>
        protected virtual Screen LoadScreen()
        {
            return ScreenFactory.CreateScreen(LoadingScreenName);
        }

        /// <summary>
        /// 非同期 Screen ローディングが完了した場合に呼び出されます。
        /// ScreenLoadCompleted イベントを発生させます。
        /// </summary>
        protected virtual void OnScreenLoadCompleted()
        {
            if (ScreenLoadCompleted != null) ScreenLoadCompleted(this, EventArgs.Empty);
        }

        /// <summary>
        /// 非同期 Screen ローディングを開始します。
        /// </summary>
        void LoadScreenAsync()
        {
            loadScreenAsyncCaller = new LoadScreenAsyncCaller(LoadScreen);
            loadScreenAsyncCaller.BeginInvoke(LoadScreenAsyncCallerCallback, null);
        }

        /// <summary>
        /// 非同期 Screen ローディングの完了を受け取るコールバック メソッドです。
        /// </summary>
        /// <param name="result"></param>
        void LoadScreenAsyncCallerCallback(IAsyncResult result)
        {
            LoadedScreen = loadScreenAsyncCaller.EndInvoke(result);
            OnScreenLoadCompleted();
        }
    }
}
