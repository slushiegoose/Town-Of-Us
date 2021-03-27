using HarmonyLib;
using UnityEngine;

namespace TownOfUs.MayorMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    public static class Role
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (PlayerControl.LocalPlayer == null) return;
            if (!__instance.isMayor()) return;
            var task = new GameObject("MayorTask").AddComponent<ImportantTextTask>();
            task.transform.SetParent(__instance.transform, false);
            task.Text = "[704FA8FF]Role: Mayor\nSave your votes to vote multiple times.[]";
            __instance.myTasks.Insert(0, task);

        }
    }
}