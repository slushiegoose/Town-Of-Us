using HarmonyLib;
using UnityEngine;

namespace TownOfUs.SheriffMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    public static class Role
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (PlayerControl.LocalPlayer == null) return;
            if (!__instance.isSheriff()) return;
            var task = new GameObject("SheriffTask").AddComponent<ImportantTextTask>();
            task.transform.SetParent(__instance.transform, false);
            task.Text = "[FFFF00FF]Role: Sheriff\nKill off the impostor but don't kill crewmates.[]";
            __instance.myTasks.Insert(0, task);

        }
    }
}