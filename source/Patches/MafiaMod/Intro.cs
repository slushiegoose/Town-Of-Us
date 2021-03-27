using HarmonyLib;

namespace TownOfUs.MafiaMod
{
    [HarmonyPatch(typeof(IntroCutscene.CoBegin__d), nameof(IntroCutscene.CoBegin__d.MoveNext))]
    public static class Intro
    {
        
        public static void Postfix(IntroCutscene.CoBegin__d __instance)
        {
            if (PlayerControl.LocalPlayer.isGodfather())
            {
                __instance.__this.ImpostorText.gameObject.SetActive(true);
                __instance.__this.Title.Text = "Godfather";
                __instance.__this.ImpostorText.Text = "Kill all crewmates";
            }
            else if (PlayerControl.LocalPlayer.isMafioso())
            {
                __instance.__this.ImpostorText.gameObject.SetActive(true);
                __instance.__this.Title.Text = "Mafioso";
                __instance.__this.ImpostorText.Text = "Work with the [FF0000FF]Mafia[] to kill the Crewmates";
            }
            else if (PlayerControl.LocalPlayer.isJanitor())
            {
                __instance.__this.ImpostorText.gameObject.SetActive(true);
                __instance.__this.Title.Text = "Janitor";
                __instance.__this.ImpostorText.Text = "Clean bodies to prevent Crewmates from discovering them.";
            }
        }
    }
}