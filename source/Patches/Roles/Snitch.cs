using System;
using System.Collections.Generic;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Snitch : Role
    {

        public int TasksLeft = Int32.MaxValue;
        public bool OneTaskLeft => TasksLeft <= 1;
        public bool TasksDone => TasksLeft <= 0;

        public List<PlayerControl> SnitchTargets = new List<PlayerControl>();

        public List<ArrowBehaviour> SnitchArrows = new List<ArrowBehaviour>();

        public List<ArrowBehaviour> ImpArrows = new List<ArrowBehaviour>();


        protected override bool Criteria()
        {
            return (OneTaskLeft && PlayerControl.LocalPlayer.Data.IsImpostor) ||
                   base.Criteria();
        }

        protected override string NameText(PlayerVoteArea player = null)
        {
            if (CamouflageMod.CamouflageUnCamouflage.IsCamoed && player == null)
            {
                return "";
            }
            if (PlayerControl.LocalPlayer.Data.IsDead) return base.NameText(player);
            if (OneTaskLeft || !Hidden) return base.NameText(player);
            Player.nameText.color = Color.white;
            if (player != null) player.NameText.color = Color.white;
            if (player != null && (MeetingHud.Instance.state == MeetingHud.VoteStates.Proceeding ||
                                   MeetingHud.Instance.state == MeetingHud.VoteStates.Results)) return Player.name;
            if (!CustomGameOptions.RoleUnderName && player == null) return Player.name;
            Player.nameText.transform.localPosition = new Vector3(
                0f,
                (Player.Data.HatId == 0U) ? 1.05f :
                CustomHats.HatCreation.TallIds.Contains(Player.Data.HatId) ? 1.6f : 1.4f,
                -0.5f
            );
            return Player.name + "\n" + "Crewmate";
        }

        public Snitch(PlayerControl player) : base(player)
        {
            Name = "Snitch";
            ImpostorText = () => "Complete all your tasks to discover the Impostors";
            TaskText = () => (TasksDone ? "Find the arrows pointing to the Impostors!" : "Complete all your tasks to discover the Impostors!");
            Color = new Color(0.83f, 0.69f, 0.22f, 1f);
            Hidden = !CustomGameOptions.SnitchOnLaunch;
            RoleType = RoleEnum.Snitch;
        }

    }
}