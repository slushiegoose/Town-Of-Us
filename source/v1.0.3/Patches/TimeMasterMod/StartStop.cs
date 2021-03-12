using System;
using UnityEngine;

namespace TownOfUs.TimeMasterMod
{
    public class StartStop
    {

        public static void StartRewind()
        {
            System.Console.WriteLine("START...");
            RecordRewind.rewinding = true;
            PlayerControl.LocalPlayer.moveable = false;
            HudManager.Instance.FullScreen.color = new Color(0f, 0.5f, 0.8f, 0.3f);
            HudManager.Instance.FullScreen.enabled = true;
            Methods.StartRewind = DateTime.UtcNow;

        }
        
        public static void StopRewind()
        {
            System.Console.WriteLine("STOP...");
            Methods.FinishRewind = DateTime.UtcNow;
            RecordRewind.rewinding = false;
            PlayerControl.LocalPlayer.moveable = true;
            HudManager.Instance.FullScreen.enabled = false;
        }
        
        
        
    }
}