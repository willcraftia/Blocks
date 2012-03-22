#region Using

using System;
using System.Net;
using Willcraftia.Net.Box.Inputs;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class UploadFunctionCore
    {
        public static T Execute<T>(string uri, UploadContent content, bool dumpXml) where T : class
        {
            var request = WebRequest.Create(uri);
            var boundary = Guid.NewGuid().ToString();
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";

            using (var contentStream = content.GetContentStream(boundary))
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
    }
}
