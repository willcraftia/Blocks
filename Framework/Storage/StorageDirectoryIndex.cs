#region Using

using System;
using System.Collections.Generic;

#endregion

namespace Willcraftia.Xna.Framework.Storage
{
    public sealed class StorageDirectoryIndex
    {
        public List<string> DirectoryNames { get; set; }

        public List<string> FileNames { get; set; }
    }
}
