#region Using

using System;
using Willcraftia.Net.Box.Inputs;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class CreateFolderFunction
    {
        public static bool DumpXml { get; set; }

        public static CreateFolderResult Execute(string apiKey, string authToken, long parentId, string name, bool share)
        {
            var input = new CreateFolderInput
            {
                ApiKey = apiKey,
                AuthToken = authToken,
                ParentId = parentId,
                Name = name,
                Share = share
            };
            var uri = input.ToUri();
            Console.WriteLine("URI: " + uri);

            return GetFunctionCore.Execute<CreateFolderResult>(uri, DumpXml);
        }
    }
}
