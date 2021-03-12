using HarmonyLib;
using UnityEngine;

namespace TownOfUs.ShifterMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    public static class Role
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (PlayerControl.LocalPlayer == null) return;
            if (!__instance.isShifter()) return;
            var task = new GameObject("ShifterTask").AddComponent<ImportantTextTask>();
            task.transform.SetParent(__instance.transform, false);
            task.Text = "[999999FF]Role: Shifter\nSteal other people's roles.[]";
            __instance.myTasks.Insert(0, task);

        }
    }
}