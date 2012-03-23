#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public enum GetFileInfoResultStatus
    {
        [XmlEnum("s_get_file_info")]
        SGetFileInfo,
        
        [XmlEnum("not_logged_in")]
        NotLoggedIn,
        
        [XmlEnum("application_restricted")]
        ApplicationRestricted,
        
        [XmlEnum("e_access_denied")]
        EAccessDenied
    }
}
