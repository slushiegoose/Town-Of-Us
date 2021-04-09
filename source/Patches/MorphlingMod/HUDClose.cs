using System;
using System.Linq;
using HarmonyLib;

namespace TownOfUs.MorphlingMod
{
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new Type[] { typeof(UnityEngine.Object) })]
    public static class HUDClose
    {

        public static void Postfix(UnityEngine.Object obj)
        { 
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Morphling))
            {
                var role = Roles.Role.GetRole<Roles.Morphling>(PlayerControl.LocalPlayer);
                role.MorphButton.renderer.sprite = TownOfUs.SampleSprite;
                role.SampledPlayer = null;
                role.LastMorphed = DateTime.UtcNow;
                
            }

        }
    }
}