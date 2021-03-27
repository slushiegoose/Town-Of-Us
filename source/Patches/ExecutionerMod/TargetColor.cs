using HarmonyLib;
using Hazel;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.ExecutionerMod
{

    public enum OnTargetDead
    {
        Crew,
        Jester,
    }
    
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class TargetColor
    {
        private static void UpdateMeeting(MeetingHud __instance, Roles.Executioner role)
        {
            foreach (var player in __instance.playerStates)
            {
                if (player.TargetPlayerId == role.target.PlayerId)
                {
                    player.NameText.Color = Color.black;
                }
            }
        }

        private static void Postfix(HudManager __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Executioner)) return;
            var role = Role.GetRole<Executioner>(PlayerControl.LocalPlayer);

            if (MeetingHud.Instance != null) UpdateMeeting(MeetingHud.Instance, role);

            role.target.nameText.Color = Color.black;

            if (PlayerControl.LocalPlayer.Data.IsDead) return;
            if (!role.target.Data.IsDead) return;
            if (role.TargetVotedOut) return;

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.ExecutionerToJester, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            
            ExeToJes(PlayerControl.LocalPlayer);
            
            
        }

        public static void ExeToJes(PlayerControl player)
        {
            player.myTasks.RemoveAt(0);
            Roles.Role.RoleDictionary.Remove(player.PlayerId);


            if (CustomGameOptions.OnTargetDead == OnTargetDead.Jester)
            {
                var jester = new Roles.Jester(player);
                var task = new GameObject("JesterTask").AddComponent<ImportantTextTask>();
                task.transform.SetParent(player.transform, false);
                task.Text =
                    $"{jester.ColorString}Role: {jester.Name}\nYour target was killed. Now you get voted out!\nFake Tasks:[]";
                player.myTasks.Insert(0, task);
            }
        }

    }
}