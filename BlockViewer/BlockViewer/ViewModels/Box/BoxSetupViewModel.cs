#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.Threading;
using Willcraftia.Xna.Blocks.BlockViewer.Models.Box;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.ViewModels.Box
{
    public sealed class BoxSetupViewModel
    {
        IAsyncTaskService asyncTaskService;

        BoxIntegration boxIntegration;

        Action getTicketAction;

        Action getAuthTokenAction;

        Action prepareFolderTreeAction;

        Action saveSettingsAction;

        public BoxSetupViewModel(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");

            asyncTaskService = game.Services.GetRequiredService<IAsyncTaskService>();
            boxIntegration = (game as BlockViewerGame).BoxIntegration;

            getTicketAction = new Action(boxIntegration.GetTicket);
            getAuthTokenAction = new Action(boxIntegration.GetAuthToken);
            prepareFolderTreeAction = new Action(boxIntegration.PrepareFolderTree);
            saveSettingsAction = new Action(boxIntegration.SaveSettings);
        }

        public void GetTicketAsync(AsyncTaskCallback callback)
        {
            EnqueueAsyncTask(getTicketAction, callback);
        }

        public void LauchAuthorizationPageOnBrowser()
        {
            boxIntegration.LauchAuthorizationPageOnBrowser();
        }

        public void AccessAccountAsync(AsyncTaskCallback callback)
        {
            EnqueueAsyncTask(getAuthTokenAction, callback);
        }

        public void PrepareFolderTreeAsync(AsyncTaskCallback callback)
        {
            EnqueueAsyncTask(prepareFolderTreeAction, callback);
        }

        public void SaveSettingsAsync(AsyncTaskCallback callback)
        {
            EnqueueAsyncTask(saveSettingsAction, callback);
        }

        void EnqueueAsyncTask(Action action, AsyncTaskCallback callback)
        {
            var task = new AsyncTask
            {
                Action = action,
                Callback = callback
            };
            asyncTaskService.Enqueue(task);
        }
    }
}
