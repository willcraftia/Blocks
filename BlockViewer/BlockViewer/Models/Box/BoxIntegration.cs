#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.Storage;
using Willcraftia.Net.Box;
using Willcraftia.Net.Box.Service;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Models.Box
{
    /// <summary>
    /// Box との接続を管理するクラスです。
    /// </summary>
    public sealed class BoxIntegration
    {
        public const string BlocksHomeFolderName = "Blocks Home";

        const string BlocksFolderName = "Blocks";

        const string IntegrationDirectoryName = "BoxIntegration";

        const string SettingsFileName = "BoxSettings.xml";

        XmlSerializer settingsSerializer = new XmlSerializer(typeof(BoxSettings));

        IBoxService boxService;

        BoxSession boxSession;

        IStorageService storageService;

        StorageDirectory integrationDirectory;

        string ticket;

        public bool BoxSessionExists
        {
            get { return boxSession != null; }
        }

        public BoxSettings BoxSettings { get; private set; }

        public bool BoxSettingsInitialized
        {
            get
            {
                return BoxSettings.AuthToken != null && 0 < BoxSettings.BlocksHomeFolderId && 0 < BoxSettings.BlocksFolderId;
            }
        }

        public BoxIntegration(Game game)
        {
            boxService = game.Services.GetRequiredService<IBoxService>();
            storageService = game.Services.GetRequiredService<IStorageService>();

            if (storageService.RootDirectory.DirectoryExists(IntegrationDirectoryName))
            {
                integrationDirectory = storageService.RootDirectory.GetDirectory(IntegrationDirectoryName);
            }
            else
            {
                integrationDirectory = storageService.RootDirectory.CreateDirectory(IntegrationDirectoryName);
            }

            if (integrationDirectory.FileExists(SettingsFileName))
            {
                using (var stream = integrationDirectory.OpenFile(SettingsFileName, FileMode.Open))
                {
                    BoxSettings = settingsSerializer.Deserialize(stream) as BoxSettings;
                    boxSession = boxService.CreateSession(BoxSettings.AuthToken);
                }
            }
            else
            {
                // 存在しなければファイルを作成します。
                BoxSettings = new BoxSettings();
                SaveSettings();
            }
        }

        public bool CheckSettings()
        {
            if (string.IsNullOrEmpty(BoxSettings.AuthToken)) return false;

            GetAuthToken();

            // todo
            return true;
        }

        public void GetTicket()
        {
            ticket = boxService.GetTicket();
        }

        public void LauchAuthorizationPageOnBrowser()
        {
            boxService.RedirectUserAuth(ticket);
        }

        public void GetAuthToken()
        {
            boxSession = boxService.GetAuthToken(ticket);
            BoxSettings.AuthToken = boxSession.AuthToken;
        }

        public void PrepareFolderTree()
        {
            // ルート フォルダの階層を 1 レベルで取得します。
            var rootFolder = boxSession.GetAccountTreeRoot("onelevel", "nozip");

            var blocksFolder = rootFolder.FindFolderByName(BlocksHomeFolderName);
            if (blocksFolder == null)
            {
                var createdFolder = boxSession.CreateFolder(0, BlocksHomeFolderName, false);
                BoxSettings.BlocksHomeFolderId = createdFolder.FolderId;
            }
            else
            {
                BoxSettings.BlocksHomeFolderId = blocksFolder.Id;
            }

            // "Blocks Data" フォルダの階層を 1 レベルで取得します。
            blocksFolder = boxSession.GetAccountTree(BoxSettings.BlocksHomeFolderId, "onelevel", "nozip");

            var meshesFolder = blocksFolder.FindFolderByName(BlocksFolderName);
            if (meshesFolder == null)
            {
                var createdFolder = boxSession.CreateFolder(BoxSettings.BlocksHomeFolderId, BlocksFolderName, false);
                BoxSettings.BlocksFolderId = createdFolder.FolderId;
            }
            else
            {
                BoxSettings.BlocksFolderId = meshesFolder.Id;
            }
        }

        public void SaveSettings()
        {
            using (var stream = integrationDirectory.CreateFile(SettingsFileName))
            {
                settingsSerializer.Serialize(stream, BoxSettings);
            }
        }

        public void Upload(IEnumerable<UploadFile> uploadFiles)
        {
            boxSession.Upload(BoxSettings.BlocksFolderId, uploadFiles, false, "Upload test.", null);
        }
    }
}
