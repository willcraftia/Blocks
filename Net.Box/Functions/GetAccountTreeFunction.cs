#region Using

using System;
using Willcraftia.Net.Box.Inputs;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class GetAccountTreeFunction
    {
        public static bool DumpXml { get; set; }

        public static GetAccountTreeResult Execute(string apiKey, string authToken, long folderId, params string[] parameters)
        {
            var input = new GetAccountTreeInput
            {
                ApiKey = apiKey,
                AuthToken = authToken,
                FolderId = folderId,
                Parameters = parameters
            };
            var uri = input.ToUri();
            Console.WriteLine("URI: " + uri);

            return GetFunctionCore.Execute<GetAccountTreeResult>(uri, DumpXml);
        }
    }
}
