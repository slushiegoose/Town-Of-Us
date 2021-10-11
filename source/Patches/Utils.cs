using HarmonyLib;
using Hazel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Reactor.Extensions;
using TownOfUs.CrewmateRoles.MedicMod;
using TownOfUs.Extensions;
using TownOfUs.ImpostorRoles.CamouflageMod;
using TownOfUs.NeutralRoles.GlitchMod;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;
using UnhollowerBaseLib;
using UnityEngine;
using Object = UnityEngine.Object;
using Reactor;

namespace TownOfUs
{
    [HarmonyPatch]
    public static class Utils
    {
        internal static bool ShowDeadBodies = false;

        public static Dictionary<PlayerControl, Color> oldColors = new Dictionary<PlayerControl, Color>();

        public static List<WinningPlayerData> potentialWinners = new List<WinningPlayerData>();

        public static string ColorText(Color color, string text) =>
            $"<color=#{color.ToHtmlStringRGBA()}>{text}</color>";
        public static void RpcBreakShield(PlayerControl player)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.AttemptSound, SendOption.Reliable, -1);
            writer.Write(player.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            StopKill.BreakShield(
                Role.GetRole<Medic>(),
                player,
                CustomGameOptions.ShieldBreaks
            );
        }

        public static void SetSkin(PlayerControl Player, uint skin)
        {
            Player.MyPhysics.SetSkin(skin);
        }

        public static void Morph(PlayerControl player, PlayerControl morphedAs)
        {
            if (CamouflageUnCamouflage.IsCamoed) return;

            Role.NamePatch.SetNameText(player.nameText, morphedAs);

            var targetAppearance = morphedAs.GetDefaultAppearance();

            PlayerControl.SetPlayerMaterialColors(targetAppearance.ColorId, player.myRend);
            player.HatRenderer.SetHat(targetAppearance.HatId, targetAppearance.ColorId);
            player.nameText.transform.localPosition = new Vector3(
                0f,
                player.Data.HatId == 0U ? 1.5f : 2.0f,
                -0.5f
            );

            if (player.MyPhysics.Skin.skin.ProdId != DestroyableSingleton<HatManager>.Instance
                .AllSkins.ToArray()[(int)targetAppearance.SkinId].ProdId)
                SetSkin(player, targetAppearance.SkinId);

            if (player.CurrentPet == null || player.CurrentPet.ProdId !=
                DestroyableSingleton<HatManager>.Instance.AllPets.ToArray()[(int)targetAppearance.PetId].ProdId)
            {
                if (player.CurrentPet != null) Object.Destroy(player.CurrentPet.gameObject);

                player.CurrentPet =
                    Object.Instantiate(
                        DestroyableSingleton<HatManager>.Instance.AllPets.ToArray()[(int)targetAppearance.PetId]);
                player.CurrentPet.transform.position = player.transform.position;
                player.CurrentPet.Source = player;
                player.CurrentPet.Visible = player.Visible;
            }

            PlayerControl.SetPlayerMaterialColors(targetAppearance.ColorId, player.CurrentPet.rend);
        }

        public static void Unmorph(PlayerControl player)
        {
            var appearance = player.GetDefaultAppearance();
            Role.NamePatch.UpdateSingle(player);
            PlayerControl.SetPlayerMaterialColors(appearance.ColorId, player.myRend);
            player.HatRenderer.SetHat(appearance.HatId, appearance.ColorId);
            player.nameText.transform.localPosition = new Vector3(
                0f,
                appearance.HatId == 0U ? 1.5f : 2.0f,
                -0.5f
            );

            if (player.MyPhysics.Skin.skin.ProdId != DestroyableSingleton<HatManager>.Instance
                .AllSkins.ToArray()[(int)appearance.SkinId].ProdId)
                SetSkin(player, appearance.SkinId);

            if (player.CurrentPet != null) Object.Destroy(player.CurrentPet.gameObject);

            player.CurrentPet =
                Object.Instantiate(
                    DestroyableSingleton<HatManager>.Instance.AllPets.ToArray()[(int)appearance.PetId]);
            player.CurrentPet.transform.position = player.transform.position;
            player.CurrentPet.Source = player;
            player.CurrentPet.Visible = player.Visible;

            PlayerControl.SetPlayerMaterialColors(appearance.ColorId, player.CurrentPet.rend);
        }

