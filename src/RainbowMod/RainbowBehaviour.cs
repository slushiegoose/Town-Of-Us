// Decompiled with JetBrains decompiler
// Type: TownOfUs.RainbowMod.RainbowBehaviour
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

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
      this.Renderer = rend;
      this.Id = id;
    }

    public void Update()
    {
      if (Object.op_Equality((Object) this.Renderer, (Object) null) || !RainbowUtils.IsRainbow(this.Id))
        return;
      RainbowUtils.SetRainbow(this.Renderer);
    }

    public RainbowBehaviour(IntPtr ptr) => base.\u002Ector(ptr);
  }
}
