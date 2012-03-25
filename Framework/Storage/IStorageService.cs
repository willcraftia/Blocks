#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Storage
{
    public interface IStorageService
    {
        StorageDirectory RootDirectory { get; }

        void Select(string storageName);
    }
}
