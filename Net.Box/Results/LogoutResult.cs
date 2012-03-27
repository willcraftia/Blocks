#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    [XmlRoot("response")]
    public sealed class LogoutResult
    {
        [XmlElement("status")]
        public string Status { get; set; }

        public override string ToString()
        {
            return "[Status=" + Status + "]";
        }
    }
}
