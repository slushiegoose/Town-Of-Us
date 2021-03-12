using HarmonyLib;

namespace TownOfUs.LoversMod
{
    [HarmonyPatch(typeof(ChatController))]
    public static class AddChat
    {
        [HarmonyPatch(nameof(ChatController.AddChat))]
        public static bool Prefix()
        {
            var localPlayer = PlayerControl.LocalPlayer;
            if (localPlayer == null) return true;
            return MeetingHud.Instance != null || LobbyBehaviour.Instance != null || localPlayer.Data.IsDead || localPlayer.isLover();
        }

    }
}