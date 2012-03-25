#region Using

using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.Storage;
using Willcraftia.Net.Box.Service;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.ViewModels.Box
{
    public sealed class BoxSetupViewModel
    {
        public delegate string GetTicketDelegate();

        public delegate BoxSession GetAuthTokenDelegate(string ticket);

        public event EventHandler GotTicket = delegate { };

        public event EventHandler AccessSucceeded = delegate { };

        IBoxService boxService;

        BoxSession boxSession;

        IStorageService storageService;

        string ticket;

        GetTicketDelegate getTicketDelegate;

        GetAuthTokenDelegate getAuthTokenDelegate;

        readonly object syncRoot = new object();

        bool fireGotTicket;

        bool fireAccessSucceeded;

        public BoxSetupViewModel(Game game)
        {
            boxService = game.Services.GetRequiredService<IBoxService>();
            storageService = game.Services.GetRequiredService<IStorageService>();
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
                getTicketDelegate = new GetTicketDelegate(boxService.GetTicket);
            getTicketDelegate.BeginInvoke(GetTicketAsyncCallback, null);
        }

        public void LauchAuthorizationPageOnBrowser()
        {
            boxService.RedirectUserAuth(ticket);
        }

        public void AccessAccountAsync()
        {
            if (getAuthTokenDelegate == null)
                getAuthTokenDelegate = new GetAuthTokenDelegate(boxService.GetAuthToken);
            getAuthTokenDelegate.BeginInvoke(ticket, GetAuthTokenAsyncCallback, null);
        }

        XmlSerializer settingsSerializer = new XmlSerializer(typeof(BoxSettings));

        public delegate void SaveSettingsDelegate();

        SaveSettingsDelegate saveSettingsDelegate;

        bool fireSavedSettings;

        public event EventHandler SavedSettings = delegate { };

        public void SaveSettingsAsync()
        {
            if (saveSettingsDelegate == null)
                saveSettingsDelegate = new SaveSettingsDelegate(SaveSettings);
            saveSettingsDelegate.BeginInvoke(SaveSettingsAsyncCallback, null);
        }

        void SaveSettings()
        {
            StorageDirectory settingsDir;
            if (!storageService.RootDirectory.DirectoryExists("BoxSettings"))
            {
                settingsDir = storageService.RootDirectory.CreateDirectory("BoxSettings");
            }
            else
            {
                settingsDir = storageService.RootDirectory.GetDirectory("BoxSettings");
            }

            var settings = new BoxSettings
            {
                AuthToken = boxSession.AuthToken
            };

            using (var stream = settingsDir.CreateFile("BoxSettings.xml"))
            {
                settingsSerializer.Serialize(stream, settings);
            }
        }

        void GetTicketAsyncCallback(IAsyncResult asyncResult)
        {
            lock (syncRoot)
            {
                ticket = getTicketDelegate.EndInvoke(asyncResult);
                fireGotTicket = true;
            }
        }

        void GetAuthTokenAsyncCallback(IAsyncResult asyncResult)
        {
            lock (syncRoot)
            {
                boxSession = getAuthTokenDelegate.EndInvoke(asyncResult);
                fireAccessSucceeded = true;
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
