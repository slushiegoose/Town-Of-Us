using System.Linq;
using HarmonyLib;
using Hazel;
using DateTime = Il2CppSystem.DateTime;

namespace TownOfUs.EngineerMod
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public class PerformKill
    {
        public static DateTime SabotageTime;
        public static bool UsedThisRound;

        public static bool Prefix(KillButtonManager __instance)
        {
            var flag = PlayerControl.LocalPlayer.isEngineer();
            if (!flag) return true;
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            if (UsedThisRound) return false;
            var system = ShipStatus.Instance.Systems[SystemTypes.Sabotage].Cast<SabotageSystemType>();
            var specials = system.Field_2.ToArray();
            var dummyActive = system.Field_4.BCKOBJLJEFE;
            var sabActive = specials.Any(s => s.Property_0);
            if (!sabActive | dummyActive) return false;
            UsedThisRound = true;
            SabotageTime = DateTime.Now;

            switch (ShipStatus.Instance.Type)
            {
                case ShipStatus.MapType.Ship:
                    var comms1 = ShipStatus.Instance.Systems[SystemTypes.Comms].Cast<HudOverrideSystemType>();
                    if (comms1.Property_0) return FixComms();
                    var reactor1 = ShipStatus.Instance.Systems[SystemTypes.Reactor].Cast<KJKDNMBDHKJ>();
                    if (reactor1.Property_1) return FixReactor(SystemTypes.Reactor);
                    var oxygen1 = ShipStatus.Instance.Systems[SystemTypes.LifeSupp].Cast<LifeSuppSystemType>();
                    if (oxygen1.Property_1) return FixOxygen();
                    var lights1 = ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
                    if (lights1.Property_1) return FixLights(lights1);
                    
                    break;
                case ShipStatus.MapType.Hq:
                    var comms2 = ShipStatus.Instance.Systems[SystemTypes.Comms].Cast<ReactorSystemType>();
                    if (comms2.Property_0) return FixMiraComms();
                    var reactor2 = ShipStatus.Instance.Systems[SystemTypes.Reactor].Cast<KJKDNMBDHKJ>();
                    if (reactor2.Property_1) return FixReactor(SystemTypes.Reactor);
                    var oxygen2 = ShipStatus.Instance.Systems[SystemTypes.LifeSupp].Cast<LifeSuppSystemType>();
                    if (oxygen2.Property_1) return FixOxygen();
                    var lights2 = ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
                    if (lights2.Property_1) return FixLights(lights2);
                    break;
                    
                case ShipStatus.MapType.Pb:
                    var comms3 = ShipStatus.Instance.Systems[SystemTypes.Comms].Cast<HudOverrideSystemType>();
                    if (comms3.Property_0) return FixComms();
                    var seismic = ShipStatus.Instance.Systems[SystemTypes.Laboratory].Cast<KJKDNMBDHKJ>();
                    if (seismic.Property_1) return FixReactor(SystemTypes.Laboratory);
                    var lights3 = ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
                    if (lights3.Property_1) return FixLights(lights3);
                    break;

            }

            return false;
        }

        private static bool FixComms()
        {
            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 0);
            return false;
        }

        private static bool FixMiraComms()
        {
            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 16 | 0);
            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 16 | 1);
            return false;
        }
        
        private static bool FixReactor(SystemTypes system)
        {
            ShipStatus.Instance.RpcRepairSystem(system, 16);
            return false;
        }
        
        private static bool FixOxygen()
        {
            ShipStatus.Instance.RpcRepairSystem(SystemTypes.LifeSupp, 16);
            return false;
        }

        private static bool FixLights(SwitchSystem lights)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.FixLights, SendOption.Reliable, -1);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            lights.ActualSwitches = lights.ExpectedSwitches;
            
            return false;
        }
        




    }
}