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
        public PlayerAbilityData KillButton;
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
                AbilityManager.Add(KillButton = new PlayerAbilityData
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
                if (!CustomGameOptions.ShieldBreaks)
                    KillButton.Timer = 0.1f;
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

            AbilityManager.EnableButtons();

            if (player == null) return;

            TownOfUs.LogMessage($"Chosen Morph: {player.name}");

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.SetMimic, SendOption.Reliable, -1);
            writer.Write(Player.PlayerId);
            writer.Write(player.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            MimicedAs = player;
            Utils.Morph(Player, player);

            MimicButton.DurationLeft = MimicButton.MaxDuration = CustomGameOptions.MimicDuration;
        }

        private void InitalizeList()
        {
            var original = HudManager.Instance.Chat;
            MimicList = Object.Instantiate(original, original.transform.parent);

            MimicList.SetVisible(true);
            MimicList.Toggle();

            var children = MimicList.chatBubPool.activeChildren;
            foreach (var renderer in children)
                renderer.gameObject.SetActive(false);

            children.Clear();

            void AddHover(PassiveButton button, SpriteRenderer background)
            {
                var hoverEvent = button.OnMouseOver = new UnityEvent();
                hoverEvent.AddListener((System.Action)(() => background.color = Color.green));

                var unHoverEvent = button.OnMouseOut = new UnityEvent();
                unHoverEvent.AddListener((System.Action)(() => background.color = Color.white));
            }

            Button.ButtonClickedEvent AddButton(ChatBubble bubble)
            {
                var gameObject = bubble.gameObject;

                gameObject.AddComponent<BoxCollider2D>().size = new Vector2(10f, 0.5f);
                var button = gameObject.AddComponent<PassiveButton>();

                AddHover(button, bubble.Background);

                return button.OnClick = new Button.ButtonClickedEvent();
            }

            var items = MimicList.scroller.transform.GetChild(0);
            ChatBubble NextBubble(PlayerControl player)
            {
                for (var i = 0;i < items.childCount;i++)
                {
                    var gameObject = items.GetChild(i).gameObject;
                    if (!gameObject.active) continue;
                    var button = gameObject.GetComponent<PassiveButton>();
                    if (button != null) continue;
                    var bubble = gameObject.GetComponent<ChatBubble>();
                    if (bubble.NameText.text == player.name)
                        return bubble;
                }
                // this should never return null
                return null;
            }

            // hacky wacky
            PlayerControl lastPlayer = null;
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                if (player.AmOwner) continue;
                lastPlayer = player;

                // hacky wacky
                var wasDead = player.Data.IsDead;
                player.Data.IsDead = false;
                MimicList.AddChat(player, $"Click to Mimic ({CustomGameOptions.MimicDuration}s)");
                player.Data.IsDead = wasDead;

                AddButton(NextBubble(player))
                    .AddListener((System.Action)(() => ChooseMimic(player)));
            }

            // hacky wacky
            MimicList.AddChat(lastPlayer, "");
            var emptyBubble = NextBubble(lastPlayer);
            emptyBubble.Background.gameObject.SetActive(false);
            emptyBubble.ChatFace.gameObject.SetActive(false);
            emptyBubble.NameText.gameObject.SetActive(false);
            AddButton(emptyBubble);

            MimicList.AddChat(lastPlayer, "Click to exit");
            var exitBubble = NextBubble(lastPlayer);
            exitBubble.ChatFace.gameObject.SetActive(false);
            exitBubble.NameText.gameObject.SetActive(false);

            AddButton(exitBubble)
                .AddListener((System.Action)(() => ChooseMimic(null)));
        }

        public void MimicCallback(PlayerControl _)
        {
            AbilityManager.DisableButtons();
            if (MimicList == null)
            {
                InitalizeList();
            }
            else
            {
                MimicList.SetVisible(true);
                MimicList.Toggle();
            }

            Player.NetTransform.Halt();

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
