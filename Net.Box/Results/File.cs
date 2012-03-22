#region Using

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public sealed class File
    {
        [XmlAttribute("id")]
        public long Id { get; set; }

        [XmlAttribute("file_name")]
        public string FileName { get; set; }

        [XmlAttribute("user_id")]
        public long UserId { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }

        [XmlIgnore]
        public bool Shared { get; set; }

        /// <summary>
        /// string へマッピングさせてから解析した値を Shared プロパティへ設定します。
        /// </summary>
        [XmlAttribute("shared")]
        public string SharedString
        {
            get { return Shared ? "1" : "0"; }
            set { Shared = ("1" == value); }
        }

        [XmlAttribute("shared_link")]
        public string SharedLink { get; set; }

        [XmlAttribute("created")]
        public long Created { get; set; }

        [XmlAttribute("updated")]
        public long Updated { get; set; }

        [XmlAttribute("size")]
        public long Size { get; set; }

        [XmlArray("tags")]
        [XmlArrayItem("tag")]
        public List<Tag> Tags { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("[Id=").Append(Id);
            sb.Append(", FileName=").Append(FileName);
            sb.Append(", UserId=").Append(UserId);
            sb.Append(", Description=").Append(Description);
            sb.Append(", Shared=").Append(Shared);
            sb.Append(", SharedLink=").Append(SharedLink);
            sb.Append(", Created=").Append(Created);
            sb.Append(", Updated=").Append(Updated);
            sb.Append(", Size=").Append(Size);
            sb.Append(", Tags=[");
            for (int i = 0; i < Tags.Count; i++)
            {
                sb.Append(Tags[i]);
                if (i < Tags.Count - 1) sb.Append(", ");
            }
            sb.Append("]");
            sb.Append("]");
            return sb.ToString();
        }
    }
}
