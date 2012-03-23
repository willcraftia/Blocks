#region Using

using System;
using System.Net;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class GetFunctionCore
    {
        public const string UriBase = "https://www.box.net/api/1.0/rest?";

        public static T Execute<T>(string uri, bool dumpXml) where T : class
        {
            var request = WebRequest.Create(uri);
            var response = request.GetResponse();
            return FunctionResult<T>.GetResult(response, dumpXml);
        }
    }
}
