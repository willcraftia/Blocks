#region Using

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using Willcraftia.Net.Box.Results;
using Willcraftia.Net.Box.Service;

#endregion

namespace Willcraftia.Net.Box.Demo
{
    class Program
    {
        static BoxManager boxManager = new BoxManager(ApiKey.Value);

        static void Main(string[] args)
        {
            //----------------------------------------------------------------
            //
            // get_ticket
            //
            Prompt("Press any key to request get_ticket.");

            var getTicketSucceeded = boxManager.GetTicket();
            Console.WriteLine("Result:");
            Console.WriteLine(boxManager.GetTicketResult);

            if (!getTicketSucceeded)
            {
                Prompt("failed.");
                return;
            }

            //----------------------------------------------------------------
            //
            // redirect
            //
            Prompt("Press any key to redirect you to Box Application auth page.");

            boxManager.RedirectUserAuth();

            //----------------------------------------------------------------
            //
            // get_auth_token
            //
            Prompt("Press any key to request get_auth_token.");

            var getAuthTokenSucceeded = boxManager.GetAuthToken();
            Console.WriteLine("Result:");
            Console.WriteLine(boxManager.GetAuthTokenResult);

            if (!getAuthTokenSucceeded)
            {
                Prompt("failed.");
                return;
            }

            var session = boxManager.CreateSession();

            //----------------------------------------------------------------
            //
            // get_account_tree[root]
            //
            Prompt("Press any key to request get_account_tree[root].");

            var getAccountTreeRootResult = session.GetAccountTreeRoot("onelevel", "nozip");
            Console.WriteLine("Result:");
            Console.WriteLine(getAccountTreeRootResult);

            if (!getAccountTreeRootResult.Succeeded)
            {
                Prompt("failed.");
                Logout(session);
                return;
            }

            string demoFolderName = "DemoFolder";
            long demoFolderId = -1;
            var demoFolder = getAccountTreeRootResult.Tree.Folder.FindFolderByName(demoFolderName);
            if (demoFolder != null) demoFolderId = demoFolder.Id;

            if (demoFolderId < 0)
            {
                //----------------------------------------------------------------
                //
                // create_folder
                //
                Prompt("Press any key to request create_folder.");

                var createFolderResult = session.CreateFolder(0, demoFolderName, true);
                Console.WriteLine("Result:");
                Console.WriteLine(createFolderResult);

                if (!createFolderResult.Succeeded)
                {
                    Prompt("failed.");
                    Logout(session);
                    return;
                }

                demoFolderId = createFolderResult.Folder.FolderId;
            }
            else
            {
                Prompt("Press any key to skip create_folder.");
            }

            //----------------------------------------------------------------
            //
            // upload
            //
            Prompt("Press any key to upload files.");

            var uploadFiles = new UploadFile[]
            {
                new UploadFile
                {
                        ContentType = "text/xml;charset=utf-8",
                        Name = "Demo_0.xml",
                        Content = @"<?xml version=""1.0""?><Demo>Demo File 0</Demo>"
                },
                new UploadFile
                {
                        ContentType = "text/xml;charset=utf-8",
                        Name = "Demo_1.xml",
                        Content = @"<?xml version=""1.0""?><Demo>Demo File 1</Demo>"
                }
            };

            var uploadResult = session.Upload(demoFolderId, uploadFiles, false, "Demo message.", null);
            Console.WriteLine("Result:");
            Console.WriteLine(uploadResult);

            if (!uploadResult.Succeeded)
            {
                Prompt("failed.");
                Logout(session);
                return;
            }

            //----------------------------------------------------------------
            //
            // overwrite
            //
            Prompt("Press any key to overwrite a file.");

            var overwriteFile = new UploadFile
            {
                ContentType = "text/xml;charset=utf-8",
                Name = "Demo_0.xml",
                Content = @"<?xml version=""1.0""?><Demo>Demo File 0 Overwritten</Demo>"
            };
            var overwriteResult = session.Overwrite(uploadResult.Files[0].Id, overwriteFile, false, "Demo message.", null);
            Console.WriteLine("Result:");
            Console.WriteLine(overwriteResult);

            if (!overwriteResult.Succeeded)
            {
                Prompt("failed.");
                Logout(session);
                return;
            }

            //----------------------------------------------------------------
            //
            // download
            //
            Prompt("Press any key to download a file.");

            string downloadedFile;
            using (var downloadStream = session.Download(uploadResult.Files[0].Id))
            {
                using (var memoryStream = new MemoryStream())
                {
                    var bytes = new byte[1024];
                    int byteCount = 0;

                    while ((byteCount = downloadStream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        memoryStream.Write(bytes, 0, byteCount);
                    }
                    
                    memoryStream.Position = 0;
                    var reader = new StreamReader(memoryStream);
                    downloadedFile = reader.ReadToEnd();
                }
            }

            Console.WriteLine("Result:");
            Console.WriteLine(downloadedFile);
            
            //----------------------------------------------------------------
            //
            // invite_collaborators
            //
            Prompt("Press any key to request invite_collaborators.");

            string[] emails = { "blockcraftia@gmail.com" };
            var inviteCollaboratorsResult = session.InviteCollaboratorsToFolder(
                demoFolderId, null, emails, Role.Viewer, false, true);
            Console.WriteLine("Result:");
            Console.WriteLine(inviteCollaboratorsResult);

            if (!inviteCollaboratorsResult.Succeeded &&
                inviteCollaboratorsResult.Status != InviteCollaboratorsResultStatus.UserAlreadyCollaborator)
            {
                Prompt("failed.");
                Logout(session);
                return;
            }

            //----------------------------------------------------------------
            //
            // get_user_id
            //
            // ※get_user_id は collaborator として既知でなければ user_id の取得を許可していない模様。
            //
            Prompt("Press any key to request get_user_id.");

            var getUserIdResult = session.GetUserId("blockcraftia@gmail.com");
            Console.WriteLine("Result:");
            Console.WriteLine(getUserIdResult);

            if (!getUserIdResult.Succeeded)
            {
                Prompt("failed.");
                Logout(session);
                return;
            }

            //----------------------------------------------------------------
            //
            // get_file_info
            //
            Prompt("Press any key to request get_file_info.");

            var getFileInfoResult = session.GetFileInfo(uploadResult.Files[0].Id);
            Console.WriteLine("Result:");
            Console.WriteLine(getFileInfoResult);

            if (!getFileInfoResult.Succeeded)
            {
                Prompt("failed.");
                Logout(session);
                return;
            }

            //----------------------------------------------------------------
            //
            // delete[file]
            //
            Prompt("Press any key to request delete[file].");

            var deleteFileResult = session.DeleteFile(uploadResult.Files[0].Id);
            Console.WriteLine("Result:");
            Console.WriteLine(deleteFileResult);

            if (!deleteFileResult.Succeeded)
            {
                Prompt("failed.");
                Logout(session);
                return;
            }

            //----------------------------------------------------------------
            //
            // delete[folder]
            //
            Prompt("Press any key to request delete[folder].");

            var deleteFolderResult = session.DeleteFolder(demoFolderId);
            Console.WriteLine("Result:");
            Console.WriteLine(deleteFolderResult);

            if (!deleteFolderResult.Succeeded)
            {
                Prompt("failed.");
                Logout(session);
                return;
            }

            //----------------------------------------------------------------
            //
            // logout
            //
            Logout(session);
        }

        static void Logout(BoxSession session)
        {
            Console.WriteLine("Logout.");
            
            var result = session.Logout();
            Console.WriteLine("Result:");
            Console.WriteLine(result);

            Prompt("Press any key to exit.");
        }

        static void Prompt(string message)
        {
            Console.WriteLine();
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}
