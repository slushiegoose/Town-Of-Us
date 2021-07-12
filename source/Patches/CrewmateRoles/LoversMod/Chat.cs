using HarmonyLib;

namespace TownOfUs.CrewmateRoles.LoversMod
{
    public static class Chat
    {
        [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
        public static class AddChat
        {
            public static bool Prefix(ChatController __instance, [HarmonyArgument(0)] PlayerControl sourcePlayer)
            {
                if (__instance != HudManager.Instance.Chat) return true;
                var localPlayer = PlayerControl.LocalPlayer;
                if (localPlayer == null) return true;
                return MeetingHud.Instance != null || LobbyBehaviour.Instance != null || localPlayer.Data.IsDead ||
                       localPlayer.IsLover() || sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
            }
        }

        [HarmonyPatch]
        public static class EnableChat
        {
            public static void Enable()
            {
                if (PlayerControl.LocalPlayer.IsLover())
                    HudManager.Instance.Chat.SetVisible(true);
            }

            [HarmonyPostfix]
            [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
            public static void MeetingClose() => Enable();

            [HarmonyPostfix]
            [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.SpawnPlayer))]
            public static void SpawnPlayer() => Enable();

            [HarmonyPostfix]
            [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Revive))]
            public static void OnRevive() => Enable();
        }
    }
}
