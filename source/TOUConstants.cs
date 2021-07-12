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

        public static Vector3 BottomLeftA = new Vector3(-4.5859f, -2.3f, -9f);
        public static Vector3 BottomLeftB = new Vector3(
            BottomLeftA.x,
            -1f,
            BottomLeftB.z
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
