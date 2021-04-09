using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace TownOfUs.MedicMod
{
    public enum ShieldOptions
    {
        Self = 0,
        Medic = 1,
        SelfAndMedic = 2,
        Everyone = 3,
    }

    public enum NotificationOptions
    {
        Medic = 0,
        Shielded = 1,
        Everyone = 2,
        Nobody = 3,
    }
    
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class ShowShield
    {
        public static Color ProtectedColor = Color.cyan;
        
        public static void Postfix(HudManager __instance)
        {
            
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Medic))
            {
                var medic = (Roles.Medic) role;

                var exPlayer = medic.exShielded;
                if (exPlayer != null)
                {
                    System.Console.WriteLine(exPlayer.name + " is ex-Shielded and unvisored");
                    exPlayer.myRend.material.SetColor("_VisorColor", Palette.VisorColor);
                    exPlayer.myRend.material.SetFloat("_Outline", 0f);
                    medic.exShielded = null;
                    continue;
                }
                
                var player = medic.ShieldedPlayer;
                if (player == null) continue;

                if (player.Data.IsDead || medic.Player.Data.IsDead)
                {
                    StopKill.BreakShield(medic.Player.PlayerId, player.PlayerId, true);
                    continue;
                }


                var showShielded = CustomGameOptions.ShowShielded;
                if (showShielded ==  ShieldOptions.Everyone)
                {
                    player.myRend.material.SetColor("_VisorColor", ProtectedColor);
                    player.myRend.material.SetFloat("_Outline", 1f);
                    player.myRend.material.SetColor("_OutlineColor", ProtectedColor);
                }
                else if (PlayerControl.LocalPlayer.PlayerId == player.PlayerId && (showShielded ==  ShieldOptions.Self || showShielded == ShieldOptions.SelfAndMedic))
                {
                    //System.Console.WriteLine("Setting " + PlayerControl.LocalPlayer.name + "'s shield");
                    player.myRend.material.SetColor("_VisorColor", ProtectedColor);
                    player.myRend.material.SetFloat("_Outline", 1f);
                    player.myRend.material.SetColor("_OutlineColor", ProtectedColor);
                }
                else if (PlayerControl.LocalPlayer.Is(RoleEnum.Medic) &&
                         (showShielded == ShieldOptions.Medic || showShielded == ShieldOptions.SelfAndMedic))
                {
                    player.myRend.material.SetColor("_VisorColor", ProtectedColor);
                    player.myRend.material.SetFloat("_Outline", 1f);
                    player.myRend.material.SetColor("_OutlineColor", ProtectedColor);
                }
            }
        }
    }
}