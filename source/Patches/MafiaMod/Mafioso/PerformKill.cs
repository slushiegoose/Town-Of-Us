using HarmonyLib;

namespace TownOfUs.MafiaMod.Mafioso
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public class PerformKillButton

    {
        public static bool Prefix(KillButtonManager __instance)
        {
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Mafioso)) return true;
            var role = Roles.Role.GetRole<Roles.Mafioso>(PlayerControl.LocalPlayer);
            //System.Console.WriteLine(role.Godfather.Player.Data.IsDead ? "ISDEADKILL" : "ISNOTDEADKILL");
            return role.Godfather.Player.Data.IsDead;
        }
    }
}