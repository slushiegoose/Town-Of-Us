using HarmonyLib;
using Hazel;

namespace TownOfUs.LoversMod
{
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CheckEndCriteria))]
    public static class EndCriteria
    {
        public static bool LoveCoupleWins;
        public static bool NobodyWins;
        
        [HarmonyPriority(Priority.Last)]
        public static bool Prefix(ShipStatus __instance)
        {
            if (JesterMod.EndCriteria.JesterVotedOut) return false;
            if (Utils.Lover1 != null && Utils.Lover2 != null)
            {
                if (__instance.Systems.ContainsKey(SystemTypes.LifeSupp))
                {
                    var lifeSuppSystemType = __instance.Systems[SystemTypes.LifeSupp].Cast<LifeSuppSystemType>();
                    if (lifeSuppSystemType.Countdown < 0f)
                    {
                        return true;
                    }
                }
                
                if (__instance.Systems.ContainsKey(SystemTypes.Laboratory))
                {
                    var reactorSystemType = __instance.Systems[SystemTypes.Laboratory].Cast<KJKDNMBDHKJ>();
                    if (reactorSystemType.Field_10 < 0f)
                    {
                        return true;
                    }
                }
                if (__instance.Systems.ContainsKey(SystemTypes.Reactor))
                {
                    var reactorSystemType = __instance.Systems[SystemTypes.Reactor].Cast<KJKDNMBDHKJ>();
                    if (reactorSystemType.Field_10 < 0f)
                    {
                        return true;
                    }
                }
                if (Methods.FourPeopleLeft())
                {
                    return false;
                }
            }

            if (Methods.CheckLoversWin())
            {
                var messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.LoveWin, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
                Methods.LoversWin();
                ShipStatus.RpcEndGame((GameOverReason)2, false);
                return false;
            }
            
            if (Methods.CheckNoImpsNoCrews())
            {
                var messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.NobodyWins, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
                Methods.NobodyWins();
                ShipStatus.RpcEndGame((GameOverReason)2, false);
                return false;
            }
            return true;
        }
    }
}