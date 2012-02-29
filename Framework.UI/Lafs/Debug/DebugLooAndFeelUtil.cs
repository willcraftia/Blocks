#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public static class DebugLooAndFeelUtil
    {
        public static ILookAndFeelSource CreateLookAndFeelSource(Game game)
        {
            var source = new DefaultLookAndFeelSource();
            
            source.LookAndFeelMap[typeof(Control)] = new DebugDefaultLookAndFeel();
            source.LookAndFeelMap[typeof(Window)] = new DebugWindowLookAndFeel();
            source.LookAndFeelMap[typeof(Controls.TextBlock)] = new DebugTextBlockLookAndFeel();
            
            return source;
        }
    }
}
