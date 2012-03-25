#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework;
using Willcraftia.Net.Box.Service;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.ViewModels.Box
{
    public sealed class BoxSetupViewModel
    {
        public delegate string GetTicketDelegate();

        public delegate BoxSession GetAuthTokenDelegate(string ticket);

        public event EventHandler GotTicket = delegate { };

        public event EventHandler AccountSucceeded = delegate { };

        IBoxService boxService;

        string ticket;

        GetTicketDelegate getTicketDelegate;

        GetAuthTokenDelegate getAuthTokenDelegate;

        readonly object syncRoot = new object();

        bool fireGotTicket;

        bool fireAccountSucceeded;

        public BoxSetupViewModel(Game game)
        {
            boxService = game.Services.GetRequiredService<IBoxService>();
        }

        public void Update()
        {
            lock (syncRoot)
            {
                if (fireGotTicket)
                {
                    GotTicket(this, EventArgs.Empty);
                    fireGotTicket = false;
                }
                if (fireAccountSucceeded)
                {
                    AccountSucceeded(this, EventArgs.Empty);
                    fireAccountSucceeded = false;
                }
            }
        }

        public void GetTicketAsync()
        {
            if (getTicketDelegate == null)
                getTicketDelegate = new GetTicketDelegate(boxService.GetTicket);
            getTicketDelegate.BeginInvoke(GetTicketAsyncCallback, null);
        }

        public void LauchAuthorizationPageOnBrowser()
        {
            boxService.RedirectUserAuth(ticket);
        }

        public void AccessAccount()
        {
            if (getAuthTokenDelegate == null)
                getAuthTokenDelegate = new GetAuthTokenDelegate(boxService.GetAuthToken);
            getAuthTokenDelegate.BeginInvoke(ticket, GetAuthTokenAsyncCallback, null);
        }

        void GetTicketAsyncCallback(IAsyncResult asyncResult)
        {
            lock (syncRoot)
            {
                ticket = getTicketDelegate.EndInvoke(asyncResult);
                fireGotTicket = true;
            }
        }

        void GetAuthTokenAsyncCallback(IAsyncResult asyncResult)
        {
            lock (syncRoot)
            {
                getAuthTokenDelegate.EndInvoke(asyncResult);
                fireAccountSucceeded = true;
            }
        }
    }
}
