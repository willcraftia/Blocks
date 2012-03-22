#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public enum UploadResultStatus
    {
        [XmlEnum("upload_ok")]
        UploadOk,

        [XmlEnum("wrong_auth_token")]
        WrongAuthToken,

        [XmlEnum("application_restricted")]
        ApplicationRestricted,

        [XmlEnum("upload_some_files_failed")]
        UploadSomeFilesFailed,

        [XmlEnum("e_file_locked")]
        EFileLocked,

        [XmlEnum("not_enough_free_space")]
        NotEnoughFreeSpace,

        [XmlEnum("filesize_limit_exceeded")]
        FilesizeLimitExceeded,

        [XmlEnum("access_denied")]
        AccessDenied,

        [XmlEnum("upload_wrong_folder_id")]
        UploadWrongFolderId,

        [XmlEnum("upload_invalid_file_name")]
        UploadInvalidFileName,

        // API ドキュメントに記載されていないが、実際に返されたエラー。
        [XmlEnum("upload_no_files_found")]
        UploadNoFilesFound
    }
}
