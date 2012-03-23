#region Using

using System;
using System.IO;
using System.Net;
using System.Text;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class UploadFunctionCore
    {
        public static T Execute<T>(string uri,
            UploadFile[] files, bool share, string message, string[] emails,
            bool dumpContent, bool dumpXml)
            where T : class
        {
            var request = WebRequest.Create(uri);
            var boundary = Guid.NewGuid().ToString();
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";

            using (var contentStream = GetContentStream(boundary, files, share, message, emails, dumpContent))
            {
                request.ContentLength = contentStream.Length;

                var bytes = new byte[1024];
                int byteCount = 0;

                using (var requestStream = request.GetRequestStream())
                {
                    while ((byteCount = contentStream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        requestStream.Write(bytes, 0, byteCount);
                    }
                }
            }

            var response = request.GetResponse();
            return FunctionResult<T>.GetResult(response, dumpXml);
        }

        public static Stream GetContentStream(string boundary,
            UploadFile[] files, bool share, string message, string[] emails,
            bool dumpContent)
        {
            var sb = new StringBuilder();

            var header = "--" + boundary;

            // Content-Disposition: file
            foreach (var file in files)
            {
                sb.AppendLine(header);
                sb.Append("Content-Disposition: file; name=\"file\"; filename=\"");
                sb.Append(file.Name);
                sb.AppendLine("\"");
                sb.AppendLine("Content-Type: " + file.ContentType);
                sb.AppendLine();
                sb.AppendLine(file.Content);
            }

            // Content-Disposition: form-data
            sb.AppendLine(header);
            sb.AppendLine("Content-Disposition: form-data; name=\"share\"");
            sb.AppendLine();
            sb.AppendLine(share ? "1" : "0");

            // Content-Disposition: form-data
            if (!string.IsNullOrEmpty(message))
            {
                sb.AppendLine(header);
                sb.AppendLine("Content-Disposition: form-data; name=\"message\"");
                sb.AppendLine();
                sb.AppendLine(message);
            }

            // Content-Disposition: form-data
            if (emails != null)
            {
                foreach (var email in emails)
                {
                    sb.AppendLine(header);
                    sb.AppendLine("Content-Disposition: form-data; name=\"emails\"");
                    sb.AppendLine();
                    sb.AppendLine(email);
                }
            }

            // footer
            sb.Append("--").Append(boundary).Append("--");

            if (dumpContent) Console.WriteLine(sb);

            return new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()), false);
        }
    }
}
