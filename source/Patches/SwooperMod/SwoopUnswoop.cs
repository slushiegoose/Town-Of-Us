using HarmonyLib;

namespace TownOfUs.SwooperMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class SwoopUnswoop
    {
        [HarmonyPriority(Priority.Last)]
        public static void Postfix(HudManager __instance)
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Swooper))
            {
                
                var swooper = (Roles.Swooper) role;
                if (swooper.IsSwooped)
                {
                    swooper.Swoop();
                } else if (swooper.Enabled)
                {
                    swooper.UnSwoop();
                }
                
            }
        }
    }
}