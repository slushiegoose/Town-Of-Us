using UnityEngine;

namespace TownOfUs.Extensions
{
    public static class UnityObjectExtensions
    {
        /// <summary>
        ///     Stops <paramref name="obj" /> from being destroyed
        /// </summary>
        /// <param name="obj">Object to stop from being destroyed</param>
        /// <returns>Passed <paramref name="obj" /></returns>
        public static T DontDestroy<T>(this T obj) where T : Object
        {
            obj.hideFlags |= HideFlags.HideAndDontSave;

            return obj.DontDestroyOnLoad();
        }

        public static T DontUnload<T>(this T obj) where T : Object
        {
            obj.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            return obj;
        }

        public static T DontDestroyOnLoad<T>(this T obj) where T : Object
        {
            Object.DontDestroyOnLoad(obj);

            return obj;
        }

        public static void Destroy(this Object obj)
        {
            Object.Destroy(obj);
        }

        public static void DestroyImmediate(this Object obj)
        {
            Object.DestroyImmediate(obj);
        }
    }
}