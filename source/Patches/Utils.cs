using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using TownOfUs.CrewmateRoles.MedicMod;
using TownOfUs.CustomHats;
using TownOfUs.Extensions;
using TownOfUs.ImpostorRoles.CamouflageMod;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;
using UnhollowerBaseLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfUs
{
    [HarmonyPatch]
    public static class Utils
    {
        internal static bool ShowDeadBodies = false;


        public static Dictionary<PlayerControl, Color> oldColors = new Dictionary<PlayerControl, Color>();

        public static List<WinningPlayerData> potentialWinners = new List<WinningPlayerData>();

        public static void SetSkin(PlayerControl Player, uint skin)
        {
            Player.MyPhysics.SetSkin(skin);
        }

        public static void Morph(PlayerControl Player, PlayerControl MorphedPlayer, bool resetAnim = false)
        {
            if (CamouflageUnCamouflage.IsCamoed) return;

            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Seer)) Player.nameText.text = MorphedPlayer.Data.PlayerName;

            var colorId = MorphedPlayer.Data.ColorId;
            PlayerControl.SetPlayerMaterialColors(colorId, Player.myRend);
            Player.HatRenderer.SetHat(MorphedPlayer.Data.HatId, colorId);
            Player.nameText.transform.localPosition = new Vector3(
                0f,
                Player.Data.HatId == 0U ? 1.5f :
                HatCreation.TallIds.Contains(Player.Data.HatId) ? 2.2f : 2.0f,
                -0.5f
            );

            if (Player.MyPhysics.Skin.skin.ProdId != DestroyableSingleton<HatManager>.Instance
                .AllSkins.ToArray()[(int) MorphedPlayer.Data.SkinId].ProdId)
                SetSkin(Player, MorphedPlayer.Data.SkinId);

            if (Player.CurrentPet == null || Player.CurrentPet.ProdId !=
                DestroyableSingleton<HatManager>.Instance.AllPets.ToArray()[(int) MorphedPlayer.Data.PetId].ProdId)
            {
                if (Player.CurrentPet != null) Object.Destroy(Player.CurrentPet.gameObject);

                Player.CurrentPet =
                    Object.Instantiate(
                        DestroyableSingleton<HatManager>.Instance.AllPets.ToArray()[(int) MorphedPlayer.Data.PetId]);
                Player.CurrentPet.transform.position = Player.transform.position;
                Player.CurrentPet.Source = Player;
                Player.CurrentPet.Visible = Player.Visible;
            }

            PlayerControl.SetPlayerMaterialColors(colorId, Player.CurrentPet.rend);
            /*if (resetAnim && !Player.inVent)
            {
                Player.MyPhysics.ResetAnim();
            }*/
        }

        public static void Unmorph(PlayerControl Player)
        {
            var colorId = Player.Data.ColorId;
            Player.nameText.text = Player.Data.PlayerName;
            PlayerControl.SetPlayerMaterialColors(colorId, Player.myRend);
            Player.HatRenderer.SetHat(Player.Data.HatId, colorId);
            Player.nameText.transform.localPosition = new Vector3(
                0f,
                Player.Data.HatId == 0U ? 1.5f :
                HatCreation.TallIds.Contains(Player.Data.HatId) ? 2.2f : 2.0f,
                -0.5f
            );

            if (Player.MyPhysics.Skin.skin.ProdId != DestroyableSingleton<HatManager>.Instance
                .AllSkins.ToArray()[(int) Player.Data.SkinId].ProdId)
                SetSkin(Player, Player.Data.SkinId);


            if (Player.CurrentPet != null) Object.Destroy(Player.CurrentPet.gameObject);

            Player.CurrentPet =
                Object.Instantiate(
                    DestroyableSingleton<HatManager>.Instance.AllPets.ToArray()[(int) Player.Data.PetId]);
            Player.CurrentPet.transform.position = Player.transform.position;
            Player.CurrentPet.Source = Player;
            Player.CurrentPet.Visible = Player.Visible;


            PlayerControl.SetPlayerMaterialColors(colorId, Player.CurrentPet.rend);

            /*if (!Player.inVent)
            {
                Player.MyPhysics.ResetAnim();
            }*/
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


        public static bool IsCrewmate(this PlayerControl player)
        {
            return GetRole(player) == RoleEnum.Crewmate;
        }


        public static void AddUnique<T>(this Il2CppSystem.Collections.Generic.List<T> self, T item)
            where T : IDisconnectHandler
        {
            if (!self.Contains(item)) self.Add(item);
        }

        public static bool isLover(this PlayerControl player)
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

        public static RoleEnum GetRole(PlayerControl player)
        {
            if (player == null) return RoleEnum.None;
            if (player.Data == null) return RoleEnum.None;

            var role = Role.GetRole(player);
            if (role != null) return role.RoleType;

            return player.Data.IsImpostor ? RoleEnum.Impostor : RoleEnum.Crewmate;
        }

        public static PlayerControl PlayerById(byte id)
        {
            foreach (var player in PlayerControl.AllPlayerControls)
                if (player.PlayerId == id)
                    return player;

            return null;
        }

        public static List<PlayerControl> getCrewmates(IEnumerable<GameData.PlayerInfo> infection)
        {
            var list = new List<PlayerControl>();
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                var isImpostor = false;
                foreach (var impostor in infection)
                    if (player.PlayerId == impostor.Object.PlayerId)
                        isImpostor = true;

                if (!isImpostor) list.Add(player);
            }

            return list;
        }

        public static bool isShielded(this PlayerControl player)
        {
            return Role.GetRoles(RoleEnum.Medic).Any(role =>
            {
                var shieldedPlayer = ((Medic) role).ShieldedPlayer;
                return shieldedPlayer != null && player.PlayerId == shieldedPlayer.PlayerId;
            });
        }

        public static Medic getMedic(this PlayerControl player)
        {
            return Role.GetRoles(RoleEnum.Medic).FirstOrDefault(role =>
            {
                var shieldedPlayer = ((Medic) role).ShieldedPlayer;
                return shieldedPlayer != null && player.PlayerId == shieldedPlayer.PlayerId;
            }) as Medic;
        }

        public static List<PlayerControl> getImpostors(IEnumerable<GameData.PlayerInfo> infection)
        {
            var list = new List<PlayerControl>();
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                var isImpostor = false;
                foreach (var impostor in infection)
                    if (player.PlayerId == impostor.Object.PlayerId)
                        isImpostor = true;

                if (isImpostor) list.Add(player);
            }

            return list;
        }

        public static PlayerControl getClosestPlayer(PlayerControl refPlayer, List<PlayerControl> AllPlayers)
        {
            var num = double.MaxValue;
            var refPosition = refPlayer.GetTruePosition();
            PlayerControl result = null;
            foreach (var player in AllPlayers)
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

        public static PlayerControl getClosestPlayer(PlayerControl refplayer)
        {
            return getClosestPlayer(refplayer, PlayerControl.AllPlayerControls.ToArray().ToList());
        }
        public static void SetTarget(
            ref PlayerControl closestPlayer,
            KillButtonManager button,
            float maxDistance = float.NaN,
            List<PlayerControl> targets = null
        )
        {
            if (!button.isActiveAndEnabled) return;
            if (maxDistance == float.NaN)
                maxDistance = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
            closestPlayer = getClosestPlayer(
                PlayerControl.LocalPlayer,
                targets ?? PlayerControl.AllPlayerControls.ToArray().ToList()
            );
            var closeEnough = closestPlayer == null || (
                getDistBetweenPlayers(PlayerControl.LocalPlayer, closestPlayer) < maxDistance
            );
            button.SetTarget(closeEnough ? closestPlayer : null);
        }


        public static double getDistBetweenPlayers(PlayerControl player, PlayerControl refplayer)
        {
            var truePosition = refplayer.GetTruePosition();
            var truePosition2 = player.GetTruePosition();
            return Vector2.Distance(truePosition, truePosition2);
        }


        public static void RpcMurderPlayer(PlayerControl killer, PlayerControl target)
        {
            MurderPlayer(killer, target);
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.BypassKill, SendOption.Reliable, -1);
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
                    KillTime = DateTime.UtcNow
                };

                Murder.KilledPlayers.Add(deadBody);
                if (!killer.Is(RoleEnum.Glitch) && !killer.Is(RoleEnum.Arsonist))
                    CrewmateRoles.ChildMod.Murder.CheckChild(target);

                if (target.Is(ModifierEnum.Diseased) && killer.Is(RoleEnum.Glitch))
                {
                    var glitch = Role.GetRole<Glitch>(killer);
                    glitch.LastKill = DateTime.UtcNow.AddSeconds(2 * CustomGameOptions.GlitchKillCooldown);
                    glitch.Player.SetKillTimer(CustomGameOptions.GlitchKillCooldown * 3);
                    return;
                }

                if (target.Is(ModifierEnum.Diseased) && killer.Data.IsImpostor)
                {
                    killer.SetKillTimer(PlayerControl.GameOptions.KillCooldown * 3);
                    return;
                }

                if (killer.Data.IsImpostor)
                {
                    killer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
                }
            }
        }

        public static IEnumerator FlashCoroutine(Color color, float waitfor = 1f, float alpha = 0.3f)
        {
            color.a = alpha;
            var fullscreen = DestroyableSingleton<HudManager>.Instance.FullScreen;
            var oldcolour = fullscreen.color;
            fullscreen.enabled = true;
            fullscreen.color = color;
            yield return new WaitForSeconds(waitfor);
            fullscreen.enabled = false;
            fullscreen.color = oldcolour;
        }

        public static IEnumerable<(T1, T2)> Zip<T1, T2>(List<T1> first, List<T2> second)
        {
            return first.Zip(second, (x, y) => (x, y));
        }

        public static void DestroyAll(this IEnumerable<Component> listie)
        {
            foreach (var item in listie)
            {
                if (item == null) continue;
                Object.Destroy(item);
                if (item.gameObject == null) return;
                Object.Destroy(item.gameObject);
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
