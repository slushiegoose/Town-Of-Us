using HarmonyLib;

namespace TownOfUs.CamouflageMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class CamouflageUnCamouflage
    {
        public static bool CommsEnabled = false;
        public static bool CamouflagerEnabled = false;

        public static bool IsCamoed => CommsEnabled | CamouflagerEnabled;
        
        public static void Postfix(HudManager __instance)
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Camouflager))
            {
                
                var camouflager = (Roles.Camouflager) role;
                if (camouflager.Camouflaged)
                {
                    CamouflagerEnabled = true;
                    camouflager.Camouflage();
                } else if (camouflager.Enabled)
                {
                    CamouflagerEnabled = false;
                    camouflager.UnCamouflage();
                }
                
            }

            if (CustomGameOptions.ColourblindComms)
            {
                var commsActive = false;
                if (!ShipStatus.Instance) return;
                switch (ShipStatus.Instance.Type)
                {
                    case ShipStatus.MapType.Ship:
                    case ShipStatus.MapType.Pb:
                        var comms1 = ShipStatus.Instance.Systems[SystemTypes.Comms].Cast<HudOverrideSystemType>();
                        if (comms1.IsActive)
                        {
                            CommsEnabled = true;
                            commsActive = true;
                            Utils.Camouflage();
                            return;
                        }

                        break;
                    case ShipStatus.MapType.Hq:
                        var comms2 = ShipStatus.Instance.Systems[SystemTypes.Comms].Cast<HqHudSystemType>();
                        if (comms2.IsActive)
                        {
                            CommsEnabled = true;
                            commsActive = true;
                            Utils.Camouflage();
                            return;
                        }

                        break;

                }

                if (!commsActive && CommsEnabled)
                {
                    CommsEnabled = false;
                    Utils.UnCamouflage();
                }
                
            }
        }
    }
}