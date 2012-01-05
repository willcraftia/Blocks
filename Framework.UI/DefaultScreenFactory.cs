#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public class DefaultScreenFactory : IScreenFactory
    {
        Game game;

        Dictionary<string, Type> screens = new Dictionary<string,Type>();

        public Type this[string screenName]
        {
            get { return screens[screenName]; }
            set { screens[screenName] = value; }
        }

        public DefaultScreenFactory(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");
            this.game = game;
        }

        // I/F
        public Screen CreateScreen(string screenName)
        {
            if (screenName == null) throw new ArgumentNullException("screenName");

            Type screenType = null;
            if (!screens.TryGetValue(screenName, out screenType)) throw new ArgumentException("Screen type not found.", "screenName");

            return Activator.CreateInstance(screenType, game) as Screen;
        }
    }
}
