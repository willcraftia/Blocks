#region Using

using System;
using Willcraftia.Net.Box.Inputs;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public sealed class GetAuthTokenFunction
    {
        public static bool DumpXml { get; set; }

        public static GetAuthTokenResult Execute(string apiKey, string ticket)
        {
            var input = new GetAuthTokenInput
            {
                ApiKey = apiKey,
                Ticket = ticket
            };
            var uri = input.ToUri();
            Console.WriteLine("URI: " + uri);

            return GetFunctionCore.Execute<GetAuthTokenResult>(uri, DumpXml);
        }
    }
}
