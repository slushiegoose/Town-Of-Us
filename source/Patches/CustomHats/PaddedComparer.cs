using System;
using System.Collections.Generic;
using System.Linq;
using Reactor;

namespace TownOfUs.Patches.CustomHats
{
    public class PaddedComparer<T> : IComparer<T> where T : IComparable
    {
        private readonly T[] _forcedToBottom;
        public PaddedComparer(params T[] forcedToBottom)
        {
            _forcedToBottom = forcedToBottom;
        }

        public int Compare(T x, T y)
        {
            if (_forcedToBottom.Contains(x) && _forcedToBottom.Contains(y))
                return StringComparer.InvariantCulture.Compare(x, y);

            if (_forcedToBottom.Contains(x))
                return 1;
            if (_forcedToBottom.Contains(y))
                return -1;

            return StringComparer.InvariantCulture.Compare(x, y);
        }
    }
}