using Hazel;
using System.Linq;
using System.Reflection;
using TownOfUs.Extensions;
using UnityEngine;
using UnityEngine.UI;

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

        private void AddChat(ChatController chat, PlayerControl player)
        {
            var pool = chat.chatBubPool;
            if (pool.NotInUse == 0)
                pool.ReclaimOldest();

            var bubble = Object.Instantiate(pool.Prefab, chat.transform) as ChatBubble;
            var transform = bubble.transform;

            transform.SetParent(chat.scroller.Inner);
            transform.localScale = Vector3.one;

            bubble.SetLeft();
            player.SetPlayerMaterialColors(bubble.ChatFace);
            bubble.SetName(player.name, false, false, Color.white);
            bubble.SetText("Click to morph");

            var localPosition = bubble.Background.transform.localPosition;
            localPosition.y = bubble.NameText.transform.localPosition.y - bubble.Background.size.y / 2f + 0.05f;
            bubble.Background.transform.localPosition = localPosition;
            chat.AlignAllBubbles();

            var clickEvent = new Button.ButtonClickedEvent();

            clickEvent.AddListener((System.Action) (() => ChooseMimic(chat, player)));

            bubble.gameObject.AddComponent<PassiveButton>().OnClick = clickEvent;
        }

        private void ChooseMimic(ChatController chat, PlayerControl player)
        {
            chat.SetVisible(false);
            chat.Toggle();
            chat.gameObject.Destroy();

            TownOfUs.LogMessage($"Chosen Morph: {player.name}");
        }

        public void MimicCallback(PlayerControl _)
        {
            var mimicList = Object.Instantiate(HudManager.Instance.Chat, Camera.main.transform);
            mimicList.SetVisible(true);
            mimicList.Toggle();

            mimicList.TextBubble.gameObject.SetActive(mimicList.TextBubble.enabled = false);
            mimicList.TextArea.gameObject.SetActive(mimicList.TextArea.enabled = false);
            mimicList.BanButton.gameObject.SetActive(mimicList.BanButton.enabled = false);
            mimicList.CharCount.gameObject.SetActive(mimicList.CharCount.enabled = false);
            mimicList.BackgroundImage.gameObject.SetActive(mimicList.BackgroundImage.enabled = false);
            
            var child = mimicList.gameObject.transform.GetChild(0).gameObject;
            child.SetActive(child.GetComponent<SpriteRenderer>().enabled = false);

            foreach (var renderer in mimicList.Content.GetComponentsInChildren<SpriteRenderer>())
                if (renderer.name.Equals("SendButton") || renderer.name.Equals("QuickChatButton"))
                    renderer.gameObject.SetActive(renderer.enabled = false);

            var children = mimicList.chatBubPool.activeChildren;
            foreach (var renderer in children)
                renderer.gameObject.SetActive(renderer.enabled = false);

            children.Clear();

            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                if (player.AmOwner) continue;
                AddChat(mimicList, player);
            }
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
