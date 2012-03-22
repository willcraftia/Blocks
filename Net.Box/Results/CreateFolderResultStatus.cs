#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public enum CreateFolderResultStatus
    {
        [XmlEnum("create_ok")]
        CreateOk,

        [XmlEnum("no_parent")]
        NoParent,

        [XmlEnum("s_folder_exists")]
        SFolderExists,

        [XmlEnum("not_logged_in")]
        NotLoggedIn,

        [XmlEnum("application_restricted")]
        ApplicationRestricted,

        [XmlEnum("invalid_folder_name")]
        InvalidFolderName,

        [XmlEnum("e_no_access")]
        ENoAccess,

        [XmlEnum("e_no_folder_name")]
        ENoFolderName,

        [XmlEnum("folder_name_too_big")]
        FolderNameTooBig,

        [XmlEnum("e_input_params")]
        EInputParams,
    }
}
