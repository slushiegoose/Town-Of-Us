using System.Linq;
using HarmonyLib;
using Hazel;
using DateTime = Il2CppSystem.DateTime;

namespace TownOfUs.EngineerMod
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public class PerformKill
    {
        public static bool Prefix(KillButtonManager __instance)
        {
            var flag = PlayerControl.LocalPlayer.Is(RoleEnum.Engineer);
            if (!flag) return true;
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            if (!__instance.enabled) return false;
            var role = Roles.Role.GetRole<Roles.Engineer>(PlayerControl.LocalPlayer);
            if (role.UsedThisRound) return false;
            var system = ShipStatus.Instance.Systems[SystemTypes.Sabotage].Cast<SabotageSystemType>();
            var specials = system.specials.ToArray();
            var dummyActive = system.dummy.IsActive;
            var sabActive = specials.Any(s => s.IsActive);
            if (!sabActive | dummyActive) return false;
            role.UsedThisRound = true;

            switch (ShipStatus.Instance.Type)
            {
                case ShipStatus.MapType.Ship:
                    var comms1 = ShipStatus.Instance.Systems[SystemTypes.Comms].Cast<HudOverrideSystemType>();
                    if (comms1.IsActive) return FixComms();
                    var reactor1 = ShipStatus.Instance.Systems[SystemTypes.Reactor].Cast<ReactorSystemType>();
                    if (reactor1.IsActive) return FixReactor(SystemTypes.Reactor);
                    var oxygen1 = ShipStatus.Instance.Systems[SystemTypes.LifeSupp].Cast<LifeSuppSystemType>();
                    if (oxygen1.IsActive) return FixOxygen();
                    var lights1 = ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
                    if (lights1.IsActive) return FixLights(lights1);
                    
                    break;
                case ShipStatus.MapType.Hq:
                    var comms2 = ShipStatus.Instance.Systems[SystemTypes.Comms].Cast<HqHudSystemType>();
                    if (comms2.IsActive) return FixMiraComms();
                    var reactor2 = ShipStatus.Instance.Systems[SystemTypes.Reactor].Cast<ReactorSystemType>();
                    if (reactor2.IsActive) return FixReactor(SystemTypes.Reactor);
                    var oxygen2 = ShipStatus.Instance.Systems[SystemTypes.LifeSupp].Cast<LifeSuppSystemType>();
                    if (oxygen2.IsActive) return FixOxygen();
                    var lights2 = ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
                    if (lights2.IsActive) return FixLights(lights2);
                    break;
                    
                case ShipStatus.MapType.Pb:
                    var comms3 = ShipStatus.Instance.Systems[SystemTypes.Comms].Cast<HudOverrideSystemType>();
                    if (comms3.IsActive) return FixComms();
                    var seismic = ShipStatus.Instance.Systems[SystemTypes.Laboratory].Cast<ReactorSystemType>();
                    if (seismic.IsActive) return FixReactor(SystemTypes.Laboratory);
                    var lights3 = ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
                    if (lights3.IsActive) return FixLights(lights3);
                    break;

            }

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.EngineerFix, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.NetId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            
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