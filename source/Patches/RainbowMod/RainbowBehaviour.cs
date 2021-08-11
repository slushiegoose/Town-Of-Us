using Reactor;
using System;
using UnityEngine;

namespace TownOfUs.RainbowMod
{
    [RegisterInIl2Cpp]
    public class RainbowBehaviour : MonoBehaviour
    {
        public Renderer Renderer;
        public int Id;

        public void AddRend(Renderer rend, int id)
        {
            Renderer = rend;
            Id = id;
        }

        public void Update()
        {
            if (RainbowUtils.IsRainbow(Id) && Renderer != null)
                RainbowUtils.SetRainbow(Renderer);
        }

        public RainbowBehaviour(IntPtr ptr) : base(ptr) { }
    }
}
