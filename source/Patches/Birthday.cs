using UnityEngine;

namespace TownOfUs
{
    //[HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
    public static class BirthdayButtons
    {
        public static GameObject Other;

        public static void Postfix(MeetingHud __instance)
        {
            foreach (var state in __instance.playerStates)
            {
                var name = state.NameText.text.ToLower();
                if (name.Contains("vikramafc"))
                {
                    // if (Other == null)
                    // {
                    //     Other = new GameObject("Other");
                    //     Other.transform.parent = state.transform;
                    //     var renderer = Other.AddComponent<SpriteRenderer>();
                    //     Other.transform.localPosition = new Vector3(1.6f, -0.01f, 0f);
                    //     Other.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                    //     Other.layer = 5;
                    // }
                }
            }
        }
    }
}