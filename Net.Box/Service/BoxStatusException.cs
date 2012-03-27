#region Using

using System;

#endregion

namespace Willcraftia.Net.Box.Service
{
    public sealed class BoxStatusException : ApplicationException
    {
        public string Status { get; private set; }

        public BoxStatusException(string status)
            : base(string.Format("Box API returned an error code '{0}'.", status))
        {
            Status = status;
        }
    }
}
