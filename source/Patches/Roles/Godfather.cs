using System.Collections.Generic;
using Hazel;

namespace TownOfUs.Roles
{
    public class Godfather : Role
    {
        public Janitor Janitor {get; set;} 
        public Mafioso Mafioso {get; set;} 


        protected internal override string NameText()
        {
            if (CamouflageMod.CamouflageUnCamouflage.IsCamoed && !MeetingHud.Instance)
            {
                return "";
            }
            return Player.name + " (G)";
        }

        protected override bool Criteria()
        {
            var localPlayerRole = GetRole(PlayerControl.LocalPlayer);

            return localPlayerRole == this || localPlayerRole == Janitor || !CustomGameOptions.TwoMafia && localPlayerRole == Mafioso;

        }

        public static void Gen(List<PlayerControl> impostors)
        {
            if (impostors.Count < 2) return;
            if (impostors.Count < 3 && !CustomGameOptions.TwoMafia) return;
            int rand;
            rand = HashRandom.Method_1(impostors.Count);
            var godfather = impostors[rand];
            impostors.Remove(godfather);
            rand = HashRandom.Method_1(impostors.Count);
            var janitor = impostors[rand];
            impostors.Remove(janitor);
            
            
            var g = new Godfather(godfather);
            var j = new Janitor(janitor);
            
            g.Janitor = j;
            j.Godfather = g;
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.SetMafia, SendOption.Reliable, -1);
            writer.Write(godfather.PlayerId);
            writer.Write(janitor.PlayerId);
            if (!CustomGameOptions.TwoMafia)
            {
                rand = HashRandom.Method_1(impostors.Count);
                var mafioso = impostors[rand];
                impostors.Remove(mafioso);
                var m = new Mafioso(mafioso);
                g.Mafioso = m;
                j.Mafioso = m;
                m.Godfather = g;
                m.Janitor = j;
                writer.Write(mafioso.PlayerId);
            }
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
        
        public Godfather(PlayerControl player) : base(player)
        {
            Name = "Godfather";
            ImpostorText = () => "Kill all crewmates";
            TaskText = () => "Kill all the crewmates.";
            Color = Palette.ImpostorRed;
            RoleType = RoleEnum.Godfather;
            Faction = Faction.Impostors;
        }
    }
}