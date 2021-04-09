using HarmonyLib;
using Hazel;


namespace TownOfUs.LoversMod
{ 
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Die))]
    public class Die
    {
        public static bool Prefix(PlayerControl __instance, [HarmonyArgument(0)] DeathReason reason)
        {
            __instance.Data.IsDead = true;
            

            var flag3 = __instance.isLover() && CustomGameOptions.BothLoversDie;
            if (!flag3) return true;
            var otherLover = Roles.Role.GetRole<Roles.Lover>(__instance).OtherLover.Player;
            if (otherLover.Data.IsDead) return true;

            if (reason == DeathReason.Exile)
            {
                AltruistMod.KillButtonTarget.DontRevive = __instance.PlayerId;
            }
            
            Utils.MurderPlayer(otherLover, otherLover);
            return true;
        }
    }
}