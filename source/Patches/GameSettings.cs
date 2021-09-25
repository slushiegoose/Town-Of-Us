using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using Reactor.Extensions;
using TownOfUs.CustomOption;
using TownOfUs.Extensions;
using UnityEngine;

namespace TownOfUs
{
    [HarmonyPatch]
    public static class GameSettings
    {
        [HarmonyPatch] //ToHudString
        private static class GameOptionsDataPatch
        {
            public static IEnumerable<MethodBase> TargetMethods()
            {
                return typeof(GameOptionsData).GetMethods(typeof(string), typeof(int));
            }

            public static void Postfix(ref string __result)
            {
                var builder = new StringBuilder(__result);

                var showLater = new List<CustomOption.CustomOption>();

                foreach (var option in CustomOption.CustomOption.AllOptions)
                {
                    if (option.Type == CustomOptionType.Button) continue;
                    if (option.Type == CustomOptionType.Header) 
                        builder.AppendLine($"\n{option.Name}");
                    else if (option.Parent != null)
                        showLater.Add(option);
                    else
                    {
                        var prefix = option.Indent ? "     " : "";
                        builder.AppendLine($"{prefix}{option.Name}: {option}");
                    }
                }

                var parentId = -1;

                foreach (var option in showLater)
                {
                    var parent = option.Parent;
                    if (parentId != parent.ID)
                    {
                        builder.AppendLine($"\n{parent.Name}");
                        parentId = parent.ID;
                    }
                    builder.AppendLine($"{option.Name}: {option}");
                }

                if (CustomOption.CustomOption.LobbyTextScroller)
                    builder.Insert(0, "(Scroll for more)\n");


                __result = $"<size=1.25>{builder}</size>";
            }
        }

        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
        public static class Update
        {
            public static void Postfix(ref GameOptionsMenu __instance)
            {
                var scroller = __instance.GetComponentInParent<Scroller>();
                scroller.YBounds.max = 70f;
            }
        }
    }
}
