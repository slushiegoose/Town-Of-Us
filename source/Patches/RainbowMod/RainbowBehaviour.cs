using System;
using UnityEngine;

namespace TownOfUs.RainbowMod
{
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
            if (Renderer == null) return;

            if (RainbowUtils.IsRainbow(Id))
            {
                RainbowUtils.SetRainbow(Renderer);
            }
        }

        public RainbowBehaviour(IntPtr ptr) : base(ptr) { }
    }
}
