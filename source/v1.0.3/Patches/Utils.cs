using System.Collections.Generic;
using HarmonyLib;
using TownOfUs.MafiaMod;

namespace TownOfUs
{
    [HarmonyPatch]
    public static class Utils
    {
        public static PlayerControl Jester;
        public static PlayerControl Mayor;
        public static PlayerControl Sheriff;
        public static PlayerControl Engineer;
        public static PlayerControl Swapper;
        public static PlayerControl Shifter;
        public static PlayerControl Investigator;
        public static PlayerControl TimeMaster;
        
        public static PlayerControl Godfather;
        public static PlayerControl Mafioso;
        public static PlayerControl Janitor;

        public static PlayerControl Lover1;
        public static PlayerControl Lover2;
        public static bool LoverImpostor;



        public static void Null()
        {
            Mayor = null;
            Jester = null;
            Sheriff = null;
            Lover1 = null;
            Lover2 = null;
            Janitor = null;
            Mafioso = null;
            Godfather = null;
            Engineer = null;
            Swapper = null;
            Shifter = null;
            Investigator = null;
            TimeMaster = null;
        }
        
        
        public static bool isJester(this PlayerControl player)
        {
            if (Jester == null) return false;
            return player.PlayerId == Jester.PlayerId;
        }
        public static bool isMayor(this PlayerControl player)
        {
            if (Mayor == null) return false;
            return player.PlayerId == Mayor.PlayerId;
        }
        
        public static bool isSheriff(this PlayerControl player)
        {
            if (Sheriff == null) return false;
            return player.PlayerId == Sheriff.PlayerId;
        }
        
        public static bool isEngineer(this PlayerControl player)
        {
            if (Engineer == null) return false;
            return player.PlayerId == Engineer.PlayerId;
        }

        public static bool isLover1(this PlayerControl player)
        {
            if (Lover1 == null) return false;
            return player.PlayerId == Lover1.PlayerId;
        }
        
        public static bool isLover2(this PlayerControl player)
        {
            if (Lover2 == null) return false;
            return player.PlayerId == Lover2.PlayerId;
        }
        
        
        public static bool isLover(this PlayerControl player)
        {
            return player.isLover1() | player.isLover2();
        }

        public static PlayerControl OtherLover(this PlayerControl player)
        {
            return player.isLover1() ? Lover2 : player.isLover2() ? Lover1 : null;
        }
        
        
        public static bool isGodfather(this PlayerControl player)
        {
            if (Godfather == null) return false;
            return player.PlayerId == Godfather.PlayerId;
        }
        
        public static bool isMafioso(this PlayerControl player)
        {
            if (Mafioso == null) return false;
            return player.PlayerId == Mafioso.PlayerId;
        }
        
        public static bool isJanitor(this PlayerControl player)
        {
            if (Janitor == null) return false;
            return player.PlayerId == Janitor.PlayerId;
        }
        
        public static bool isSwapper(this PlayerControl player)
        {
            if (Swapper == null) return false;
            return player.PlayerId == Swapper.PlayerId;
        }
        
        public static bool isShifter(this PlayerControl player)
        {
            if (Shifter == null) return false;
            return player.PlayerId == Shifter.PlayerId;
        }

        public static bool isInvestigator(this PlayerControl player)
        {
            if (Investigator == null) return false;
            return player.PlayerId == Investigator.PlayerId;
        }

        public static bool isTimeMaster(this PlayerControl player)
        {
            if (TimeMaster == null) return false;
            return player.PlayerId == TimeMaster.PlayerId;
        }

        public static bool IsCrewmate(this PlayerControl player)
        {
            return GetRole(player) == Roles.Crewmate;
        }

        public static Roles GetRole(PlayerControl player)
        {
            if (player == null) return Roles.None;
            if (player.Data == null) return Roles.None;
            if (player.isSheriff()) return Roles.Sheriff;
            if (player.isJester()) return Roles.Jester;
            if (player.isEngineer()) return Roles.Engineer;
            if (player.isMayor()) return Roles.Mayor;
            if (player.isLover1()) return Roles.Lover1;
            if (player.isLover2() & !LoverImpostor) return Roles.Lover2;
            if (player.isSwapper()) return Roles.Swapper;
            if (player.isInvestigator()) return Roles.Investigator;
            if (player.isTimeMaster()) return Roles.TimeMaster;
            return player.Data.IsImpostor ? Roles.Impostor : Roles.Crewmate;
        }
        
        public static PlayerControl PlayerById(byte id)
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player.PlayerId == id)
                {
                    return player;
                }
            }

            return null;
        }

        public static List<PlayerControl> getCrewmates(IEnumerable<GameData.PlayerInfo> infection)
        {
            var list = new List<PlayerControl>();
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                var isImpostor = false;
                foreach (var impostor in infection)
                {
                    if (player.PlayerId == impostor.Object.PlayerId)
                    {
                        isImpostor = true;
                    }
                }

                if (!isImpostor)
                {
                    list.Add(player);
                }
                
            }
            return list;
        }
        
        public static List<PlayerControl> getImpostors(IEnumerable<GameData.PlayerInfo> infection)
        {
            var list = new List<PlayerControl>();
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                var isImpostor = false;
                foreach (var impostor in infection)
                {
                    if (player.PlayerId == impostor.Object.PlayerId)
                    {
                        isImpostor = true;
                    }
                }

                if (isImpostor)
                {
                    list.Add(player);
                }
                
            }
            return list;
        }
        
        

    }
}