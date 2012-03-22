#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public class Tag
    {
        [XmlAttribute("id")]
        public long Id { get; set; }

        public override string ToString()
        {
            return "[Id=" + Id + "]";
        }
    }
}
