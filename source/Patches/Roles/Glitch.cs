using Hazel;
using System.Linq;
using System.Reflection;
using TownOfUs.Extensions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace TownOfUs.Roles
{
    public class Glitch : Role, IVisualAlteration
    {
        public static AssetBundle bundle = loadBundle();
        public static Sprite MimicSprite = bundle.LoadAsset<Sprite>("MimicSprite").DontUnload();
        public static Sprite HackSprite = bundle.LoadAsset<Sprite>("HackSprite").DontUnload();
        public static Sprite LockSprite = bundle.LoadAsset<Sprite>("Lock").DontUnload();
        public bool GlitchWins { get; set; }
        public PlayerControl MimicedAs = null;
        public PlayerAbilityData MimicButton;
        public ChatController MimicList;

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

                AbilityManager.Add(MimicButton = new PlayerAbilityData
                {
                    Callback = MimicCallback,
                    IsHighlighted = () => true,
                    MaxTimer = CustomGameOptions.MimicCooldown,
                    Icon = MimicSprite,
                    Position = TOUConstants.BottomLeftA,
                    OnDurationEnd = UnMimic,
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
            Utils.RpcSetHacked(target);
        }

        private void UnMimic()
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.RpcResetAnim, SendOption.Reliable, -1);
            writer.Write(Player.PlayerId);
            writer.Write(MimicedAs.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            MimicedAs = null;
            MimicButton.MaxDuration = float.NaN;
            Utils.Unmorph(Player);
        }

        private void ChooseMimic(PlayerControl player)
        {
            MimicList.Toggle();

            TownOfUs.LogMessage($"Chosen Morph: {player.name}");
            AbilityManager.EnableButtons();

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.SetMimic, SendOption.Reliable, -1);
            writer.Write(Player.PlayerId);
            writer.Write(player.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            MimicedAs = player;
            Utils.Morph(Player, player);

            MimicButton.DurationLeft = MimicButton.MaxDuration = CustomGameOptions.MimicDuration;
        }

        public void MimicCallback(PlayerControl _)
        {
            AbilityManager.DisableButtons();
            if (MimicList == null)
            {
                var original = HudManager.Instance.Chat;
                MimicList = Object.Instantiate(original, original.transform.parent);
            }
            MimicList.SetVisible(true);
            MimicList.Toggle();

            MimicList.TextBubble.gameObject.SetActive(false);
            MimicList.TextArea.gameObject.SetActive(false);
            MimicList.BanButton.gameObject.SetActive(false);
            MimicList.CharCount.gameObject.SetActive(false);
            MimicList.BackgroundImage.gameObject.GetComponent<SpriteRenderer>().enabled = false;

            var child = MimicList.gameObject.transform.GetChild(0).gameObject;
            child.SetActive(false);

            foreach (var renderer in MimicList.Content.GetComponentsInChildren<SpriteRenderer>())
                if (renderer.name.Equals("SendButton") || renderer.name.Equals("QuickChatButton"))
                    renderer.gameObject.SetActive(false);

            var children = MimicList.chatBubPool.activeChildren;
            foreach (var renderer in children)
                renderer.gameObject.SetActive(false);

            children.Clear();

            var idx = 0;
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                if (player.AmOwner) continue;
                MimicList.AddChat(player, $"Click to Mimic ({CustomGameOptions.MimicDuration}s)");

                var chatBubble = MimicList.chatBubPool.activeChildren[idx++].Cast<ChatBubble>();
                var gameObject = chatBubble.gameObject;
                var background = chatBubble.Background;
                var collider = gameObject.AddComponent<BoxCollider2D>();
                var button = gameObject.AddComponent<PassiveButton>();

                collider.size = new Vector2(10f, 0.5f);

                var clickEvent = button.OnClick = new Button.ButtonClickedEvent();
                clickEvent.AddListener((System.Action)(() => ChooseMimic(player)));

                var hoverEvent = button.OnMouseOver = new UnityEvent();
                hoverEvent.AddListener((System.Action)(() => background.color = Color.green));

                var unHoverEvent = button.OnMouseOut = new UnityEvent();
                unHoverEvent.AddListener((System.Action)(() => background.color = Color.white));
            }
        }

        public bool TryGetModifiedAppearance(out VisualAppearance appearance)
        {
            appearance = (MimicedAs ?? Player).GetDefaultAppearance();
            return MimicedAs != null;
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
