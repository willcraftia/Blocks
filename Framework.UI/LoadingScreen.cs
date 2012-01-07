#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public class LoadingScreen : Screen, IScreenFactoryAware
    {
        public event EventHandler NextScreenLoadCompleted;

        delegate Screen LoadScreenAsyncCaller();

        LoadScreenAsyncCaller loadScreenAsyncCaller;

        public Screen NextScreen { get; private set; }

        // I/F
        public IScreenFactory ScreenFactory { get; set; }

        public string NextScreenName { get; set; }

        public LoadingScreen(Game game) : base(game) { }

        protected override void LoadContent()
        {
            LoadNextScreenAsync();

            base.LoadContent();
        }

        protected virtual Screen LoadScreen()
        {
            return ScreenFactory.CreateScreen(NextScreenName);
        }

        protected virtual void OnNextScreenLoadCompleted() { }

        void LoadNextScreenAsync()
        {
            loadScreenAsyncCaller = new LoadScreenAsyncCaller(LoadScreen);
            loadScreenAsyncCaller.BeginInvoke(LoadScreenAsyncCallerCallback, null);
        }

        void LoadScreenAsyncCallerCallback(IAsyncResult result)
        {
            NextScreen = loadScreenAsyncCaller.EndInvoke(result);
            RaiseNextScreenLoadCompleted();
        }

        void RaiseNextScreenLoadCompleted()
        {
            OnNextScreenLoadCompleted();
            if (NextScreenLoadCompleted != null) NextScreenLoadCompleted(this, EventArgs.Empty);
        }
    }
}
