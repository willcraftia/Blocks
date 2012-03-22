#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public enum CollaborationStatus
    {
        [XmlEnum("accepted")]
        Accepted,

        [XmlEnum("pending")]
        Pending
    }
}
