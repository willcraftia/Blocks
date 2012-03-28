#region Using

using System;
using Willcraftia.Xna.Blocks.BlockViewer.Models.Box;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.ViewModels.Box
{
    public sealed class BoxSetupViewModel
    {
        delegate void AsyncDelegate();

        BoxIntegration boxIntegration;

        AsyncDelegate getTicketDelegate;

        AsyncDelegate getAuthTokenDelegate;

        AsyncDelegate prepareFolderTreeDelegate;

        AsyncDelegate saveSettingsDelegate;

        public BoxSetupViewModel(BoxIntegration boxIntegration)
        {
            this.boxIntegration = boxIntegration;

            getTicketDelegate = new AsyncDelegate(boxIntegration.GetTicket);
            getAuthTokenDelegate = new AsyncDelegate(boxIntegration.GetAuthToken);
            prepareFolderTreeDelegate = new AsyncDelegate(boxIntegration.PrepareFolderTree);
            saveSettingsDelegate = new AsyncDelegate(boxIntegration.SaveSettings);
        }

        public void GetTicketAsync(AsyncWebCallback callback)
        {
            getTicketDelegate.BeginInvoke(GetTicketAsyncCallback, callback);
        }

        public void LauchAuthorizationPageOnBrowser()
        {
            boxIntegration.LauchAuthorizationPageOnBrowser();
        }

        public void AccessAccountAsync(AsyncWebCallback callback)
        {
            getAuthTokenDelegate.BeginInvoke(GetAuthTokenAsyncCallback, callback);
        }

        public void PrepareFolderTreeAsync(AsyncWebCallback callback)
        {
            prepareFolderTreeDelegate.BeginInvoke(PrepareFolderTreeAsyncCallback, callback);
        }

        public void SaveSettingsAsync(AsyncWebCallback callback)
        {
            saveSettingsDelegate.BeginInvoke(SaveSettingsAsyncCallback, callback);
        }

        void GetTicketAsyncCallback(IAsyncResult asyncResult)
        {
            HandleAsyncDelegate(getTicketDelegate, asyncResult);
        }

        void GetAuthTokenAsyncCallback(IAsyncResult asyncResult)
        {
            HandleAsyncDelegate(getAuthTokenDelegate, asyncResult);
        }

        void PrepareFolderTreeAsyncCallback(IAsyncResult asyncResult)
        {
            HandleAsyncDelegate(prepareFolderTreeDelegate, asyncResult);
        }

        void SaveSettingsAsyncCallback(IAsyncResult asyncResult)
        {
            HandleAsyncDelegate(saveSettingsDelegate, asyncResult);
        }

        void HandleAsyncDelegate(AsyncDelegate d, IAsyncResult asyncResult)
        {
            bool succeeded = true;
            Exception exception = null;
            try
            {
                d.EndInvoke(asyncResult);
            }
            catch (Exception e)
            {
                succeeded = false;
                exception = e;
            }

            var callback = asyncResult.AsyncState as AsyncWebCallback;
            callback(succeeded, exception);
        }
    }
}
