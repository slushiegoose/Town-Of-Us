// Decompiled with JetBrains decompiler
// Type: TownOfUs.TimeMasterMod.PointInTime
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using UnityEngine;

namespace TownOfUs.TimeMasterMod
{
  public class PointInTime
  {
    public Vector3 position;
    public Vector2 velocity;
    public float unix;

    public PointInTime(Vector3 position, Vector2 velocity, float unix)
    {
      this.position = position;
      this.velocity = velocity;
      this.unix = unix;
    }
  }
}
