using System.Collections.Generic;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;
using Hazel;
using UnityEngine;

namespace TownOfUs.Extensions
{
    public static class AmongUsExtensions
    {
        private const float MIN = -50f;
        private const float MAX = 50f;
        private static float ReverseLerp(float t)
        {
            return Mathf.Clamp((t - MIN) / (MAX - MIN), 0f, 1f);
        }

        public static void Write(this MessageWriter writer, Vector3 value)
        {
            var x = (ushort)(ReverseLerp(value.x) * ushort.MaxValue);
            var y = (ushort)(ReverseLerp(value.y) * ushort.MaxValue);
            var z = (ushort)(ReverseLerp(value.z) * ushort.MaxValue);

            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
        }

        public static Vector3 ReadVector3(this MessageReader reader)
        {
            var x = reader.ReadUInt16() / (float)ushort.MaxValue;
            var y = reader.ReadUInt16() / (float)ushort.MaxValue;
            var z = reader.ReadUInt16() / (float)ushort.MaxValue;

            return new Vector3(
                Mathf.Lerp(MIN, MAX, x),
                Mathf.Lerp(MIN, MAX, y),
                Mathf.Lerp(MIN, MAX, z)
            );
        }

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

        public static VisualAppearance GetDefaultAppearance(this PlayerControl player)
        {
            return new VisualAppearance()
            {
                ColorId = player.Data.ColorId,
                HatId = player.Data.HatId,
                SkinId = player.Data.SkinId,
                PetId = player.Data.PetId,
            };
        }

        public static bool TryGetAppearance(this PlayerControl player, IVisualAlteration modifier, out VisualAppearance appearance)
        {
            if (modifier != null)
                return modifier.TryGetModifiedAppearance(out appearance);

            appearance = player.GetDefaultAppearance();
            return false;
        }

        public static VisualAppearance GetAppearance(this PlayerControl player)
        {
            if (player.TryGetAppearance(Role.GetRole(player) as IVisualAlteration, out var appearance))
                return appearance;
            else if (player.TryGetAppearance(Modifier.GetModifier(player) as IVisualAlteration, out appearance))
                return appearance;
            else
                return player.GetDefaultAppearance();
        }
    }
}
