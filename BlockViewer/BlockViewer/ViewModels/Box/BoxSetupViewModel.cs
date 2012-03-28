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
        }

        public void GetTicketAsync(AsyncWebRequestCallback callback)
        {
            if (getTicketDelegate == null)
                getTicketDelegate = new AsyncDelegate(boxIntegration.GetTicket);
            getTicketDelegate.BeginInvoke(GetTicketAsyncCallback, callback);
        }

        public void LauchAuthorizationPageOnBrowser()
        {
            boxIntegration.LauchAuthorizationPageOnBrowser();
        }

        public void AccessAccountAsync(AsyncWebRequestCallback callback)
        {
            if (getAuthTokenDelegate == null)
                getAuthTokenDelegate = new AsyncDelegate(boxIntegration.GetAuthToken);
            getAuthTokenDelegate.BeginInvoke(GetAuthTokenAsyncCallback, callback);
        }

        public void PrepareFolderTreeAsync(AsyncWebRequestCallback callback)
        {
            if (prepareFolderTreeDelegate == null)
                prepareFolderTreeDelegate = new AsyncDelegate(boxIntegration.PrepareFolderTree);
            prepareFolderTreeDelegate.BeginInvoke(PrepareFolderTreeAsyncCallback, callback);
        }

        public void SaveSettingsAsync(AsyncWebRequestCallback callback)
        {
            if (saveSettingsDelegate == null)
                saveSettingsDelegate = new AsyncDelegate(boxIntegration.SaveSettings);
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

            var callback = asyncResult.AsyncState as AsyncWebRequestCallback;
            callback(succeeded, exception);
        }
    }
}
