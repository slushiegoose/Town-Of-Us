using System;
using System.Linq;
using HarmonyLib;

namespace TownOfUs.SpyMod
{
    [HarmonyPatch(typeof(VitalsMinigame), nameof(VitalsMinigame.Update))]
    public class Vitals
    {
        public static void Postfix(VitalsMinigame __instance)
        {
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Spy)) return;
            for(int i=0; i<__instance.vitals.Count; i++)
            {;
                var panel = __instance.vitals[i];
                var info = GameData.Instance.AllPlayers.ToArray()[i];
                if (!panel.IsDead) continue;
                var deadBody = MedicMod.Murder.KilledPlayers.First(x => x.PlayerId == info.PlayerId);
                var num = (float) (DateTime.UtcNow - deadBody.KillTime).TotalMilliseconds;
                panel.Text.Text = Math.Round(num/1000f) + "s";
            }
        }
    }
}