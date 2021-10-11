using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.ImpostorRoles.MorphlingMod
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    public static class ResetSample
    {
        public static void Postfix()
        {
            var morphling = Role.GetRole<Morphling>();

            if (morphling == null) return;
            morphling.SampledPlayer = morphling.MorphedPlayer = null;
            var morphButton = morphling.MorphButton;

            if (morphButton == null) return;
            morphButton.HiddenOnCamo = false;
            morphButton.Icon = TownOfUs.SampleSprite;
            morphButton.UpdateSprite();
            morphButton.MaxDuration = float.NaN;
        }
    }
}
