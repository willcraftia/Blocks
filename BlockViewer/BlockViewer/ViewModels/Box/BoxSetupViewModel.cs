#region Using

using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.Storage;
using Willcraftia.Net.Box.Results;
using Willcraftia.Net.Box.Service;
using Willcraftia.Xna.Blocks.BlockViewer.Models.Box;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.ViewModels.Box
{
    public sealed class BoxSetupViewModel
    {
        public delegate void GetTicketDelegate();

        public delegate void GetAuthTokenDelegate();

        public delegate void PrepareFolderTreeDelegate();

        public delegate void SaveSettingsDelegate();

        public event EventHandler GotTicket = delegate { };

        public event EventHandler AccessSucceeded = delegate { };

        public event EventHandler PreparedFolders = delegate { };

        public event EventHandler SavedSettings = delegate { };

        BoxIntegration boxIntegration;

        GetTicketDelegate getTicketDelegate;

        GetAuthTokenDelegate getAuthTokenDelegate;

        PrepareFolderTreeDelegate prepareFolderTreeDelegate;

        SaveSettingsDelegate saveSettingsDelegate;

        readonly object syncRoot = new object();

        bool fireGotTicket;

        bool fireAccessSucceeded;

        bool firePreparedFolders;

        bool fireSavedSettings;

        public BoxSetupViewModel(BoxIntegration boxIntegration)
        {
            this.boxIntegration = boxIntegration;
        }

        public void Update()
        {
            lock (syncRoot)
            {
                if (fireGotTicket)
                {
                    GotTicket(this, EventArgs.Empty);
                    fireGotTicket = false;
                }
                if (fireAccessSucceeded)
                {
                    AccessSucceeded(this, EventArgs.Empty);
                    fireAccessSucceeded = false;
                }
                if (firePreparedFolders)
                {
                    PreparedFolders(this, EventArgs.Empty);
                    firePreparedFolders = false;
                }
                if (fireSavedSettings)
                {
                    SavedSettings(this, EventArgs.Empty);
                    fireSavedSettings = false;
                }
            }
        }

        public void GetTicketAsync()
        {
            if (getTicketDelegate == null)
                getTicketDelegate = new GetTicketDelegate(boxIntegration.GetTicket);
            getTicketDelegate.BeginInvoke(GetTicketAsyncCallback, null);
        }

        public void LauchAuthorizationPageOnBrowser()
        {
            boxIntegration.LauchAuthorizationPageOnBrowser();
        }

        public void AccessAccountAsync()
        {
            if (getAuthTokenDelegate == null)
                getAuthTokenDelegate = new GetAuthTokenDelegate(boxIntegration.GetAuthToken);
            getAuthTokenDelegate.BeginInvoke(GetAuthTokenAsyncCallback, null);
        }

        public void PrepareFolderTreeAsync()
        {
            if (prepareFolderTreeDelegate == null)
                prepareFolderTreeDelegate = new PrepareFolderTreeDelegate(boxIntegration.PrepareFolderTree);
            prepareFolderTreeDelegate.BeginInvoke(PrepareFolderTreeAsyncCallback, null);
        }

        public void SaveSettingsAsync()
        {
            if (saveSettingsDelegate == null)
                saveSettingsDelegate = new SaveSettingsDelegate(boxIntegration.SaveSettings);
            saveSettingsDelegate.BeginInvoke(SaveSettingsAsyncCallback, null);
        }

        void GetTicketAsyncCallback(IAsyncResult asyncResult)
        {
            lock (syncRoot)
            {
                getTicketDelegate.EndInvoke(asyncResult);
                fireGotTicket = true;
            }
        }

        void GetAuthTokenAsyncCallback(IAsyncResult asyncResult)
        {
            lock (syncRoot)
            {
                getAuthTokenDelegate.EndInvoke(asyncResult);
                fireAccessSucceeded = true;
            }
        }

        void PrepareFolderTreeAsyncCallback(IAsyncResult asyncResult)
        {
            lock (syncRoot)
            {
                prepareFolderTreeDelegate.EndInvoke(asyncResult);
                firePreparedFolders = true;
            }
        }

        void SaveSettingsAsyncCallback(IAsyncResult asyncResult)
        {
            lock (syncRoot)
            {
                saveSettingsDelegate.EndInvoke(asyncResult);
                fireSavedSettings = true;
            }
        }
    }
}
