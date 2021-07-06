using UnityEngine;

namespace TownOfUs
{
    public static class TOUConstants
    {
        public static Vector3 KillButtonPosition = new Vector3(3.2359f, -2.3f, -9);
        public static Vector3 OverKillbutton = new Vector3(
            KillButtonPosition.x,
            -1f,
            KillButtonPosition.z
        );
    }

    public enum MapTypes
    {
        Skeld,
        MiraHQ,
        Polus,
        Airship
    }
}
