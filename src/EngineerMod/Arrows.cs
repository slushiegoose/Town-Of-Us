// Decompiled with JetBrains decompiler
// Type: TownOfUs.EngineerMod.Arrows
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using Hazel;
using Il2CppSystem;
using Reactor;
using Reactor.Extensions;
using UnityEngine;

namespace TownOfUs.EngineerMod
{
  [HarmonyPatch(typeof (FFGALNAPKCD), "FixedUpdate")]
  public class Arrows
  {
    public static DGPHMLNNPDN Arrow;
    public static bool ArrowCreated;

    private static Sprite Sprite => TownOfUs.TownOfUs.EngineerArrow;

    public static bool ArrowsOpen
    {
      get
      {
        DateTime sabotageTime = PerformKill.SabotageTime;
        TimeSpan timeSpan = DateTime.op_Subtraction(DateTime.get_UtcNow(), sabotageTime);
        return ((TimeSpan) ref timeSpan).get_TotalSeconds() < 10.0;
      }
    }

    public static void GenArrows()
    {
      Arrows.ArrowCreated = true;
      PerformKill.UsedThisRound = true;
      PerformKill.SabotageTime = DateTime.get_Now();
      if (FFGALNAPKCD.get_LocalPlayer().get_Data().get_DLPCKPBIJOE() || !FFGALNAPKCD.get_LocalPlayer().get_Data().get_DAPKNDBLKIA() || Utils.Engineer.get_Data().get_DLPCKPBIJOE())
        return;
      GameObject gameObject = new GameObject();
      Arrows.Arrow = (DGPHMLNNPDN) gameObject.AddComponent<DGPHMLNNPDN>();
      gameObject.get_transform().set_parent(((Component) FFGALNAPKCD.get_LocalPlayer()).get_gameObject().get_transform());
      SpriteRenderer spriteRenderer = (SpriteRenderer) gameObject.AddComponent<SpriteRenderer>();
      spriteRenderer.set_sprite(Arrows.Sprite);
      Arrows.Arrow.set_image(spriteRenderer);
      gameObject.set_layer(5);
      PluginSingleton<TownOfUs.TownOfUs>.get_Instance().get_Log().LogMessage((object) "CREATED ARROW");
    }

    public static void RemoveArrows()
    {
      Arrows.ArrowCreated = false;
      if (Object.op_Equality((Object) Arrows.Arrow, (Object) null))
        return;
      UnityObjectExtensions.Destroy((Object) ((Component) Arrows.Arrow).get_gameObject());
      Arrows.Arrow = (DGPHMLNNPDN) null;
      PluginSingleton<TownOfUs.TownOfUs>.get_Instance().get_Log().LogMessage((object) "DELETED ARROW");
    }

    public static void Postfix(FFGALNAPKCD __instance)
    {
      if (Object.op_Equality((Object) Utils.Engineer, (Object) null))
        return;
      if (Arrows.ArrowsOpen && !Arrows.ArrowCreated && !Utils.Engineer.get_Data().get_DLPCKPBIJOE() && FFGALNAPKCD.get_LocalPlayer().isEngineer())
      {
        Arrows.GenArrows();
        PluginSingleton<TownOfUs.TownOfUs>.get_Instance().get_Log().LogMessage((object) "Set");
        MessageWriter messageWriter = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 64, (SendOption) 1, -1);
        ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter);
      }
      if ((Utils.Engineer.get_Data().get_DLPCKPBIJOE() || !Arrows.ArrowsOpen) && Arrows.ArrowCreated && FFGALNAPKCD.get_LocalPlayer().isEngineer())
      {
        Arrows.RemoveArrows();
        PluginSingleton<TownOfUs.TownOfUs>.get_Instance().get_Log().LogMessage((object) "Die");
        MessageWriter messageWriter = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 65, (SendOption) 1, -1);
        ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter);
      }
      if (!__instance.isEngineer() || !Object.op_Inequality((Object) Arrows.Arrow, (Object) null))
        return;
      Arrows.Arrow.set_target(((Component) __instance.get_MyPhysics()).get_transform().get_position());
    }
  }
}
