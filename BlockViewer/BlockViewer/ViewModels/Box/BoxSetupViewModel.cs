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
        public delegate string GetTicketDelegate();

        public delegate BoxSession GetAuthTokenDelegate(string ticket);

        public delegate void PrepareFolderTreeDelegate();

        public const string BlocksFolderName = "Blocks Data";

        public const string BlockMehsesFolderName = "BlockMehses";

        public event EventHandler GotTicket = delegate { };

        public event EventHandler AccessSucceeded = delegate { };

        public event EventHandler PreparedFolders = delegate { };

        IBoxService boxService;

        BoxSession boxSession;

        Folder rootFolder;

        IStorageService storageService;

        string ticket;

        BoxSettings boxSettings = new BoxSettings();

        GetTicketDelegate getTicketDelegate;

        GetAuthTokenDelegate getAuthTokenDelegate;

        PrepareFolderTreeDelegate prepareFolderTreeDelegate;

        readonly object syncRoot = new object();

        bool fireGotTicket;

        bool fireAccessSucceeded;

        bool firePreparedFolders;

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

        public void PrepareFolderTreeAsync()
        {
            if (prepareFolderTreeDelegate == null)
                prepareFolderTreeDelegate = new PrepareFolderTreeDelegate(PrepareFolderTree);
            prepareFolderTreeDelegate.BeginInvoke(PrepareFolderTreeAsyncCallback, null);
        }

        void PrepareFolderTree()
        {
            // ルート フォルダの階層を 1 レベルで取得します。
            var rootFolder = boxSession.GetAccountTreeRoot("onelevel", "nozip");

            var blocksFolder = rootFolder.FindFolderByName(BlocksFolderName);
            if (blocksFolder == null)
            {
                var createdFolder = boxSession.CreateFolder(0, BlocksFolderName, false);
                boxSettings.BlocksFolderId = createdFolder.FolderId;
            }
            else
            {
                boxSettings.BlocksFolderId = blocksFolder.Id;
            }

            // "Blocks Data" フォルダの階層を 1 レベルで取得します。
            blocksFolder = boxSession.GetAccountTree(boxSettings.BlocksFolderId, "onelevel", "nozip");

            var meshesFolder = blocksFolder.FindFolderByName(BlockMehsesFolderName);
            if (meshesFolder == null)
            {
                var createdFolder = boxSession.CreateFolder(boxSettings.BlocksFolderId, BlockMehsesFolderName, false);
                boxSettings.MeshesFolderId = createdFolder.FolderId;
            }
            else
            {
                boxSettings.MeshesFolderId = meshesFolder.Id;
            }
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

            using (var stream = settingsDir.CreateFile("BoxSettings.xml"))
            {
                settingsSerializer.Serialize(stream, boxSettings);
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
                boxSettings.AuthToken = boxSession.AuthToken;
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
