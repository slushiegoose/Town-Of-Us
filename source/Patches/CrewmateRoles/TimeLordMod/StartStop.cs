using System;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.CrewmateRoles.TimeLordMod
{
    public class StartStop
    {
        public static Color OldColor;

        public static void StartRewind()
        {
            RecordRewind.Rewinding = true;
            PlayerControl.LocalPlayer.moveable = false;
            OldColor = HudManager.Instance.FullScreen.color;
            HudManager.Instance.FullScreen.color = new Color(0f, 0.5f, 0.8f, 0.3f);
            HudManager.Instance.FullScreen.enabled = true;

            RecordRewind.TimeLeft = RecordRewind.RecordTime;
        }

        public static void StopRewind()
        {
            RecordRewind.Rewinding = false;
            PlayerControl.LocalPlayer.moveable = true;
            HudManager.Instance.FullScreen.enabled = false;
            HudManager.Instance.FullScreen.color = OldColor;

            RecordRewind.TimeLeft = float.MinValue;
        }
    }
}
