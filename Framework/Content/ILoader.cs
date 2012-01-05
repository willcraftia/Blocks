#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Content
{
    public interface ILoader<TResult>
    {
        TResult Load();
    }
}
