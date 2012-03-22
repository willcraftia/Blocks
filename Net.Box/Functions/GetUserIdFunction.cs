#region Using

using System;
using Willcraftia.Net.Box.Inputs;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class GetUserIdFunction
    {
        public static bool DumpXml { get; set; }

        public static GetUserIdResult Execute(string apiKey, string authToken, string login)
        {
            var input = new GetUserIdInput
            {
                ApiKey = apiKey,
                AuthToken = authToken,
                Login = login
            };
            var uri = input.ToUri();
            Console.WriteLine("URI: " + uri);

            return GetFunctionCore.Execute<GetUserIdResult>(uri, DumpXml);
        }
    }
}
