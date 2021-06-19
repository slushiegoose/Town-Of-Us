using System;
using System.Linq;
using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.NeutralRoles.GlitchMod
{
    [HarmonyPatch(typeof(IntroCutscene._CoBegin_d__14), nameof(IntroCutscene._CoBegin_d__14.MoveNext))]
    internal class Start
    {
        private static void Postfix(IntroCutscene._CoBegin_d__14 __instance)
        {
            var glitch = Role.AllRoles.FirstOrDefault(x => x.RoleType == RoleEnum.Glitch);
            if (glitch != null)
            {
                ((Glitch) glitch).LastMimic = DateTime.UtcNow;
                ((Glitch) glitch).LastHack = DateTime.UtcNow;
                ((Glitch) glitch).LastKill = DateTime.UtcNow.AddSeconds(CustomGameOptions.InitialGlitchKillCooldown +
                                                                        CustomGameOptions.GlitchKillCooldown * -1);
            }
        }
    }
}