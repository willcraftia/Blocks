#region Using

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using Willcraftia.Net.Box.Functions;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Service
{
    public sealed class BoxManager
    {
        BoxSession session;

        public string ApiKey { get; private set; }

        public GetTicketResult GetTicketResult { get; private set; }

        public GetAuthTokenResult GetAuthTokenResult { get; private set; }

        public string Ticket
        {
            get { return GetTicketSucceeded ? GetTicketResult.Ticket : null; }
        }

        public string AuthToken
        {
            get { return GetAuthTokenSucceeded ? GetAuthTokenResult.AuthToken : null; }
        }

        public bool GetTicketSucceeded
        {
            get { return GetTicketResult != null && GetTicketResult.Succeeded; }
        }

        public bool GetAuthTokenSucceeded
        {
            get { return GetAuthTokenResult != null && GetAuthTokenResult.Succeeded; }
        }

        public BoxManager(string apiKey)
        {
            if (apiKey == null) throw new ArgumentNullException("apiKey");
            ApiKey = apiKey;
        }

        public bool GetTicket()
        {
            InvalidateTicket();
            GetTicketResult = GetTicketFunction.Execute(ApiKey);
            return GetTicketSucceeded;
        }

        public void RedirectUserAuth()
        {
            if (!GetTicketSucceeded) throw new InvalidOperationException("No ticket exists.");

            var uri = "https://www.box.net/api/1.0/auth/" + Ticket;
            Process.Start(uri);
        }

        public bool GetAuthToken()
        {
            if (!GetTicketSucceeded) throw new InvalidOperationException("No ticket exists.");

            GetAuthTokenResult = GetAuthTokenFunction.Execute(ApiKey, Ticket);
            return GetAuthTokenSucceeded;
        }

        public BoxSession CreateSession()
        {
            if (!GetAuthTokenSucceeded) throw new InvalidOperationException("The ticket is not authorized.");

            if (session == null || !session.Valid) session = new BoxSession(this);
            return session;
        }

        internal LogoutResult Logout()
        {
            if (session == null || !session.Valid)
                throw new InvalidOperationException("No session exists.");

            var result = LogoutFunction.Execute(ApiKey, AuthToken);
            InvalidateTicket();
            return result;
        }

        void InvalidateTicket()
        {
            GetTicketResult = null;
            GetAuthTokenResult = null;
            if (session != null) session.Valid = false;
        }
    }
}
