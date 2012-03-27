#region Using

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Willcraftia.Net.Box.Functions;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Service
{
    public sealed class BoxManager : IBoxService
    {
        const string statusApplicationRestricted = "application_restricted";

        // I/F
        public BoxSession Session { get; private set; }

        public string ApiKey { get; private set; }

        public BoxManager(string assemblyFile, string apiKeyClassName)
        {
            ApiKey = LoadApiKey(assemblyFile, apiKeyClassName);
        }

        // I/F
        public string GetTicket()
        {
            var result = GetTicketFunction.Execute(ApiKey);

            if (result.Status != "get_ticket_ok") HandleErrorStatus(result.Status);

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

            if (result.Status != "get_auth_token_ok") HandleErrorStatus(result.Status);

            return CreateSession(result.AuthToken);
        }

        // I/F
        public BoxSession CreateSession(string authToken)
        {
            if (authToken == null) throw new ArgumentNullException("authToken");

            Session = new BoxSession(ApiKey, authToken);
            return Session;
        }

        string LoadApiKey(string assemblyFile, string apiKeyClassName)
        {
            var assembly = Assembly.LoadFrom(assemblyFile);
            var module = assembly.GetModule(assemblyFile);
            var apiKeyType = module.GetType(apiKeyClassName);

            var fieldInfo = apiKeyType.GetField("Value", BindingFlags.Static | BindingFlags.NonPublic);
            return fieldInfo.GetValue(null) as string;
        }

        void HandleErrorStatus(string erroStatus)
        {
            if (erroStatus == statusApplicationRestricted)
                throw new BoxApplicationRestrictedException();

            throw new BoxStatusException(erroStatus);
        }
    }
}
