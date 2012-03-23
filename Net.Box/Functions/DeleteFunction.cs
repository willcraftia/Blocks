#region Using

using System;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class DeleteFunction
    {
        const string uriBase = GetFunctionCore.UriBase + "action=delete";

        public static bool DumpUri { get; set; }

        public static bool DumpXml { get; set; }

        public static DeleteResult Execute(string apiKey, string authToken, Target target, long targetId)
        {
            var uri = uriBase + "&api_key=" + apiKey + "&auth_token=" + authToken +
                "&target=" + target.ToParameterValue() + "&target_id=" + targetId;
            if (DumpUri) Console.WriteLine("URI: " + uri);

            return GetFunctionCore.Execute<DeleteResult>(uri, DumpXml);
        }
    }
}
