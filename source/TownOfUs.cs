using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using TownOfUs.CustomHats;
using TownOfUs.CustomOption;
using TownOfUs.Extensions;
using TownOfUs.RainbowMod;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TownOfUs
{
    [BepInPlugin("com.slushiegoose.townofus", "Town Of Us", "2.1.3")]
    public class TownOfUs : BasePlugin
    {
        public static Sprite JanitorClean;
        public static Sprite EngineerFix;
        public static Sprite SwapperSwitch;
        public static Sprite SwapperSwitchDisabled;
        public static Sprite Shift;
        public static Sprite Kill;
        public static Sprite Footprint;
        public static Sprite Rewind;
        public static Sprite NormalKill;
        public static Sprite GreyscaleKill;
        public static Sprite ShiftKill;
        public static Sprite MedicSprite;
        public static Sprite SeerSprite;
        public static Sprite SampleSprite;
        public static Sprite MorphSprite;
        public static Sprite UseButton;
        public static Sprite Camouflage;
        public static Sprite Arrow;
        public static Sprite CreateCamSprite;
        public static Sprite SecuritySprite;
        public static Sprite Abstain;
        public static Sprite MineSprite;
        public static Sprite SwoopSprite;
        public static Sprite DouseSprite;
        public static Sprite IgniteSprite;
        public static Sprite ReviveSprite;
        public static Sprite ButtonSprite;
        public static Sprite PolusSprite;
        public static Sprite BannerSprite;

        public static Sprite CycleSprite;
        public static Sprite GuessSprite;


        public static Sprite WolfVoteSprite;

        private static DLoadImage _iCallLoadImage;


        private Harmony _harmony;

        public ConfigEntry<string> Ip { get; set; }

        public ConfigEntry<ushort> Port { get; set; }
        //public static Sprite BirthdayVoteSprite;


        public override void Load()
        {
            try
            {
                PluginSingleton<TownOfUs>.Instance = this;

                System.Console.WriteLine("000.000.000.000/000000000000000000");

                _harmony = new Harmony("com.slushiegoose.townofus");

                Generate.GenerateAll();

                JanitorClean = CreateSprite("TownOfUs.Resources.Janitor.png");
                EngineerFix = CreateSprite("TownOfUs.Resources.Engineer.png");
                //EngineerArrow = CreateSprite("TownOfUs.Resources.EngineerArrow.png");
                SwapperSwitch = CreateSprite("TownOfUs.Resources.SwapperSwitch.png");
                SwapperSwitchDisabled = CreateSprite("TownOfUs.Resources.SwapperSwitchDisabled.png");
                Shift = CreateSprite("TownOfUs.Resources.Shift.png");
                Kill = CreateSprite("TownOfUs.Resources.Kill.png");
                Footprint = CreateSprite("TownOfUs.Resources.Footprint.png");
                Rewind = CreateSprite("TownOfUs.Resources.Rewind.png");
                NormalKill = CreateSprite("TownOfUs.Resources.NormalKill.png");
                GreyscaleKill = CreateSprite("TownOfUs.Resources.GreyscaleKill.png");
                ShiftKill = CreateSprite("TownOfUs.Resources.ShiftKill.png");
                MedicSprite = CreateSprite("TownOfUs.Resources.Medic.png");
                SeerSprite = CreateSprite("TownOfUs.Resources.Seer.png");
                SampleSprite = CreateSprite("TownOfUs.Resources.Sample.png");
                MorphSprite = CreateSprite("TownOfUs.Resources.Morph.png");
                UseButton = CreateSprite("TownOfUs.Resources.UseButton.png");
                Camouflage = CreateSprite("TownOfUs.Resources.Camouflage.png");
                Arrow = CreateSprite("TownOfUs.Resources.Arrow.png");
                CreateCamSprite = CreateSprite("TownOfUs.Resources.CreateCam.png");
                SecuritySprite = CreateSprite("TownOfUs.Resources.Security.png");
                Abstain = CreateSprite("TownOfUs.Resources.Abstain.png");
                MineSprite = CreateSprite("TownOfUs.Resources.Mine.png");
                SwoopSprite = CreateSprite("TownOfUs.Resources.Swoop.png");
                DouseSprite = CreateSprite("TownOfUs.Resources.Douse.png");
                IgniteSprite = CreateSprite("TownOfUs.Resources.Ignite.png");
                ReviveSprite = CreateSprite("TownOfUs.Resources.Revive.png");
                ButtonSprite = CreateSprite("TownOfUs.Resources.Button.png");
                PolusSprite = CreateSprite("TownOfUs.Resources.polus.gg.png");
                BannerSprite = CreateSprite("TownOfUs.Resources.Banner.png");
                CycleSprite = CreateSprite("TownOfUs.Resources.Cycle.png");
                GuessSprite = CreateSprite("TownOfUs.Resources.Guess.png");

                // WolfVoteSprite = CreateSprite("TownOfUs.Resources.Vote1.png");
                //BirthdayVoteSprite = CreateSprite("TownOfUs.Resources.Vote2.png");

                PalettePatch.Load();
                ClassInjector.RegisterTypeInIl2Cpp<RainbowBehaviour>();
                ClassInjector.RegisterTypeInIl2Cpp<HatAnimator>();

                // RegisterInIl2CppAttribute.Register();
                ClassInjector.RegisterTypeInIl2Cpp<Coroutines.Component>();

                var gameObject = new GameObject(nameof(TownOfUs)).DontDestroy();
                gameObject.AddComponent<Coroutines.Component>();

                Ip = Config.Bind("Custom", "Ipv4 or Hostname", "127.0.0.1");
                Port = Config.Bind("Custom", "Port", (ushort) 22023);
                var defaultRegions = ServerManager.DefaultRegions.ToList();
                var ip = Ip.Value;
                if (Uri.CheckHostName(Ip.Value).ToString() == "Dns")
                    foreach (var address in Dns.GetHostAddresses(Ip.Value))
                    {
                        if (address.AddressFamily != AddressFamily.InterNetwork)
                            continue;
                        ip = address.ToString();
                        break;
                    }

                // ServerManager.Instance.AddOrUpdateRegion(new StaticRegionInfo(
                // 	"Custom-Server", StringNames.NoTranslation, ip, new ServerInfo[]
                // 	{
                // 		new ServerInfo("Custom-Server", ip, Port.Value)
                // 	}
                // ).Cast<IRegionInfo>());

                ServerManager.DefaultRegions = defaultRegions.ToArray();

                SceneManager.add_sceneLoaded((Action<Scene, LoadSceneMode>) ((scene, loadSceneMode) =>
                {
                    ModManager.Instance.ShowModStamp();
                }));
                _harmony.PatchAll();
            }
            catch (Exception e)
            {
                Log.LogError($"{e.Message}. Stack: {e.StackTrace}");
            }
        }

        public static Sprite CreateSprite(string name, bool hat = false)
        {
            var pixelsPerUnit = hat ? 225f : 100f;
            var pivot = hat ? new Vector2(0.5f, 0.8f) : new Vector2(0.5f, 0.5f);

            var assembly = Assembly.GetExecutingAssembly();
            var tex = GUIExtensions.CreateEmptyTexture();
            var imageStream = assembly.GetManifestResourceStream(name);
            var img = imageStream.ReadFully();
            LoadImage(tex, img, true);
            tex.DontDestroy();
            var sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), pivot, pixelsPerUnit);
            sprite.DontDestroy();
            return sprite;
        }

        public static Sprite CreatePolusHat(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var imageStream = assembly.GetManifestResourceStream(name);
            var img = imageStream.ReadFully();

            var tex = new Texture2D(128, 128, (TextureFormat) 1, false);
            LoadImage(tex, img, false);
            tex.DontDestroy();
            var sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            sprite.DontDestroy();
            return sprite;
        }

        private static void LoadImage(Texture2D tex, byte[] data, bool markNonReadable)
        {
            _iCallLoadImage ??= IL2CPP.ResolveICall<DLoadImage>("UnityEngine.ImageConversion::LoadImage");
            var il2CPPArray = (Il2CppStructArray<byte>) data;
            _iCallLoadImage.Invoke(tex.Pointer, il2CPPArray.Pointer, markNonReadable);
        }

        private delegate bool DLoadImage(IntPtr tex, IntPtr data, bool markNonReadable);
    }
}
