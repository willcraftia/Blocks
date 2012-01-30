#region Using

using System;
using System.Collections.ObjectModel;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public class WindowCollection : Collection<Window>
    {
        /// <summary>
        /// このコレクションを所有する Desktop。
        /// </summary>
        protected Desktop Desktop { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="desktop">このコレクションを所有する Desktop。</param>
        public WindowCollection(Desktop desktop)
        {
            if (desktop == null) throw new ArgumentNullException("desktop");
            Desktop = desktop;
        }
    }
}
