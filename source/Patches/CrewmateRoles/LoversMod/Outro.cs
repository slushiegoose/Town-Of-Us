using System.Linq;
using HarmonyLib;
using TMPro;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.CrewmateRoles.LoversMod
{
    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.Start))]
    internal class Outro
    {
        public static void Postfix(EndGameManager __instance)
        {
            TextMeshPro text;
            Vector3 pos;
            if (Role.NobodyWins)
            {
                text = Object.Instantiate(__instance.WinText);
                var color = __instance.WinText.color;
                color.a = 1f;
                text.color = color;
                text.text = "Only neutral roles were left";
                pos = __instance.WinText.transform.localPosition;
                pos.y = 1.5f;
                text.transform.position = pos;
//				text.scale = 0.5f;
                return;
            }


            if (!Role.AllRoles.Where(x => x.RoleType == RoleEnum.Lover || x.RoleType == RoleEnum.LoverImpostor)
                .Any(x => ((Lover) x).LoveCoupleWins)) return;
            if (Role.GetRoles(RoleEnum.Jester).Any(x => ((Jester) x).VotedOut)) return;
            PoolablePlayer[] array = Object.FindObjectsOfType<PoolablePlayer>();
            if (array[0] != null)
            {
                array[0].gameObject.transform.position -= new Vector3(1.5f, 0f, 0f);
                array[0].SetFlipX(true);
                array[0].NameText.color = new Color(1f, 0.4f, 0.8f, 1f);
            }

            if (array[1] != null)
            {
                array[1].SetFlipX(false);
                array[1].gameObject.transform.position =
                    array[0].gameObject.transform.position + new Vector3(1.2f, 0f, 0f);
                array[1].gameObject.transform.localScale *= 0.92f;
                array[1].HatSlot.transform.position += new Vector3(0.1f, 0f, 0f);
                array[1].NameText.color = new Color(1f, 0.4f, 0.8f, 1f);
            }

            __instance.BackgroundBar.material.color = new Color(1f, 0.4f, 0.8f, 1f);

            text = Object.Instantiate(__instance.WinText);
            text.text = "Love couple wins";
            text.color = new Color(1f, 0.4f, 0.8f, 1f);
            pos = __instance.WinText.transform.localPosition;
            pos.y = 1.5f;
            text.transform.position = pos;
//			text.scale = 1f;
        }
    }
}