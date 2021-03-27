using HarmonyLib;

namespace TownOfUs.SwapperMod
{
    [HarmonyPatch(typeof(Console), nameof(Console.Method_61))]
    public class NoSabotage
    {

        public static bool Prefix(Console __instance, [HarmonyArgument(0)] PlayerControl pc, ref PlayerTask __result)
        {
            if (!pc.isSwapper()) return true;
            foreach (var task in pc.myTasks)
            {
                if (task.IsComplete || !task.ValidConsole(__instance)) continue;
                
                if (task.TaskType == TaskTypes.FixLights || //task.TaskType == TaskTypes.RestoreOxy ||
                    //task.TaskType == TaskTypes.ResetReactor || task.TaskType == TaskTypes.ResetSeismic ||
                    task.TaskType == TaskTypes.FixComms)
                {
                    __result = null;
                }
                else
                {
                    __result = task;
                }

                return false;
            }

            __result = null;
            return false;
        }
        
        
    }
}