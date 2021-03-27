using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.ChildMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    public class Murder
    {
        public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
        {
            //System.Console.WriteLine("Reaches here");
            CheckChild(target);
        }

        public static void CheckChild(PlayerControl target)
        {
            //System.Console.WriteLine("Reaches here2");
            var role = Role.GetRole(target);
            if (role == null) return;
            if (role.RoleType == RoleEnum.Child)
            {
                //System.Console.WriteLine("Reaches here2.5");
                ((Child) role).Wins();
            }
        }
    }
}