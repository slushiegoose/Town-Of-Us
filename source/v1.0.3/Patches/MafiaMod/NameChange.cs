using HarmonyLib;

namespace TownOfUs.MafiaMod
{
    [HarmonyPatch(typeof(HudManager))]
    public class NameChange
    {

        private static void UpdateMeeting(MeetingHud __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (!PlayerControl.LocalPlayer.Data.IsImpostor) return;
            if(Utils.Godfather == null || Utils.Mafioso == null || Utils.Janitor == null) return;
            foreach (var player in __instance.playerStates)
            {
                var name = player.NameText.Text;
                if(name == Utils.Godfather.name || name == Utils.Godfather.name + " (G)")
                {
                    player.NameText.Text = Utils.Godfather.name + " (G)";
                }
                if(name == Utils.Mafioso.name || name == Utils.Mafioso.name + " (M)")
                {
                    player.NameText.Text = Utils.Mafioso.name + " (M)";
                }
                if(name == Utils.Janitor.name || name == Utils.Janitor.name + " (J)")
                {
                    player.NameText.Text = Utils.Janitor.name + " (J)";
                }
            }
        }

        [HarmonyPatch(nameof(HudManager.Update))]
        public static void Postfix(HudManager __instance)
        {
            var flag = MeetingHud.Instance != null;
            if (flag)
            {
                UpdateMeeting(MeetingHud.Instance);
            }

            var flag2 = PlayerControl.AllPlayerControls.Count > 1;
            if (!flag2) return;
            var flag3 = Utils.Godfather != null && Utils.Mafioso != null && Utils.Janitor != null;
            if (!flag3) return;
            var flag4 = PlayerControl.LocalPlayer.Data.IsImpostor;
            if (!flag4) return;
            foreach(var player in PlayerControl.AllPlayerControls)
            {
                if(player.isGodfather())
                {
                    player.nameText.Text = Utils.Godfather.name + " (G)";
                }
                if(player.isMafioso())
                {
                    player.nameText.Text = Utils.Mafioso.name + " (M)";
                }
                if(player.isJanitor())
                {
                    player.nameText.Text = Utils.Janitor.name + " (J)";
                }
            }



        }
    }
}