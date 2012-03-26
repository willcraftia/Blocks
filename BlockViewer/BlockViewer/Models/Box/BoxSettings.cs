#region Using

using System;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Models.Box
{
    // TODO
    public sealed class BoxSettings
    {
        public string AuthToken { get; set; }

        public long BlocksFolderId { get; set; }

        public long MeshesFolderId { get; set; }

        public BoxSettings()
        {
            BlocksFolderId = -1;
            MeshesFolderId = -1;
        }
    }
}
