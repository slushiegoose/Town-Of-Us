using System;
using UnityEngine;

namespace TownOfUs.TimeLordMod
{
    public class StartStop
    {
        public static Color oldColor;

        public static void StartRewind(Roles.TimeLord role)
        {
            //System.Console.WriteLine("START...");
            RecordRewind.rewinding = true;
            RecordRewind.whoIsRewinding = role;
            PlayerControl.LocalPlayer.moveable = false;
            oldColor = HudManager.Instance.FullScreen.color;
            HudManager.Instance.FullScreen.color = new Color(0f, 0.5f, 0.8f, 0.3f);
            HudManager.Instance.FullScreen.enabled = true;
            role.StartRewind = DateTime.UtcNow;

        }
        
        public static void StopRewind(Roles.TimeLord role)
        {
            //System.Console.WriteLine("STOP...");
            role.FinishRewind = DateTime.UtcNow;
            RecordRewind.rewinding = false;
            PlayerControl.LocalPlayer.moveable = true;
            HudManager.Instance.FullScreen.enabled = false;
            HudManager.Instance.FullScreen.color = oldColor;
        }
        
        
        
    }
}