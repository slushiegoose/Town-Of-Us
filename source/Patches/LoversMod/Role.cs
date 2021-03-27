using HarmonyLib;
using UnityEngine;

namespace TownOfUs.LoversMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    public static class Role
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (PlayerControl.LocalPlayer == null) return;
            if (!__instance.isLover()) return;
            var task = new GameObject("LoversTask").AddComponent<ImportantTextTask>();
            task.transform.SetParent(__instance.transform, false);
            task.GenTaskText(__instance);
            __instance.myTasks.Insert(0, task);

        }

        public static void GenTaskText(this ImportantTextTask task, PlayerControl __instance)
        {
            var role = __instance.Data.IsImpostor ? "Loving [FF0000FF]Impostor[FF80D5FF]" : "Lover";
            var lover = __instance.OtherLover();
            task.Text = $"[FF80D5FF]Role: {role}\nStay alive with your love {lover.name} \nand win together[]";
                
        }
    }
}