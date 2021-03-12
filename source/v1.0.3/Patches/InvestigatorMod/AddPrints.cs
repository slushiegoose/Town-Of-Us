using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace TownOfUs.InvestigatorMod {
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static class AddPrints {
        private static float _time = 0.0f;
        private static float Interval => CustomGameOptions.FootprintInterval;
        private static bool Vent => CustomGameOptions.VentFootprintVisible;
        
        private const float PeriodInterval = 0.25f;

        public static bool GameStarted = false;

        public static Vector2 Position(PlayerControl player)
        {
            return player.GetTruePosition() + new Vector2(0, 0.366667f);
        }
        

        public static void Postfix(PlayerControl __instance)
        {
            if (!GameStarted || !PlayerControl.LocalPlayer.isInvestigator()) return;
            // New Footprint
            _time += Time.deltaTime;
            if (_time >= Interval) {
                _time -= Interval;

                if (PlayerControl.LocalPlayer.isInvestigator()) {
                    foreach (var player in PlayerControl.AllPlayerControls)
                    {
                        if (player == null || player.Data.IsDead ||
                            player.PlayerId == PlayerControl.LocalPlayer.PlayerId) continue;
                        var canPlace = !Footprint.AllPrints.Any(print =>
                            Vector3.Distance(print.Position, Position(player)) < 0.5f &&
                            print.Color.a > 0.5 &&
                            print.Player.PlayerId == player.PlayerId);

                        if (Vent && ShipStatus.Instance != null)
                        {
                            if (ShipStatus.Instance.AllVents.Any(vent =>
                                Vector2.Distance(vent.gameObject.transform.position, Position(player)) < 1f))
                            {
                                canPlace = false;
                            }
                        }

                        if (canPlace) new Footprint(player);
                    }
                }
            }

            // Update

            for (var i = 0; i < Footprint.AllPrints.Count; i++)
            {
                var footprint = Footprint.AllPrints[i];
                if (footprint.Update())
                {
                    i--;
                }
            }
        }
    }
}