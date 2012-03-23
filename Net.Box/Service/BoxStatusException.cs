#region Using

using System;

#endregion

namespace Willcraftia.Net.Box.Service
{
    public sealed class BoxStatusException<T> : ApplicationException
    {
        public T Status { get; private set; }

        public BoxStatusException(T status)
        {
            Status = status;
        }
    }
}
