#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public sealed class AccountTree
    {
        [XmlElement("folder")]
        public Folder Folder { get; set; }

        public override string ToString()
        {
            return "[Folder=" + Folder + "]";
        }
    }
}
