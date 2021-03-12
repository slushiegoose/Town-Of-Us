using HarmonyLib;
using UnityEngine;

namespace TownOfUs.MafiaMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    public static class Role
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.isGodfather())
            {
                var task = new GameObject("GodfatherTask").AddComponent<ImportantTextTask>();
                task.transform.SetParent(__instance.transform, false);
                task.Text = "[FF0000FF]Role: Godfather\nKill all the crewmates.[]";
                __instance.myTasks.Insert(0, task);
            }
            if (PlayerControl.LocalPlayer.isMafioso())
            {
                var task = new GameObject("MafiosoTask").AddComponent<ImportantTextTask>();
                task.transform.SetParent(__instance.transform, false);
                task.Text = "[FF0000FF]Role: Mafioso\nInherit the Godfather once they die.[]";
                __instance.myTasks.Insert(0, task);
            }
            if (PlayerControl.LocalPlayer.isJanitor())
            {
                var task = new GameObject("JanitorTask").AddComponent<ImportantTextTask>();
                task.transform.SetParent(__instance.transform, false);
                task.Text = "[FF0000FF]Role: Janitor\nClean up bodies.[]";
                __instance.myTasks.Insert(0, task);
            }

        }
    }
}