#region Using

using System;
using System.Diagnostics;
using System.Reflection;
using Willcraftia.Net.Box.Functions;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Service
{
    public sealed class BoxManager : IBoxService
    {
        public string ApiKey { get; private set; }

        public BoxManager(string assemblyFile, string apiKeyClassName)
        {
            ApiKey = LoadApiKey(assemblyFile, apiKeyClassName);
        }

        string LoadApiKey(string assemblyFile, string apiKeyClassName)
        {
            var assembly = Assembly.LoadFrom(assemblyFile);
            var module = assembly.GetModule(assemblyFile);
            var apiKeyType = module.GetType(apiKeyClassName);

            var fieldInfo = apiKeyType.GetField("Value", BindingFlags.Static | BindingFlags.NonPublic);
            return fieldInfo.GetValue(null) as string;
        }

        // I/F
        public string GetTicket()
        {
            var result = GetTicketFunction.Execute(ApiKey);

            if (result.Status != GetTicketStatus.GetTicketOk)
                throw new BoxStatusException<GetTicketStatus>(result.Status);

            return result.Ticket;
        }

        // I/F
        public void RedirectUserAuth(string ticket)
        {
            if (ticket == null) throw new ArgumentNullException("ticket");

            var uri = "https://www.box.net/api/1.0/auth/" + ticket;
            Process.Start(uri);
        }

        // I/F
        public BoxSession GetAuthToken(string ticket)
        {
            if (ticket == null) throw new ArgumentNullException("ticket");

            var result = GetAuthTokenFunction.Execute(ApiKey, ticket);

            if (result.Status != GetAuthTokenStatus.GetAuthTokenOk)
                throw new BoxStatusException<GetAuthTokenStatus>(result.Status);

            return CreateSession(result.AuthToken);
        }

        // I/F
        public BoxSession CreateSession(string authToken)
        {
            if (authToken == null) throw new ArgumentNullException("authToken");

            return new BoxSession(ApiKey, authToken);
        }
    }
}
