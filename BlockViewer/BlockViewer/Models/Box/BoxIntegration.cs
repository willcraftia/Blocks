#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.Storage;
using Willcraftia.Net.Box;
using Willcraftia.Net.Box.Results;
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

        BoxSettings boxSettings;

        public bool BoxSessionEnabled
        {
            get { return boxSession != null; }
        }

        public bool HasValidFolderTree
        {
            get { return 0 < boxSettings.HomeFolderId && 0 < boxSettings.BlocksFolderId; }
        }

        public BoxIntegration(Game game)
        {
            boxService = game.Services.GetRequiredService<IBoxService>();
            storageService = game.Services.GetRequiredService<IStorageService>();
        }

        public void Initialize()
        {
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
                try
                {
                    using (var stream = integrationDirectory.OpenFile(SettingsFileName, FileMode.Open))
                    {
                        boxSettings = settingsSerializer.Deserialize(stream) as BoxSettings;
                    }
                }
                catch
                {
                    // デシリアライズに失敗する場合は、保存されている設定を削除します。
                    integrationDirectory.DeleteFile(SettingsFileName);
                }
            }
            else
            {
                boxSettings = new BoxSettings();
            }
        }

        public bool RestoreSession()
        {
            if (string.IsNullOrEmpty(boxSettings.AuthToken)) return false;

            boxSession = boxService.CreateSession(boxSettings.AuthToken);

            Folder rootFolder;
            try
            {
                rootFolder = boxSession.GetAccountTreeRoot("onelevel", "nozip");
            }
            catch (BoxStatusException e)
            {
                boxSession = null;

                if (e.Status == "not_logged_in")
                {
                    // AuthToken が無効になっています。
                    boxSettings.AuthToken = null;
                    boxSettings.HomeFolderId = -1;
                    boxSettings.BlocksFolderId = -1;
                    SaveSettings();
                    return false;
                }
                else
                {
                    // その他のエラーならば throw します。
                    throw;
                }
            }

            // 保存されている Blocks Home と Blocks フォルダの ID を検査します。
            // それらが無効であっても、AuthToken は有効であるため、
            // BoxSession の復元は成功で終わらせます。
            if (0 < boxSettings.HomeFolderId)
            {
                // Blocks Home と Blocks フォルダは同時に作成するので、
                // どちらかの ID が無効な場合、同時に無効に設定します。

                var homeFolder = rootFolder.FindFolderById(boxSettings.HomeFolderId);
                if (homeFolder == null)
                {
                    // Blocks Home フォルダが存在しません。
                    boxSettings.HomeFolderId = -1;
                    boxSettings.BlocksFolderId = -1;
                    SaveSettings();
                }
                else
                {
                    var homeFolderTree = boxSession.GetAccountTree(homeFolder.Id, "onelevel", "nozip");
                    var blockFolder = homeFolderTree.FindFolderById(boxSettings.BlocksFolderId);
                    if (blockFolder == null)
                    {
                        // Blocks フォルダが存在しません。
                        boxSettings.HomeFolderId = -1;
                        boxSettings.BlocksFolderId = -1;
                        SaveSettings();
                    }
                }
            }
            else
            {
                if (0 < boxSettings.BlocksFolderId)
                {
                    // フォルダ情報の不整合を起こしているため、初期化して保存します。
                    boxSettings.HomeFolderId = -1;
                    boxSettings.BlocksFolderId = -1;
                    SaveSettings();
                }
            }

            return true;
        }

        public void GetTicket()
        {
            // Ticket 取得済みならばスキップします。
            if (ticket != null) return;

            ticket = boxService.GetTicket();
        }

        public void LauchAuthorizationPageOnBrowser()
        {
            boxService.RedirectUserAuth(ticket);
        }

        public void GetAuthToken()
        {
            boxSession = boxService.GetAuthToken(ticket);
            boxSettings.AuthToken = boxSession.AuthToken;
        }

        public void PrepareFolderTree()
        {
            // ルート フォルダの階層を 1 レベルで取得します。
            var rootFolder = boxSession.GetAccountTreeRoot("onelevel", "nozip");

            var blocksFolder = rootFolder.FindFolderByName(BlocksHomeFolderName);
            if (blocksFolder == null)
            {
                var createdFolder = boxSession.CreateFolder(0, BlocksHomeFolderName, false);
                boxSettings.HomeFolderId = createdFolder.FolderId;
            }
            else
            {
                boxSettings.HomeFolderId = blocksFolder.Id;
            }

            // "Blocks Data" フォルダの階層を 1 レベルで取得します。
            blocksFolder = boxSession.GetAccountTree(boxSettings.HomeFolderId, "onelevel", "nozip");

            var meshesFolder = blocksFolder.FindFolderByName(BlocksFolderName);
            if (meshesFolder == null)
            {
                var createdFolder = boxSession.CreateFolder(boxSettings.HomeFolderId, BlocksFolderName, false);
                boxSettings.BlocksFolderId = createdFolder.FolderId;
            }
            else
            {
                boxSettings.BlocksFolderId = meshesFolder.Id;
            }
        }

        public void SaveSettings()
        {
            using (var stream = integrationDirectory.CreateFile(SettingsFileName))
            {
                settingsSerializer.Serialize(stream, boxSettings);
            }
        }

        public void Upload(IEnumerable<UploadFile> uploadFiles)
        {
            boxSession.Upload(boxSettings.BlocksFolderId, uploadFiles, false, "Upload test.", null);
        }
    }
}
