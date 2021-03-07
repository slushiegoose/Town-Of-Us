// Decompiled with JetBrains decompiler
// Type: TownOfUs.LoversMod.Die
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using Hazel;

namespace TownOfUs.LoversMod
{
  [HarmonyPatch(typeof (FFGALNAPKCD))]
  public class Die
  {
    [HarmonyPatch("Die")]
    public static bool Prefix(FFGALNAPKCD __instance, [HarmonyArgument(0)] DBLJKMDLJIF oecopgmhmkc)
    {
      if (((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).get_ClientId() != ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).get_HostId())
        return true;
      __instance.get_Data().set_DLPCKPBIJOE(true);
      if (!__instance.isLover() || !CustomGameOptions.BothLoversDie)
        return true;
      FFGALNAPKCD ffgalnapkcd = __instance.OtherLover();
      if (ffgalnapkcd.get_Data().get_DLPCKPBIJOE())
        return true;
      MessageWriter messageWriter = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 60, (SendOption) 1, -1);
      messageWriter.Write(ffgalnapkcd.get_PlayerId());
      ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter);
      ffgalnapkcd.MurderPlayer(ffgalnapkcd);
      return true;
    }
  }
}
