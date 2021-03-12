using HarmonyLib;
using UnityEngine;

namespace TownOfUs.TimeMasterMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    public static class Role
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (PlayerControl.LocalPlayer == null) return;
            if (!__instance.isTimeMaster()) return;
            var task = new GameObject("TimeMasterTask").AddComponent<ImportantTextTask>();
            task.transform.SetParent(__instance.transform, false);
            task.Text = "[0000FFFF]Role: Time Master\nRewind Time![]";
            __instance.myTasks.Insert(0, task);

        }
    }
}