#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    [XmlRoot("response")]
    public sealed class DeleteResult
    {
        [XmlElement("status")]
        public DeleteResultStatus Status { get; set; }

        public bool Succeeded
        {
            get { return Status == DeleteResultStatus.SDeleteNode; }
        }

        public override string ToString()
        {
            return "[Status=" + Status + "]";
        }
    }
}
