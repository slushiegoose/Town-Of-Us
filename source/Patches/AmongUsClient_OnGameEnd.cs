using System.Linq;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using TownOfUs.Roles;

namespace TownOfUs
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
    public class AmongUsClient_OnGameEnd
    {
        public static void Postfix(AmongUsClient __instance, [HarmonyArgument(0)] GameOverReason reason,
            [HarmonyArgument(0)] bool showAd)
        {
            Utils.potentialWinners.Clear();
            foreach (var player in PlayerControl.AllPlayerControls)
                Utils.potentialWinners.Add(new WinningPlayerData(player.Data));
        }
    }

    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.Start))]
    public class EndGameManager_SetEverythingUp
    {
        public static void Prefix()
        {
            var jester = Role.AllRoles.FirstOrDefault(x => x.RoleType == RoleEnum.Jester && ((Jester) x).VotedOut);
            var executioner = Role.AllRoles.FirstOrDefault(x =>
                x.RoleType == RoleEnum.Executioner && ((Executioner) x).TargetVotedOut);
            if (Role.NobodyWins)
            {
                TempData.winners = new List<WinningPlayerData>();
                return;
            }

            if (jester != null)
            {
                var winners = Utils.potentialWinners.Where(x => x.Name == jester.PlayerName).ToList();
                TempData.winners = new List<WinningPlayerData>();
                foreach (var win in winners)
                {
                    win.IsDead = false;
                    TempData.winners.Add(win);
                }

                return;
            }

            if (executioner != null)
            {
                var winners = Utils.potentialWinners.Where(x => x.Name == executioner.PlayerName).ToList();
                TempData.winners = new List<WinningPlayerData>();
                foreach (var win in winners) TempData.winners.Add(win);
                return;
            }

            var lover = Role.AllRoles
                .Where(x => x.RoleType == RoleEnum.Lover || x.RoleType == RoleEnum.LoverImpostor)
                .FirstOrDefault(x => ((Lover) x).LoveCoupleWins);
            if (lover != null)
            {
                var lover1 = (Lover) lover;
                var lover2 = lover1.OtherLover;
                var winners = Utils.potentialWinners
                    .Where(x => x.Name == lover1.PlayerName || x.Name == lover2.PlayerName).ToList();
                TempData.winners = new List<WinningPlayerData>();
                foreach (var win in winners) TempData.winners.Add(win);
                return;
            }

            var glitch = Role.AllRoles.FirstOrDefault(x => x.RoleType == RoleEnum.Glitch && ((Glitch) x).GlitchWins);
            if (glitch != null)
            {
                var winners = Utils.potentialWinners.Where(x => x.Name == glitch.PlayerName).ToList();
                TempData.winners = new List<WinningPlayerData>();
                foreach (var win in winners) TempData.winners.Add(win);
                return;
            }

            var arsonist =
                Role.AllRoles.FirstOrDefault(x => x.RoleType == RoleEnum.Arsonist && ((Arsonist) x).ArsonistWins);
            if (arsonist != null)
            {
                var winners = Utils.potentialWinners.Where(x => x.Name == arsonist.PlayerName).ToList();
                TempData.winners = new List<WinningPlayerData>();
                foreach (var win in winners) TempData.winners.Add(win);
                return;
            }

            var phantom =
                Role.AllRoles.FirstOrDefault(x => x.RoleType == RoleEnum.Phantom && ((Phantom) x).CompletedTasks);
            if (phantom != null)
            {
                var winners = Utils.potentialWinners.Where(x => x.Name == phantom.PlayerName).ToList();
                TempData.winners = new List<WinningPlayerData>();
                foreach (var win in winners) TempData.winners.Add(win);
            }
        }
    }
}
