#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    [XmlRoot("response")]
    public sealed class CreateFolderResult
    {
        [XmlElement("status")]
        public CreateFolderStatus Status { get; set; }

        [XmlElement("folder")]
        public CreatedFolder Folder { get; set; }

        public override string ToString()
        {
            return "[Status=" + Status + ", Folder=" + Folder + "]";
        }
    }
}
