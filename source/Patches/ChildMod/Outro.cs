using System.Linq;
using HarmonyLib;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.ChildMod
{
    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.Start))]
    public static class Outro
    {
        public static void Postfix(EndGameManager __instance)
        {
            var role = Role.AllRoles.FirstOrDefault(x => x.RoleType == RoleEnum.Child && ((Child) x).Dead);
            //System.Console.WriteLine(role);
            if (role == null) return;
            var text = Object.Instantiate(__instance.WinText);
            text.Text = "The Child was killed";
            text.Color = Color.red;
            var pos = __instance.WinText.transform.localPosition;
            pos.y = 1.5f;
            text.transform.position = pos;
            text.scale = 1f;
        }
    }
}