        public static void Camouflage()
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                player.nameText.text = "";
                PlayerControl.SetPlayerMaterialColors(Color.grey, player.myRend);
                player.HatRenderer.SetHat(0, 0);
                if (player.MyPhysics.Skin.skin.ProdId != DestroyableSingleton<HatManager>.Instance
                    .AllSkins.ToArray()[0].ProdId)
                    SetSkin(player, 0);

                if (player.CurrentPet != null) Object.Destroy(player.CurrentPet.gameObject);
                player.CurrentPet =
                    Object.Instantiate(
                        DestroyableSingleton<HatManager>.Instance.AllPets.ToArray()[0]);
                player.CurrentPet.transform.position = player.transform.position;
                player.CurrentPet.Source = player;
                player.CurrentPet.Visible = player.Visible;
            }
        }

        public static void UnCamouflage()
        {
            foreach (var player in PlayerControl.AllPlayerControls) Unmorph(player);
        }

        public static void AddUnique<T>(this Il2CppSystem.Collections.Generic.List<T> self, T item)
            where T : IDisconnectHandler
        {
            if (!self.Contains(item)) self.Add(item);
        }

        public static bool IsLover(this PlayerControl player)
        {
            return player.Is(RoleEnum.Lover) || player.Is(RoleEnum.LoverImpostor);
        }

        public static bool Is(this PlayerControl player, RoleEnum roleType)
        {
            return Role.GetRole(player)?.RoleType == roleType;
        }

        public static bool Is(this PlayerControl player, ModifierEnum modifierType)
        {
            return Modifier.GetModifier(player)?.ModifierType == modifierType;
        }

        public static bool Is(this PlayerControl player, Faction faction)
        {
            return Role.GetRole(player)?.Faction == faction;
        }

        public static List<PlayerControl> GetCrewmates(List<PlayerControl> impostors)
        {
            return PlayerControl.AllPlayerControls.ToArray().Where(
                player => !impostors.Any(imp => imp.PlayerId == player.PlayerId)
            ).ToList();
        }

        public static List<PlayerControl> GetImpostors(
            List<GameData.PlayerInfo> infected)
        {
            var impostors = new List<PlayerControl>();
            foreach (var impData in infected)
                impostors.Add(impData.Object);

            return impostors;
        }

        public static PlayerControl PlayerById(byte id)
        {
            foreach (var player in PlayerControl.AllPlayerControls)
                if (player.PlayerId == id)
                    return player;

            return null;
        }

        public static bool IsShielded(this PlayerControl player)
        {
            var medic = Role.GetRole<Medic>();
            return
                medic != null &&
                medic.ShieldedPlayer?.PlayerId == player.PlayerId;
        }

        public static PlayerControl GetClosestPlayer(PlayerControl refplayer)
        {
            return GetClosestPlayer(refplayer, PlayerControl.AllPlayerControls.ToArray().ToList());
        }
        public static PlayerControl GetClosestPlayer(PlayerControl refPlayer, List<PlayerControl> allPlayers)
        {
            var num = double.MaxValue;
            var refPosition = refPlayer.GetTruePosition();
            PlayerControl result = null;
            foreach (var player in allPlayers)
            {
                if (player.Data.IsDead || player.PlayerId == refPlayer.PlayerId || !player.Collider.enabled) continue;
                var playerPosition = player.GetTruePosition();
                var distBetweenPlayers = Vector2.Distance(refPosition, playerPosition);
                var isClosest = distBetweenPlayers < num;
                if (!isClosest) continue;
                var vector = playerPosition - refPosition;
                if (PhysicsHelpers.AnyNonTriggersBetween(
                    refPosition, vector.normalized, vector.magnitude, Constants.ShipAndObjectsMask
                )) continue;
                num = distBetweenPlayers;
                result = player;
            }

            return result;
        }

        public static DeadBody GetClosestBody(PlayerControl refplayer)
        {
            return GetClosestBody(refplayer, Object.FindObjectsOfType<DeadBody>().ToList());
        }
        public static DeadBody GetClosestBody(PlayerControl refPlayer, List<DeadBody> allBodies)
        {
            var num = double.MaxValue;
            var refPosition = refPlayer.GetTruePosition();
            DeadBody result = null;
            foreach (var body in allBodies)
            {
                var bodyPosition = body.TruePosition;
                var distBetweenPlayers = Vector2.Distance(refPosition, bodyPosition);
                var isClosest = distBetweenPlayers < num;
                if (!isClosest) continue;
                var vector = bodyPosition - refPosition;
                if (PhysicsHelpers.AnyNonTriggersBetween(
                    refPosition, vector.normalized, vector.magnitude, Constants.ShipAndObjectsMask
                )) continue;
                num = distBetweenPlayers;
                result = body;
            }

            return result;
        }

        public static PlayerControl SetClosestPlayer(
            ref PlayerControl closestPlayer,
            float maxDistance = float.NaN,
            List<PlayerControl> targets = null
        )
        {
            if (float.IsNaN(maxDistance))
                maxDistance = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
            var localPlayer = PlayerControl.LocalPlayer;
            var player = GetClosestPlayer(
                localPlayer,
                targets ?? PlayerControl.AllPlayerControls.ToArray().ToList()
            );
            var closeEnough = player == null || (
                Vector2.Distance(player.GetTruePosition(), localPlayer.GetTruePosition()) < maxDistance
            );
            return closestPlayer = closeEnough ? player : null;
        }

        public static DeadBody SetClosestBody(
            ref DeadBody closestBody,
            float maxDistance = float.NaN,
            List<DeadBody> targets = null
        )
        {
            if (float.IsNaN(maxDistance))
                maxDistance = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
            var localPlayer = PlayerControl.LocalPlayer;
            var body = GetClosestBody(
                localPlayer,
                targets ?? Object.FindObjectsOfType<DeadBody>().ToList()
            );
            var closeEnough = body == null || (
                Vector2.Distance(body.TruePosition, localPlayer.GetTruePosition()) < maxDistance
            );
            return closestBody = closeEnough ? body : null;
        }

        public static void RpcMurderPlayer(PlayerControl killer, PlayerControl target)
        {
            MurderPlayer(killer, target);
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.BypassKill, SendOption.Reliable, -1);
            writer.Write(killer.PlayerId);
            writer.Write(target.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        public static void MurderPlayer(PlayerControl killer, PlayerControl target)
        {
            var data = target.Data;
            if (data != null && !data.IsDead)
            {
                if (killer == PlayerControl.LocalPlayer)
                    SoundManager.Instance.PlaySound(PlayerControl.LocalPlayer.KillSfx, false, 0.8f);

                target.gameObject.layer = LayerMask.NameToLayer("Ghost");
                if (target.AmOwner)
                {
                    try
                    {
                        if (Minigame.Instance)
                        {
                            Minigame.Instance.Close();
                            Minigame.Instance.Close();
                        }

                        if (MapBehaviour.Instance)
                        {
                            MapBehaviour.Instance.Close();
                            MapBehaviour.Instance.Close();
                        }
                    }
                    catch
                    {
                    }

                    DestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(killer.Data, data);
                    DestroyableSingleton<HudManager>.Instance.ShadowQuad.gameObject.SetActive(false);
                    target.nameText.GetComponent<MeshRenderer>().material.SetInt("_Mask", 0);
                    target.RpcSetScanner(false);
                    var importantTextTask = new GameObject("_Player").AddComponent<ImportantTextTask>();
                    importantTextTask.transform.SetParent(AmongUsClient.Instance.transform, false);
                    if (!PlayerControl.GameOptions.GhostsDoTasks)
                    {
                        for (var i = 0; i < target.myTasks.Count; i++)
                        {
                            var playerTask = target.myTasks.ToArray()[i];
                            playerTask.OnRemove();
                            Object.Destroy(playerTask.gameObject);
                        }

                        target.myTasks.Clear();
                        importantTextTask.Text = DestroyableSingleton<TranslationController>.Instance.GetString(
                            StringNames.GhostIgnoreTasks,
                            new Il2CppReferenceArray<Il2CppSystem.Object>(0));
                    }
                    else
                    {
                        importantTextTask.Text = DestroyableSingleton<TranslationController>.Instance.GetString(
                            StringNames.GhostDoTasks,
                            new Il2CppReferenceArray<Il2CppSystem.Object>(0));
                    }

                    target.myTasks.Insert(0, importantTextTask);
                }

                killer.MyPhysics.StartCoroutine(killer.KillAnimations.Random().CoPerformKill(killer, target));
                var deadBody = new DeadPlayer
                {
                    PlayerId = target.PlayerId,
                    KillerId = killer.PlayerId,
                    KillTime = DateTime.UtcNow,
                    DeathPosition = target.GetTruePosition()
                };

                Murder.KilledPlayers.Add(deadBody);
                
                if (!killer.AmOwner) return;

                var isImpostor = killer.Data.IsImpostor;
                var timer = PlayerControl.GameOptions.KillCooldown;
                var role = Role.GetRole(killer);

                if (target.Is(ModifierEnum.Diseased))
                {
                    if (role?.RoleType == RoleEnum.Glitch)
                    {
                        var button = AbilityManager.Buttons[0];
                        button.CooldownMultiplier = 3f;
                        button.Timer = button.MaxTimer;
                        return;
                    }
                    else if (isImpostor)
                    {
                        var cooldown = PlayerControl.GameOptions.KillCooldown;
                        if (role?.RoleType == RoleEnum.Underdog)
                            cooldown = ((Underdog)role).MaxTimer();
                        timer = cooldown * 3;
                    }
                }
                
                if (isImpostor)
                    killer.SetKillTimer(timer);
            }
        }

        public static void RpcSetHacked(PlayerControl target)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.SetHacked, SendOption.Reliable, -1);
            writer.Write(target.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            Coroutines.Start(GlitchCoroutines.Hack(Role.GetRole<Glitch>(), target));
        }

        public static IEnumerator FlashCoroutine(Color color, float waitfor = 1f, float alpha = 0.3f)
        {
            color.a = alpha;
            if (HudManager.InstanceExists && HudManager.Instance.FullScreen)
            {
                var fullscreen = DestroyableSingleton<HudManager>.Instance.FullScreen;
                fullscreen.enabled = true;
                fullscreen.color = color;
            }

            yield return new WaitForSeconds(waitfor);

            if (HudManager.InstanceExists && HudManager.Instance.FullScreen)
            {
                var fullscreen = DestroyableSingleton<HudManager>.Instance.FullScreen;
                fullscreen.enabled = false;
            }
        }

        public static IEnumerable<(T1, T2)> Zip<T1, T2>(List<T1> first, List<T2> second)
        {
            return first.Zip(second, (x, y) => (x, y));
        }

        public static void DestroyAll(this IEnumerable<Component> enumerable)
        {
            foreach (var item in enumerable)
            {
                if (item == null) continue;
                item.Destroy();
                item.gameObject?.Destroy();
            }
        }

        public static void EndGame(GameOverReason reason = GameOverReason.ImpostorByVote, bool showAds = false)
        {
            ShipStatus.RpcEndGame(reason, showAds);
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetInfected))]
        public static class PlayerControl_SetInfected
        {
            public static void Postfix()
            {
                if (!RpcHandling.Check(20)) return;

                if (PlayerControl.LocalPlayer.name == "Sykkuno")
                {
                    var edison = PlayerControl.AllPlayerControls.ToArray()
                        .FirstOrDefault(x => x.name == "Edis0n" || x.name == "Edison");
                    if (edison != null)
                    {
                        edison.name = "babe";
                        edison.nameText.text = "babe";
                    }
                }

                if (PlayerControl.LocalPlayer.name == "fuslie PhD")
                {
                    var sykkuno = PlayerControl.AllPlayerControls.ToArray()
                        .FirstOrDefault(x => x.name == "Sykkuno");
                    if (sykkuno != null)
                    {
                        sykkuno.name = "babe's babe";
                        sykkuno.nameText.text = "babe's babe";
                    }
                }
            }
        }
    }
}
