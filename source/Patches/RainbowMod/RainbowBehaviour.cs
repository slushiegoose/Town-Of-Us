using System;
using Reactor;
using UnhollowerBaseLib.Attributes;
using UnityEngine;

namespace TownOfUs.RainbowMod
{
    [RegisterInIl2Cpp]
    public class RainbowBehaviour : MonoBehaviour
    {
        public Renderer Renderer;
        public int Id;

        [HideFromIl2Cpp]
        public void AddRend(Renderer rend, int id)
        {
            Renderer = rend;
            Id = id;
        }

        public void Update()
        {
            if (Renderer == null) return;

            if (RainbowUtils.IsRainbow(Id))
            {
                RainbowUtils.SetRainbow(Renderer);
            }
        }

        public RainbowBehaviour(IntPtr ptr) : base(ptr)
        {
        }
    }
}
