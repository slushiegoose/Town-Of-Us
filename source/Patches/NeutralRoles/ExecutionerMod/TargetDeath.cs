using HarmonyLib;
using Hazel;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.NeutralRoles.ExecutionerMod
{
    public enum OnTargetDead
    {
        Crew,
        Jester
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Die))]
    public class TargetDeath
    {
        public static void Postfix(
            PlayerControl __instance,
            [HarmonyArgument(0)] DeathReason reason)
        {
            var exec = Role.GetRole<Executioner>();
            var target = exec?.Target;
            if (target == null || exec.Player.Data.IsDead || target.PlayerId != __instance.PlayerId) return;

            if (reason == DeathReason.Exile)
                exec.Wins();
            else
                ExeToJes(exec.Player);
        }

        public static void ExeToJes(PlayerControl player)
        {
            var amOwner = player.AmOwner;
            if (amOwner) player.myTasks.RemoveAt(0);
            Role.RoleDictionary.Remove(player.PlayerId);

            if (CustomGameOptions.OnTargetDead == OnTargetDead.Jester)
            {
                var jester = new Jester(player);
                if (amOwner)
                {
                    var task = new GameObject("JesterTask").AddComponent<ImportantTextTask>();
                    task.transform.SetParent(player.transform, false);
                    task.Text = Utils.ColorText(
                        jester.Color,
                        $"Role: {jester.Name}\nYour target was killed. Now you get voted out!"
                    );
                    player.myTasks.Insert(0, task);
                }
            }
            else
                new Crewmate(player);
            Role.NamePatch.UpdateSingle(player);
        }
    }
}
