using HarmonyLib;
using UnityEngine;

namespace TownOfUs.EngineerMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    public static class Role
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (PlayerControl.LocalPlayer == null) return;
            if (!__instance.isEngineer()) return;
            var task = new GameObject("EngineerTask").AddComponent<ImportantTextTask>();
            task.transform.SetParent(__instance.transform, false);
            task.Text = "[FFB300FF]Role: Engineer\nFix sabotages from anywhere.[]";
            __instance.myTasks.Insert(0, task);

        }
    }
}