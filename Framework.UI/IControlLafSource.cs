﻿#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public interface IControlLafSource : IDisposable
    {
        IUIContext UIContext { get; set; }

        void Initialize();

        IControlLaf GetControlLaf(Control control);
    }
}