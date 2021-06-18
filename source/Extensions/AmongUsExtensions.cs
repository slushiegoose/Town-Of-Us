using System.Collections.Generic;

namespace TownOfUs.Extensions
{
    public static class AmongUsExtensions
    {
        public static KeyValuePair<byte, int> MaxPair(this Dictionary<byte, int> self, out bool tie)
        {
            tie = true;
            var result = new KeyValuePair<byte, int>(byte.MaxValue, int.MinValue);
            foreach (var keyValuePair in self)
                if (keyValuePair.Value > result.Value)
                {
                    result = keyValuePair;
                    tie = false;
                }
                else if (keyValuePair.Value == result.Value)
                {
                    tie = true;
                }

            return result;
        }

        public static KeyValuePair<byte, int> MaxPair(this byte[] self, out bool tie)
        {
            tie = true;
            var result = new KeyValuePair<byte, int>(byte.MaxValue, int.MinValue);
            for (byte i = 0; i < self.Length; i++)
                if (self[i] > result.Value)
                {
                    result = new KeyValuePair<byte, int>(i, self[i]);
                    tie = false;
                }
                else if (self[i] == result.Value)
                {
                    tie = true;
                }

            return result;
        }
    }
}