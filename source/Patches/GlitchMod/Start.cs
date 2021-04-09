using System;
using System.Linq;
using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.GlitchMod
{
    [HarmonyPatch(typeof(IntroCutscene._CoBegin_d__11), nameof(IntroCutscene._CoBegin_d__11.MoveNext))]
    class Start
    {
        static void Postfix(IntroCutscene._CoBegin_d__11 __instance)
        {
            var glitch = Role.AllRoles.FirstOrDefault(x => x.RoleType == RoleEnum.Glitch);
            if (glitch != null)
            {
                ((Glitch)glitch).LastMimic = DateTime.UtcNow;
                ((Glitch)glitch).LastHack = DateTime.UtcNow;
                ((Glitch)glitch).LastKill = DateTime.UtcNow.AddSeconds(CustomGameOptions.InitialGlitchKillCooldown + (CustomGameOptions.GlitchKillCooldown * -1));
            }
        }
    }
}
