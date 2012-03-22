#region Using

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Willcraftia.Net.Box.Inputs
{
    public sealed class UploadContent
    {
        public sealed class File
        {
            public string Name { get; set; }

            public string Content { get; set; }

            // text/xml;charset=utf-8
            public string ContentType { get; set; }
        }

        StringBuilder contents = new StringBuilder();

        public List<File> Files { get; private set; }

        public bool Share { get; set; }

        public string Message { get; set; }

        public List<string> Emails { get; private set; }

        public UploadContent()
        {
            Files = new List<File>();
            Emails = new List<string>();
        }

        public Stream GetContentStream(string boundary)
        {
            var header = "--" + boundary;

            foreach (var file in Files)
            {
                contents.AppendLine(header);
                contents.Append("Content-Disposition: file; name=\"file\"; filename=\"");
                contents.Append(file.Name);
                contents.AppendLine("\"");
                contents.AppendLine("Content-Type: " + file.ContentType);
                contents.AppendLine();
                contents.AppendLine(file.Content);
            }

            contents.AppendLine(header);
            contents.AppendLine("Content-Disposition: form-data; name=\"share\"");
            contents.AppendLine();
            contents.AppendLine(Share ? "1" : "0");

            if (!string.IsNullOrEmpty(Message))
            {
                contents.AppendLine(header);
                contents.AppendLine("Content-Disposition: form-data; name=\"message\"");
                contents.AppendLine();
                contents.AppendLine(Message);
            }

            foreach (var email in Emails)
            {
                contents.AppendLine(header);
                contents.AppendLine("Content-Disposition: form-data; name=\"emails\"");
                contents.AppendLine();
                contents.AppendLine(email);
            }

            // footer
            contents.Append("--").Append(boundary).Append("--");

            Console.WriteLine(contents);

            byte[] bytes = Encoding.UTF8.GetBytes(contents.ToString());
            contents.Length = 0;

            return new MemoryStream(bytes, false);
        }
    }
}
