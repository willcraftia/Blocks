#region Using

using System;
using System.Collections.ObjectModel;

#endregion

namespace Willcraftia.Xna.Framework.UI.ScreenFactories
{
    public sealed class ScreenDefinitionCollection : KeyedCollection<string, ScreenDefinition>
    {
        protected override string GetKeyForItem(ScreenDefinition item)
        {
            return item.Name;
        }
    }
}
