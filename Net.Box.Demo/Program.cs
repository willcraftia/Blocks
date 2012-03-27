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
        static void Main(string[] args)
        {
            var assemblyFile = "Willcraftia.Net.Box.Demo.ApiKey.dll";
            var apiKeyClassName = "Willcraftia.Net.Box.Demo.ApiKey";
            var boxManager = new BoxManager(assemblyFile, apiKeyClassName);

            //----------------------------------------------------------------
            //
            // get_ticket
            //
            Prompt("Press any key to request get_ticket.");

            string ticket;
            try
            {
                ticket = boxManager.GetTicket();
                Console.WriteLine("Ticket: " + ticket);
            }
            catch (Exception e)
            {
                Prompt(e);
                return;
            }

            //----------------------------------------------------------------
            //
            // redirect
            //
            Prompt("Press any key to redirect you to Box Application auth page.");

            boxManager.RedirectUserAuth(ticket);

            //----------------------------------------------------------------
            //
            // get_auth_token
            //
            Prompt("Press any key to request get_auth_token.");

            BoxSession session;
            try
            {
                session = boxManager.GetAuthToken(ticket);
                Console.WriteLine("AuthToken: " + session.AuthToken);
            }
            catch (Exception e)
            {
                Prompt(e);
                return;
            }

            //----------------------------------------------------------------
            //
            // get_account_tree[root]
            //
            Prompt("Press any key to request get_account_tree[root].");

            Folder rootFolder;
            try
            {
                rootFolder = session.GetAccountTreeRoot("onelevel", "nozip");
                Console.WriteLine("Root Folder: " + rootFolder);
            }
            catch (Exception e)
            {
                Prompt(e);
                return;
            }

            string demoFolderName = "DemoFolder";
            long demoFolderId = -1;
            var demoFolder = rootFolder.FindFolderByName(demoFolderName);
            if (demoFolder != null) demoFolderId = demoFolder.Id;

            //----------------------------------------------------------------
            //
            // create_folder
            //
            if (demoFolderId < 0)
            {
                Prompt("Press any key to request create_folder.");

                CreatedFolder createdFolder;
                try
                {
                    createdFolder = session.CreateFolder(0, demoFolderName, true);
                }
                catch (Exception e)
                {
                    Prompt(e);
                    return;
                }

                demoFolderId = createdFolder.FolderId;
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

            List<UploadedFile> uploadedFiles;
            try
            {
                uploadedFiles = session.Upload(demoFolderId, uploadFiles, false, "Demo message.", null);
                Console.WriteLine("Uploaded Files:");
                foreach (var uploadedFile in uploadedFiles) Console.WriteLine(uploadedFile);
            }
            catch (Exception e)
            {
                Prompt(e);
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
            try
            {
                var overwrittenFile = session.Overwrite(uploadedFiles[0].Id, overwriteFile, false, "Demo message.", null);
                Console.WriteLine("Overwritten File:");
                Console.WriteLine(overwrittenFile);
            }
            catch (Exception e)
            {
                Prompt(e);
                return;
            }

            //----------------------------------------------------------------
            //
            // download
            //
            Prompt("Press any key to download a file.");

            string downloadedFile;
            using (var downloadStream = session.Download(uploadedFiles[0].Id))
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
            List<InvitedCollaborator> invitedCollaborators;
            try
            {
                invitedCollaborators = session.InviteCollaboratorsToFolder(demoFolderId, null, emails, Role.Viewer, false, true);
                Console.WriteLine("Invited Collaborators:");
                foreach (var invited in invitedCollaborators) Console.WriteLine(invited);
            }
            catch (BoxStatusException e)
            {
                if (e.Status == "user_already_collaborator")
                {
                    Prompt("The specified users were already collaborators.");
                }
                else
                {
                    Prompt(e);
                    return;
                }
            }

            //----------------------------------------------------------------
            //
            // get_user_id
            //
            // ※get_user_id は collaborator として既知でなければ user_id の取得を許可していない模様。
            //
            Prompt("Press any key to request get_user_id.");

            var userEmail = "blockcraftia@gmail.com";
            try
            {
                var resolvedUserId = session.GetUserId(userEmail);
                Console.WriteLine(string.Format("The ID of the user '{0}' is '{1}'.", userEmail, resolvedUserId));
            }
            catch
            {
                Prompt(string.Format("The user '{0}' could not be resolved.", userEmail));
            }

            //----------------------------------------------------------------
            //
            // get_file_info
            //
            Prompt("Press any key to request get_file_info.");

            try
            {
                var fileInfo = session.GetFileInfo(uploadedFiles[0].Id);
                Console.WriteLine(string.Format("The file '{0}' information: ", uploadedFiles[0].Id));
                Console.WriteLine(fileInfo);
            }
            catch (Exception e)
            {
                Prompt(e);
                return;
            }

            //----------------------------------------------------------------
            //
            // delete[file]
            //
            Prompt("Press any key to request delete[file].");

            try
            {
                session.DeleteFile(uploadedFiles[0].Id);
                Console.WriteLine(string.Format("The file '{0}' was deleted.", uploadedFiles[0].Id));
            }
            catch (Exception e)
            {
                Prompt(e);
                return;
            }

            //----------------------------------------------------------------
            //
            // delete[folder]
            //
            Prompt("Press any key to request delete[folder].");

            try
            {
                session.DeleteFolder(demoFolderId);
                Console.WriteLine(string.Format("The folder '{0}' was deleted.", demoFolderId));
            }
            catch (Exception e)
            {
                Prompt(e);
                return;
            }

            Prompt("Press any key to exist this application.");
        }

        static void Prompt(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }

        static void Prompt(Exception e)
        {
            Console.WriteLine("Failed: " + e.GetType());
            Console.ReadLine();
        }
    }
}
