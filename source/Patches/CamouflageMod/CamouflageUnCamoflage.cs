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
            CamouflagerEnabled = false;
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
                if (ShipStatus.Instance != null)
                {

                    switch (PlayerControl.GameOptions.MapId)
                    {
                        case 0:
                        case 2:
                        case 3:
                        case 4:
                            var comms1 = ShipStatus.Instance.Systems[SystemTypes.Comms].Cast<HudOverrideSystemType>();
                            if (comms1.IsActive)
                            {
                                CommsEnabled = true;
                                Utils.Camouflage();
                                return;
                            }

                            break;
                        case 1:
                            var comms2 = ShipStatus.Instance.Systems[SystemTypes.Comms].Cast<HqHudSystemType>();
                            if (comms2.IsActive)
                            {
                                CommsEnabled = true;
                                Utils.Camouflage();
                                return;
                            }

                            break;

                    }
                }

                if (CommsEnabled)
                {
                    CommsEnabled = false;
                    Utils.UnCamouflage();
                }
            }
        }
    }
}