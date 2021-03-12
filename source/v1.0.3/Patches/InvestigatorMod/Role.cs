using HarmonyLib;
using UnityEngine;

namespace TownOfUs.InvestigatorMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    public static class Role
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (PlayerControl.LocalPlayer == null) return;
            if (!__instance.isInvestigator()) return;
            var task = new GameObject("InvestigatorTask").AddComponent<ImportantTextTask>();
            task.transform.SetParent(__instance.transform, false);
            task.Text = "[00B3B3FF]Role: Investigator\nYou can see everyone's footprints.[]";
            __instance.myTasks.Insert(0, task);

        }
    }
}