using System.Collections.Generic;
using HarmonyLib;

namespace TownOfUs.ShifterMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class RemoveTasks
    {
        public static void Postfix(HudManager __instance)
        {
            if (Utils.Shifter == null) return;
            var removeTask = new List<PlayerTask>();
            if (Utils.Shifter.myTasks == null || Utils.Shifter.myTasks.Count <= 0) return;
            foreach (var task in Utils.Shifter.myTasks)
            {
                if (task == null) continue;
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
                Utils.Shifter.RemoveTask(task);
            }
        }
    }
}