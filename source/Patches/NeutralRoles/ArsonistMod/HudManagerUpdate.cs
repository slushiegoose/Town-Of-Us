using System.Linq;
using HarmonyLib;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.NeutralRoles.ArsonistMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HudManagerUpdate
    {
        public static Sprite IgniteSprite => TownOfUs.IgniteSprite;

        public static void UpdateMeeting(MeetingHud __instance, Arsonist role)
        {
            foreach (var state in __instance.playerStates)
            {
                var player = PlayerControl.AllPlayerControls.ToArray()
                    .FirstOrDefault(x => x.PlayerId == state.TargetPlayerId);
                if (player == null) continue;
                if (role.DousedPlayers.Contains(player.PlayerId)) state.NameText.color = Color.black;
            }
        }

        public static void Postfix(HudManager __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Arsonist)) return;
            var role = Role.GetRole<Arsonist>(PlayerControl.LocalPlayer);

            if (MeetingHud.Instance != null) UpdateMeeting(MeetingHud.Instance, role);
            foreach (var playerId in role.DousedPlayers)
            {
                var player = Utils.PlayerById(playerId);
                player.myRend.material.SetColor("_VisorColor", role.Color);
                player.nameText.color = Color.black;
            }

            if (role.IgniteButton == null)
            {
                role.IgniteButton = Object.Instantiate(__instance.KillButton, HudManager.Instance.transform);
                role.IgniteButton.renderer.enabled = true;
            }

            role.IgniteButton.renderer.sprite = IgniteSprite;
            var position = __instance.KillButton.transform.localPosition;
            role.IgniteButton.transform.localPosition = new Vector3(position.x,
                __instance.ReportButton.transform.localPosition.y, position.z);

            role.IgniteButton.gameObject.SetActive(!PlayerControl.LocalPlayer.Data.IsDead && !MeetingHud.Instance);
            __instance.KillButton.gameObject.SetActive(!PlayerControl.LocalPlayer.Data.IsDead && !MeetingHud.Instance);
            role.IgniteButton.SetCoolDown(0f, 1f);
            __instance.KillButton.SetCoolDown(role.DouseTimer(), CustomGameOptions.DouseCd);

            var notDoused = PlayerControl.AllPlayerControls.ToArray().Where(
                player => !role.DousedPlayers.Contains(player.PlayerId)
            ).ToList();

            Utils.SetTarget(ref role.ClosestPlayer, __instance.KillButton, float.NaN, notDoused);


            if (!role.IgniteButton.isCoolingDown & role.IgniteButton.isActiveAndEnabled & !role.IgniteUsed &
                role.CheckEveryoneDoused())
            {
                role.IgniteButton.renderer.color = Palette.EnabledColor;
                role.IgniteButton.renderer.material.SetFloat("_Desat", 0f);
                return;
            }

            role.IgniteButton.renderer.color = Palette.DisabledClear;
            role.IgniteButton.renderer.material.SetFloat("_Desat", 1f);
        }
    }
}
