using HarmonyLib;
using UnityEngine;

namespace TownOfUs.JesterMod
{
    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.Start))]
    public static class Outro
    {
        
        public static void Postfix(EndGameManager __instance)
        {
            if (!EndCriteria.JesterVotedOut) return;
            PoolablePlayer[] array = Object.FindObjectsOfType<PoolablePlayer>();
            array[0].NameText.Text = "[FFBFCCFF]" + array[0].NameText.Text;
            __instance.BackgroundBar.material.color = new Color(1f, 0.75f, 0.8f, 1f);
            var text = UnityEngine.Object.Instantiate(__instance.WinText);
            text.Text = "Jester wins";
            text.Color = new Color(1f, 0.75f, 0.8f, 1f);
            var pos = __instance.WinText.transform.localPosition;
            pos.y = 1.5f;
            text.transform.position = pos;
            text.scale = 1f;
        }
    }
}