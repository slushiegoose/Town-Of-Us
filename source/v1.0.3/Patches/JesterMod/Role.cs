using HarmonyLib;
using UnityEngine;

namespace TownOfUs.JesterMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    public static class Role
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (PlayerControl.LocalPlayer == null) return;
            if (!__instance.isJester()) return;
            var task = new GameObject("JesterTask").AddComponent<ImportantTextTask>();
            task.transform.SetParent(__instance.transform, false);
            task.Text = "[FFBFCCFF]Role: Jester\nGet voted out![]";
            __instance.myTasks.Insert(0, task);

        }
    }
}