#region Using

using System;
using Willcraftia.Net.Box.Inputs;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class GetTicketFunction
    {
        public static bool DumpXml { get; set; }

        public static GetTicketResult Execute(string apiKey)
        {
            var input = new GetTicketInput { ApiKey = apiKey };
            var uri = input.ToUri();
            Console.WriteLine("URI: " + uri);

            return GetFunctionCore.Execute<GetTicketResult>(uri, DumpXml);
        }
    }
}
