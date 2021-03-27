using HarmonyLib;
using UnityEngine;

namespace TownOfUs.SwapperMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    public static class Role
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!__instance.isSwapper()) return;
            var task = new GameObject("SwapperTask").AddComponent<ImportantTextTask>();
            task.transform.SetParent(__instance.transform, false);
            task.Text = "[66E666FF]Role: Swapper\nSwap two people's votes and wreak havoc![]";
            __instance.myTasks.Insert(0, task);

        }
    }
}