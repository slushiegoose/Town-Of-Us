using Hazel;
using InnerNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TownOfUs.CrewmateRoles.MedicMod;
using TownOfUs.Extensions;
using TownOfUs.Roles.Modifiers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfUs.Roles
{
    public class Glitch : Role//, IVisualAlteration
    {
        public static AssetBundle bundle = loadBundle();
        public static Sprite MimicSprite = bundle.LoadAsset<Sprite>("MimicSprite").DontUnload();
        public static Sprite HackSprite = bundle.LoadAsset<Sprite>("HackSprite").DontUnload();
        public static Sprite LockSprite = bundle.LoadAsset<Sprite>("Lock").DontUnload();
        public bool GlitchWins { get; set; }

        public Glitch(PlayerControl player) : base(player)
        {
            Name = "The Glitch";
            Color = Color.green;
            RoleType = RoleEnum.Glitch;
            ImpostorText = () => "foreach PlayerControl Glitch.MurderPlayer";
            TaskText = () => "Murder everyone:";
            Faction = Faction.Neutral;

            if (player.AmOwner)
            {
                AbilityManager.Add(new PlayerAbilityData
                {
                    Callback = KillCallback,
                    MaxTimer = CustomGameOptions.GlitchKillCooldown,
                    Range = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance],
                    TargetColor = Color,
                    Position = TOUConstants.KillButtonPosition
                });

                AbilityManager.Add(new PlayerAbilityData
                {
                    Callback = MimicCallback,
                    IsHighlighted = () => true,
                    MaxTimer = CustomGameOptions.MimicCooldown,
                    Icon = MimicSprite,
                    Position = TOUConstants.BottomLeftA
                });

                AbilityManager.Add(new PlayerAbilityData
                {
                    Callback = HackCallback,
                    MaxTimer = CustomGameOptions.HackCooldown,
                    Range = GameOptionsData.KillDistances[CustomGameOptions.GlitchHackDistance],
                    TargetColor = Color,
                    Icon = HackSprite,
                    Position = TOUConstants.BottomLeftB
                });
            }
        }

        public void KillCallback(PlayerControl target)
        {
            if (target.isShielded())
            {
                Utils.RpcBreakShield(target);
                return;
            }
            Utils.RpcMurderPlayer(Player, target);
        }

        public void HackCallback(PlayerControl target)
        {
            TownOfUs.LogMessage("Hack Callback");
            Utils.RpcSetHacked(target);
        }

        public void MimicCallback(PlayerControl _)
        {
            // TODO: make
        }

        public bool TryGetVisualAppearance(out VisualAppearance appearance)
        {
            appearance = Player.GetDefaultAppearance();
            return false;
        }

        public static AssetBundle loadBundle()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("TownOfUs.Resources.glitchbundle");
            var assets = stream.ReadFully();
            return AssetBundle.LoadFromMemory(assets);
        }

        internal override bool CheckEndCriteria(ShipStatus __instance)
        {
            if (Player.Data.IsDead || Player.Data.Disconnected) return true;

            if (PlayerControl.AllPlayerControls.ToArray().Count(x => !x.Data.IsDead && !x.Data.Disconnected) == 1)
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(
                    PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.GlitchWin,
                    SendOption.Reliable,
                    -1
                );
                writer.Write(Player.PlayerId);
                Wins();
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                Utils.EndGame();
                return false;
            }

            return false;
        }

        public void Wins()
        {
            GlitchWins = true;
        }

        public void Loses()
        {
            Player.Data.IsImpostor = true;
        }

        protected override void IntroPrefix(IntroCutscene._CoBegin_d__14 __instance)
        {
            var glitchTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
            glitchTeam.Add(PlayerControl.LocalPlayer);
            __instance.yourTeam = glitchTeam;
        }
    }
}
