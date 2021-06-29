using System;
using System.Net;
using System.Threading.Tasks;
using Reactor.Extensions;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs
{
    public static class Lights
    {
        public static void SetLights(Color color)
        {
            try
            {
                Task.Run(() =>
                {
                    var str = color.ToHtmlStringRGBA().Substring(0, 6);
                    var url = "http://localhost:42269/setColor?color=" + str;
                    var request = (HttpWebRequest) WebRequest.Create(new Uri(url));
                    request.Method = "GET";
                    request.Timeout = 2 * 1000;
                    request.GetResponse();
                });
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
        }

        public static void SetLights()
        {
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Crewmate))
            {
                SetLights(Palette.CrewmateBlue);
                return;
            }

            SetLights(Role.GetRole(PlayerControl.LocalPlayer).Color);
        }
    }
}