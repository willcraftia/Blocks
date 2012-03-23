#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public sealed class CreatedFolder
    {
        [XmlElement("folder_id")]
        public long FolderId { get; set; }

        [XmlElement("folder_name")]
        public string FolderName { get; set; }

        [XmlElement("user_id")]
        public long UserId { get; set; }

        [XmlElement("path")]
        public string Path { get; set; }

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

        [XmlElement("public_name")]
        public string PublicName { get; set; }

        [XmlElement("parent_folder_id")]
        public long ParentFolderId { get; set; }

        [XmlElement("password")]
        public string Password { get; set; }

        public override string ToString()
        {
            return "[FolderId=" + FolderId +
                ", FolderName=" + FolderName +
                ", UserId=" + UserId +
                ", Path=" + Path +
                ", Shared=" + Shared +
                ", PublicName=" + PublicName +
                ", ParentFolderId=" + ParentFolderId +
                ", Password=" + Password +
                "]";
        }
    }
}
