using HarmonyLib;
using TownOfUs.Roles.Modifiers;
using Reactor.Extensions;

namespace TownOfUs.Modifiers
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    public static class ButtonBarryPatch
    {
        public static void Postfix()
        {
            var localPlayer = PlayerControl.LocalPlayer;
            var buttonBarry = Modifier.GetModifier<ButtonBarry>(localPlayer);
            var button = buttonBarry?.ButtonButton;
            if (button == null) return;

            if (localPlayer.RemainingEmergencies == 0 || buttonBarry.ButtonUsed)
            {
                AbilityManager.Buttons.Remove(button);
                buttonBarry.ButtonButton = null;
                button.KillButton.gameObject.Destroy();
            }
        }
    }
}
