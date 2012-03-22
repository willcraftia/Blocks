#region Using

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public sealed class Folder
    {
        [XmlAttribute("id")]
        public long Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

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

        [XmlAttribute("permissions")]
        public string Permissions { get; set; }

        [XmlArray("tags")]
        [XmlArrayItem("tag")]
        public List<Tag> Tags { get; set; }

        [XmlArray("files")]
        [XmlArrayItem("file")]
        public List<File> Files { get; set; }

        [XmlArray("folders")]
        [XmlArrayItem("folder")]
        public List<Folder> Folders { get; set; }

        public Folder FindFolderByName(string name)
        {
            if (Name == name) return this;

            foreach (var child in Folders)
            {
                var target = child.FindFolderByName(name);
                if (target != null) return target;
            }

            return null;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("[Id=").Append(Id);
            sb.Append(", Name=").Append(Name);
            sb.Append(", UserId=").Append(UserId);
            sb.Append(", Description=").Append(Description);
            sb.Append(", Shared=").Append(Shared);
            sb.Append(", SharedLink=").Append(SharedLink);
            sb.Append(", Permissions=").Append(Permissions);
            sb.Append(", Tags=[");
            for (int i = 0; i < Tags.Count; i++)
            {
                sb.Append(Tags[i]);
                if (i < Tags.Count - 1) sb.Append(", ");
            }
            sb.Append("]");
            sb.Append(", Files=[");
            for (int i = 0; i < Files.Count; i++)
            {
                sb.Append(Files[i]);
                if (i < Files.Count - 1) sb.Append(", ");
            }
            sb.Append("]");
            sb.Append(", Folders=[");
            for (int i = 0; i < Folders.Count; i++)
            {
                sb.Append(Folders[i]);
                if (i < Folders.Count - 1) sb.Append(", ");
            }
            sb.Append("]");
            sb.Append("]");
            return sb.ToString();
        }
    }
}
