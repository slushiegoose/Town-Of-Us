using System.Linq;
using HarmonyLib;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.JesterMod
{
    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.Start))]
    public static class Outro
    {
        
        public static void Postfix(EndGameManager __instance)
        {

            
            var role = Role.AllRoles.FirstOrDefault(x => x.RoleType == RoleEnum.Jester && ((Jester) x).VotedOut);
            if (role == null) return;
            PoolablePlayer[] array = Object.FindObjectsOfType<PoolablePlayer>();
            array[0].NameText.Text = role.ColorString + array[0].NameText.Text;
            __instance.BackgroundBar.material.color = role.Color;
            var text = Object.Instantiate(__instance.WinText);
            text.Text = "Jester wins";
            text.Color = role.Color;
            var pos = __instance.WinText.transform.localPosition;
            pos.y = 1.5f;
            text.transform.position = pos;
            text.scale = 1f;
        }
    }
}