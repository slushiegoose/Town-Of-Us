using HarmonyLib;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.CrewmateRoles.MedicMod
{
    public enum ShieldOptions
    {
        Self = 0,
        Medic = 1,
        SelfAndMedic = 2,
        Everyone = 3
    }
    public enum NotificationOptions
    {
        Medic = 0,
        Shielded = 1,
        Everyone = 2,
        Nobody = 3
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public class ShowShield
    {
        public static Color ProtectedColor = Color.cyan;

        public static void Postfix(PlayerControl __instance)
        {
            var medic = Role.GetRole<Medic>();
            if (medic?.ShieldedPlayer != __instance) return;

            void SetOutline()
            {
                var material = __instance.myRend.material;

                material.SetColor("_VisorColor", ProtectedColor);
                material.SetFloat("_Outline", 1f);
                material.SetColor("_OutlineColor", ProtectedColor);
            }

            var showShielded = CustomGameOptions.ShowShielded;

            if (showShielded == ShieldOptions.Everyone)
                SetOutline();
            else
            {
                var selfAndMedic = showShielded == ShieldOptions.SelfAndMedic;

                if ((selfAndMedic || showShielded == ShieldOptions.Self) && __instance.AmOwner)
                    SetOutline();
                else if ((selfAndMedic || showShielded == ShieldOptions.Medic) && medic.Player.AmOwner)
                    SetOutline();
            }
        }
    }
}
