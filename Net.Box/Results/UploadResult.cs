#region Using

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    [XmlRoot("response")]
    public sealed class UploadResult
    {
        public sealed class ResultFile
        {
            [XmlAttribute("file_name")]
            public string FileName { get; set; }

            [XmlAttribute("id")]
            public long Id { get; set; }

            [XmlAttribute("folder_id")]
            public long FolderId { get; set; }

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

            [XmlAttribute("public_name")]
            public string PublicName { get; set; }

            [XmlAttribute("error")]
            public string Error { get; set; }

            public override string ToString()
            {
                return "[FileName=" + FileName +
                    ", Id=" + Id +
                    ", FolderId=" + FolderId +
                    ", Shared=" + Shared +
                    ", PublicName=" + PublicName +
                    ", Error=" + Error +
                    "]";
            }
        }

        [XmlElement("status")]
        public UploadResultStatus Status { get; set; }

        [XmlArray("files")]
        [XmlArrayItem("file")]
        public List<ResultFile> Files { get; set; }

        public bool Succeeded
        {
            get { return Status == UploadResultStatus.UploadOk; }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("[Status=").Append(Status);
            sb.Append(", Files=[");
            for (int i = 0; i < Files.Count; i++)
            {
                sb.Append(Files[i]);
                if (i < Files.Count - 1) sb.Append(", ");
            }
            sb.Append("]");
            sb.Append("]");
            return sb.ToString();
        }
    }
}
