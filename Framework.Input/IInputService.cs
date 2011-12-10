#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    public interface IInputService
    {
        IMouse Mouse { get; }
    }
}
