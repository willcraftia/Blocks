#region Using

using System;

#endregion

namespace Willcraftia.Net.Box.BuildApiKey
{
    public sealed class BoxApp
    {
        public string Id { get; set; }

        public string ApiKey { get; set; }

        #region ToString

        public override string ToString()
        {
            return "[ApplicationId=" + Id + ", ApiKey=" + ApiKey + "]";
        }

        #endregion
    }
}
