#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public enum GetAccountTreeResultStatus
    {
        [XmlEnum("listing_ok")]
        ListingOk,

        [XmlEnum("application_restricted")]
        ApplicationRestricted,

        [XmlEnum("not_logged_in")]
        NotLoggedIn,

        [XmlEnum("e_folder_id")]
        EFolderId
    }
}
