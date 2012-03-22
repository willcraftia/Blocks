#region Using

using System;
using Willcraftia.Net.Box.Inputs;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class LogoutFunction
    {
        public static bool DumpXml { get; set; }

        public static LogoutResult Execute(string apiKey, string authToken)
        {
            var input = new LogoutInput
            {
                ApiKey = apiKey,
                AuthToken = authToken
            };
            var uri = input.ToUri();
            Console.WriteLine("URI: " + uri);

            return GetFunctionCore.Execute<LogoutResult>(uri, DumpXml);
        }
    }
}
