using System.Linq;
using HarmonyLib;

namespace TownOfUs.GlitchMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class MimicUnmimic
    {
        public static void Postfix(HudManager __instance)
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Glitch))
            {
                
                var glitch = (Roles.Glitch) role;
                if (glitch.IsUsingMimic)
                {
                    Utils.Morph(glitch.Player, glitch.MimicTarget);
                } else if (glitch.MimicTarget)
                {
                    Utils.Unmorph(glitch.Player);
                }
                
            }
        }
    }
}