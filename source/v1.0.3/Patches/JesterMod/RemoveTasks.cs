using System.Collections.Generic;
using HarmonyLib;

namespace TownOfUs.JesterMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class RemoveTasks
    {
        public static void Postfix(HudManager __instance)
        {
            if (Utils.Jester == null) return;
            var removeTask = new List<PlayerTask>();
            foreach (var task in Utils.Jester.myTasks)
            {
                if (task.TaskType != TaskTypes.FixComms && task.TaskType != TaskTypes.FixLights &&
                    //task.TaskType != TaskTypes.ResetReactor && task.TaskType != TaskTypes.ResetSeismic &&
                    //task.TaskType != TaskTypes.RestoreOxy &&
                    task.gameObject.GetComponent<ImportantTextTask>() == null)
                {
                    removeTask.Add(task);
                }
            }

            foreach (var task in removeTask)
            {
                Utils.Jester.RemoveTask(task);
            }
        }
    }
}