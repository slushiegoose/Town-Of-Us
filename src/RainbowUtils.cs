// Decompiled with JetBrains decompiler
// Type: RainbowUtils
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using UnhollowerBaseLib;
using UnityEngine;

public class RainbowUtils
{
  private static readonly int BackColor = Shader.PropertyToID("_BackColor");
  private static readonly int BodyColor = Shader.PropertyToID("_BodyColor");
  private static readonly int VisorColor = Shader.PropertyToID("_VisorColor");

  public static Color Rainbow => new HSBColor(RainbowUtils.PP(0.0f, 1f, 0.8f), 1f, 1f).ToColor();

  public static Color RainbowShadow => RainbowUtils.Shadow(RainbowUtils.Rainbow);

  public static float PP(float min, float max, float mul) => min + Mathf.PingPong(Time.get_time() * mul, max - min);

  public static Color Shadow(Color color) => new Color((float) (color.r - 0.300000011920929), (float) (color.g - 0.300000011920929), (float) (color.b - 0.300000011920929));

  public static void SetRainbow(Renderer rend)
  {
    rend.get_material().SetColor(RainbowUtils.BackColor, RainbowUtils.RainbowShadow);
    rend.get_material().SetColor(RainbowUtils.BodyColor, RainbowUtils.Rainbow);
    rend.get_material().SetColor(RainbowUtils.VisorColor, Color32.op_Implicit(LOCPGOACAJF.get_FNILJLIFOKF()));
  }

  public static bool IsRainbow(int id) => ((Il2CppArrayBase<string>) LOCPGOACAJF.get_OKIPHGGAPMH()).get_Item(id) == "RNBOW";
}
