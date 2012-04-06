#region Using

using System;
using System.Threading;
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

        WaitCallback getTicketAsync;

        WaitCallback getAuthTokenAsync;

        WaitCallback prepareFolderTreeAsync;

        WaitCallback saveSettingsAsync;

        public BoxSetupViewModel(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");

            asyncTaskService = game.Services.GetRequiredService<IAsyncTaskService>();
            boxIntegration = (game as BlockViewerGame).BoxIntegration;

            getTicketAsync = (state) => boxIntegration.GetTicket();
            getAuthTokenAsync = (state) => boxIntegration.GetAuthToken();
            prepareFolderTreeAsync = (state) => boxIntegration.PrepareFolderTree();
            saveSettingsAsync = (state) => boxIntegration.SaveSettings();
        }

        public void GetTicketAsync(AsyncTaskResultCallback callback)
        {
            asyncTaskService.Enqueue(getTicketAsync, null, callback);
        }

        public void LauchAuthorizationPageOnBrowser()
        {
            boxIntegration.LauchAuthorizationPageOnBrowser();
        }

        public void AccessAccountAsync(AsyncTaskResultCallback callback)
        {
            asyncTaskService.Enqueue(getAuthTokenAsync, null, callback);
        }

        public void PrepareFolderTreeAsync(AsyncTaskResultCallback callback)
        {
            asyncTaskService.Enqueue(prepareFolderTreeAsync, null, callback);
        }

        public void SaveSettingsAsync(AsyncTaskResultCallback callback)
        {
            asyncTaskService.Enqueue(saveSettingsAsync, null, callback);
        }
    }
}
