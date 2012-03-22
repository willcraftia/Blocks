#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    [XmlRoot("response")]
    public sealed class GetAccountTreeResult
    {
        public sealed class ResultTree
        {
            [XmlElement("folder")]
            public Folder Folder { get; set; }

            public override string ToString()
            {
                return "[Folder=" + Folder + "]";
            }
        }

        [XmlElement("status")]
        public GetAccountTreeResultStatus Status { get; set; }

        [XmlElement("tree")]
        public ResultTree Tree { get; set; }

        public bool Succeeded
        {
            get { return Status == GetAccountTreeResultStatus.ListingOk; }
        }

        public override string ToString()
        {
            return "[Status=" + Status +
                ", Tree=" + Tree +
                "]";
        }
    }
}
