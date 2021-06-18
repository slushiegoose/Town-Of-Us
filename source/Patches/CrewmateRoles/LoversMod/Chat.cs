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
                       localPlayer.isLover() || sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
            }
        }

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static class EnableChat
        {
            public static void Postfix(HudManager __instance)
            {
                if (PlayerControl.LocalPlayer.isLover() & !__instance.Chat.isActiveAndEnabled)
                    __instance.Chat.SetVisible(true);
            }
        }
    }
}