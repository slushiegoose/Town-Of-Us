using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.NeutralRoles.PhantomMod
{
    [HarmonyPatch(typeof(SpawnInMinigame), nameof(SpawnInMinigame.Begin))]
    public class NoSpawn
    {
        public static bool Prefix(SpawnInMinigame __instance)
        {
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Phantom))
            {
                var caught = Role.GetRole<Phantom>(PlayerControl.LocalPlayer).Caught;
                if (!caught)
                {
                    __instance.Close();
                    return false;
                }
            }

            return true;
        }
    }
}