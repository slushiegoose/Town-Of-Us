using HarmonyLib;
using Hazel;
using TownOfUs.Roles.Modifiers;

namespace TownOfUs.Modifiers.ButtonBarryMod
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public class PerformKill
    {
        public static bool Prefix(KillButtonManager __instance)
        {
            if (!PlayerControl.LocalPlayer.Is(ModifierEnum.ButtonBarry)) return true;
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Swapper)) return true;
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Glitch)) return true;

            var role = Modifier.GetModifier<ButtonBarry>(PlayerControl.LocalPlayer);
            if (__instance != role.ButtonButton) return true;
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            if (PlayerControl.LocalPlayer.Data.IsDead) return false;
            if (role.ButtonUsed) return false;
            if (PlayerControl.LocalPlayer.RemainingEmergencies <= 0) return false;
            if (!__instance.enabled) return false;

            System.Console.WriteLine("Reached here!");

            role.ButtonUsed = true;
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.BarryButton, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            if (AmongUsClient.Instance.AmHost)
            {
                MeetingRoomManager.Instance.reporter = PlayerControl.LocalPlayer;
                MeetingRoomManager.Instance.target = null;
                AmongUsClient.Instance.DisconnectHandlers.AddUnique(
                    MeetingRoomManager.Instance.Cast<IDisconnectHandler>());
                if (ShipStatus.Instance.CheckTaskCompletion()) return false;
                DestroyableSingleton<HudManager>.Instance.OpenMeetingRoom(PlayerControl.LocalPlayer);
                PlayerControl.LocalPlayer.RpcStartMeeting(null);
            }

            return false;
        }
    }
}