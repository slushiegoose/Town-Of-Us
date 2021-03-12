using HarmonyLib;
using Hazel;
using Reactor;
using Reactor.Extensions;
using UnityEngine;
using DateTime = Il2CppSystem.DateTime; 

namespace TownOfUs.EngineerMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public class Arrows
    {

        public static ArrowBehaviour Arrow;
        public static bool ArrowCreated;
        private static Sprite Sprite => TownOfUs.EngineerArrow;
        
        public static bool ArrowsOpen
        {
            get
            {
                var use = PerformKill.SabotageTime;
                var now = DateTime.UtcNow;
                return use != null && (now - use).TotalSeconds < 10;
            }
        }

        public static void GenArrows()
        {
            ArrowCreated = true;
            PerformKill.UsedThisRound = true;
            PerformKill.SabotageTime = DateTime.Now;
            if (PlayerControl.LocalPlayer.Data.IsDead || !PlayerControl.LocalPlayer.Data.IsImpostor ||
                Utils.Engineer.Data.IsDead) return;
            var gameobj = new GameObject();
            Arrow = gameobj.AddComponent<ArrowBehaviour>();
            gameobj.transform.parent = PlayerControl.LocalPlayer.gameObject.transform;
            var renderer = gameobj.AddComponent<SpriteRenderer>();
            renderer.sprite = Sprite;
            Arrow.image = renderer;
            gameobj.layer = 5;
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage("CREATED ARROW");
            

        }

        public static void RemoveArrows()
        {
            ArrowCreated = false;
            if (Arrow == null) return;
            Arrow.gameObject.Destroy();
            Arrow = null;
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage("DELETED ARROW");
        }
        

        public static void Postfix(PlayerControl __instance)
        {
            if (Utils.Engineer == null) return;

            if (ArrowsOpen && !ArrowCreated && !Utils.Engineer.Data.IsDead && PlayerControl.LocalPlayer.isEngineer())
            {
                GenArrows();
                PluginSingleton<TownOfUs>.Instance.Log.LogMessage("Set");
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.SetArrows, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }

            if ((Utils.Engineer.Data.IsDead || !ArrowsOpen) && ArrowCreated && PlayerControl.LocalPlayer.isEngineer())
            { 
                RemoveArrows();
                PluginSingleton<TownOfUs>.Instance.Log.LogMessage("Die");
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.RemoveArrows, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                
            }
            
            
            if (!__instance.isEngineer()) return;
            if (Arrow != null)
            {
                Arrow.target = __instance.MyPhysics.transform.position;
            }
        }
        
        
        
    }
}