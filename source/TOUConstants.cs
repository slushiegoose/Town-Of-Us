using UnityEngine;

namespace TownOfUs
{
    public enum AbilityPositions
    {
        KillButton,
        OverKillButton,
        BottomLeftA,
        BottomLeftB,
        BottomLeftC
    }

    public static class TOUConstants
    {
        public static Vector3 GetPosition(AbilityPositions position)
        {
            var hudManager = HudManager.Instance;
            var hudKill = hudManager.KillButton.transform.localPosition;
            var hudReport = hudManager.ReportButton.transform.localPosition;
            var hudUse = hudManager.UseButton.transform.localPosition;
            return position switch
            {
                AbilityPositions.KillButton => hudKill,
                AbilityPositions.OverKillButton => new Vector3(
                    hudKill.x,
                    hudReport.y,
                    hudKill.z
                ),
                AbilityPositions.BottomLeftA => new Vector3(
                    -hudUse.x,
                    hudKill.y,
                    hudKill.z
                ),
                AbilityPositions.BottomLeftB => new Vector3(
                    -hudUse.x,
                    hudReport.y,
                    hudKill.z
                ),
                AbilityPositions.BottomLeftC => new Vector3(
                    -hudKill.x,
                    hudKill.y,
                    -hudKill.z
                )
            };
        }
    }

    public enum MapTypes
    {
        Skeld,
        MiraHQ,
        Polus,
        Airship
    }
}
