using System.Linq;
using HarmonyLib;

namespace TownOfUs.MorphlingMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class MorphUnmorph
    {
        public static void Postfix(HudManager __instance)
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Morphling))
            {
                
                var morphling = (Roles.Morphling) role;
                if (morphling.Morphed)
                {
                    morphling.Morph();
                } else if (morphling.MorphedPlayer)
                {
                    morphling.Unmorph();
                }
                
            }
        }
    }
}