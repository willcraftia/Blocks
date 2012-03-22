#region Using

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Willcraftia.Net.Box.BuildApiKey
{
    public sealed class BoxAppSettings
    {
        public List<BoxApp> BoxApps { get; set; }

        public BoxApp FindBoxAppById(string id)
        {
            foreach (var boxApp in BoxApps)
            {
                if (boxApp.Id == id) return boxApp;
            }
            return null;
        }

        #region ToString

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("[BoxApps=[");
            for (int i = 0; i < BoxApps.Count; i++)
            {
                sb.Append(BoxApps[i]);
                if (i < BoxApps.Count - 1) sb.Append(", ");
            }
            sb.Append("]]");

            return sb.ToString();
        }

        #endregion
    }
}
