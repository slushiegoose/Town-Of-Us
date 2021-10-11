using Hazel;
using System.Linq;
using System.Reflection;
using Reactor;
using TownOfUs.CrewmateRoles.MedicMod;
using Reactor.Extensions;
using TownOfUs.Extensions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TownOfUs.ImpostorRoles.CamouflageMod;

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
        public PlainAbilityData MimicButton;
        public ChatController MimicList;

        public Glitch(PlayerControl player) : base(player)
        {
            RoleType = RoleEnum.Glitch;
            ImpostorText = () => "foreach PlayerControl Glitch.MurderPlayer";
            TaskText = () => "Murder everyone:";
            Faction = Faction.Neutral;
            CreateButtons();
        }

        public override void CreateButtons()
        {
            if (Player.AmOwner)
            {
                AbilityManager.Add(KillButton = new PlayerAbilityData
                {
                    Callback = KillCallback,
                    MaxTimer = CustomGameOptions.GlitchKillCooldown,
                    Range = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance],
                    TargetColor = Color,
                    Position = AbilityPositions.KillButton
                });

                AbilityManager.Add(MimicButton = new PlainAbilityData
                {
                    Callback = MimicCallback,
                    MaxTimer = CustomGameOptions.MimicCooldown,
                    Icon = MimicSprite,
                    Position = AbilityPositions.BottomLeftA,
                    OnDurationEnd = UnMimic,
                    HiddenOnCamo = true,
                    IsHighlighted = () => true
                });

                AbilityManager.Add(new PlayerAbilityData
                {
                    Callback = HackCallback,
                    MaxTimer = CustomGameOptions.HackCooldown,
                    Range = GameOptionsData.KillDistances[CustomGameOptions.GlitchHackDistance],
                    Icon = HackSprite,
                    TargetColor = Color,
                    Position = AbilityPositions.BottomLeftB
                });
            }
        }

        public void KillCallback(PlayerControl target)
        {
            if (target.IsShielded())
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
                (byte)CustomRPC.ResetAnim, SendOption.Reliable, -1);
            writer.Write(Player.PlayerId);
            writer.Write(MimicedAs.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            MimicedAs = null;
            MimicButton.MaxDuration = float.NaN;
            Utils.Unmorph(Player);
        }

        private void ChooseMimic(PlayerControl player)
        {
            var pool = MimicList.chatBubPool;
            foreach (var bubble in pool.activeChildren)
            {
                bubble.Destroy();
                bubble.gameObject.Destroy();
            }
            foreach (var bubble in pool.inactiveChildren)
            {
                bubble.Destroy();
                bubble.gameObject.Destroy();
            }
            pool.activeChildren.Clear();
            pool.inactiveChildren.Clear();

            MimicList.Destroy();
            MimicList.gameObject.Destroy();
            MimicList = null;

            AbilityManager.EnableButtons();

            if (player == null) return;

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

            var prefab = MimicList.chatBubPool.Prefab;

            void AddChat(PlayerControl player, string text, System.Action clickEvent = null)
            {
                var bubble = Object.Instantiate(prefab).Cast<ChatBubble>();
                bubble.transform.SetParent(MimicList.scroller.Inner);
                bubble.transform.localScale = Vector3.one;
                bubble.gameObject.SetActive(true);
                bubble.OwnerPool = MimicList.chatBubPool;
                MimicList.chatBubPool.activeChildren.Add(bubble);

                player.SetPlayerMaterialColors(bubble.Player.Body);
                bubble.SetCosmetics(player.Data);
                bubble.SetLeft();
                bubble.SetName(player.name, false, false, Color.white);
                bubble.SetText(text);
                bubble.AlignChildren();
                bubble.Xmark.gameObject.SetActive(false);
                bubble.votedMark.gameObject.SetActive(false);
                MimicList.AlignAllBubbles();

                if (clickEvent != null)
                {
                    bubble.gameObject.AddComponent<BoxCollider2D>().size = new Vector2(10f, 0.5f);
                    var button = bubble.gameObject.AddComponent<PassiveButton>();
                    AddHover(button, bubble.Background);

                    (button.OnClick = new Button.ButtonClickedEvent()).AddListener(clickEvent);
                }

                var isEmpty = string.IsNullOrEmpty(text);
                var isExit = !isEmpty && text.Equals("Click to exit");

                if (isEmpty || isExit)
                {
                    bubble.Player.gameObject.SetActive(false);
                    bubble.NameText.gameObject.SetActive(false);

                    if (isEmpty)
                        bubble.Background.gameObject.SetActive(false);
                }
            }

            var localPlayer = PlayerControl.LocalPlayer;

            var allPlayers = PlayerControl.AllPlayerControls.ToArray().ToList();
            var mimicText = $"Click to Mimic ({CustomGameOptions.MimicDuration}s)";
            foreach (var player in allPlayers)
            {
                if (player.AmOwner) continue;

                AddChat(player, mimicText, () => ChooseMimic(player));
            }

            // noop because weirdchamp
            AddChat(localPlayer, "", () => { });
            AddChat(localPlayer, "Click to exit", () => ChooseMimic(null));
            
            MimicButton.KillButton.SetCoolDown(
                MimicButton.Timer = 0f,
                MimicButton.MaxTimer
            );
        }

        public void MimicCallback()
        {
            AbilityManager.DisableButtons();
            InitalizeList();

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
            MimicButton.Timer = 0f;
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

        public override bool CheckEndCriteria(ShipStatus __instance)
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

        public override void IntroPrefix(IntroCutscene._CoBegin_d__14 __instance)
        {
            var glitchTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
            glitchTeam.Add(PlayerControl.LocalPlayer);
            __instance.yourTeam = glitchTeam;
        }
    }
}
