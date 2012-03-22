#region Using

using System;
using Willcraftia.Net.Box.Inputs;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class InviteCollaboratorsFunction
    {
        public static bool DumpXml { get; set; }

        public static InviteCollaboratorsResult Execute(string apiKey, string authToken,
            Target target, long targetId,
            long[] userIds, string[] emails, Role itemRole,
            bool resendInvite, bool noEmail,
            params string[] parameters)
        {
            var input = new InviteCollaboratorsInput
            {
                ApiKey = apiKey,
                AuthToken = authToken,
                Target = target,
                TargetId = targetId,
                UserIds = userIds,
                Emails = emails,
                ItemRole = itemRole,
                ResendInvite = resendInvite,
                NoEmail = noEmail,
                Parameters = parameters
            };
            var uri = input.ToUri();
            Console.WriteLine("URI: " + uri);

            return GetFunctionCore.Execute<InviteCollaboratorsResult>(uri, DumpXml);
        }
    }
}
