// Decompiled with JetBrains decompiler
// Type: TownOfUs.RainbowMod.PalettePatch
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.RainbowMod
{
  public static class PalettePatch
  {
    public static void Load()
    {
      string[] strArray1 = new string[19]
      {
        "RED",
        "BLUE",
        "GRN",
        "PINK",
        "ORNG",
        "YLOW",
        "BLAK",
        "WHTE",
        "PURP",
        "BRWN",
        "CYAN",
        "LIME",
        "MELON",
        "CHOCO",
        "LTBLUE",
        "BEIGE",
        "LTPINK",
        "TURQ",
        "RNBOW"
      };
      Color32[] color32Array1 = new Color32[19]
      {
        new Color32((byte) 198, (byte) 17, (byte) 17, byte.MaxValue),
        new Color32((byte) 19, (byte) 46, (byte) 210, byte.MaxValue),
        new Color32((byte) 17, (byte) 128, (byte) 45, byte.MaxValue),
        new Color32((byte) 238, (byte) 84, (byte) 187, byte.MaxValue),
        new Color32((byte) 240, (byte) 125, (byte) 13, byte.MaxValue),
        new Color32((byte) 246, (byte) 246, (byte) 87, byte.MaxValue),
        new Color32((byte) 63, (byte) 71, (byte) 78, byte.MaxValue),
        new Color32((byte) 215, (byte) 225, (byte) 241, byte.MaxValue),
        new Color32((byte) 107, (byte) 47, (byte) 188, byte.MaxValue),
        new Color32((byte) 113, (byte) 73, (byte) 30, byte.MaxValue),
        new Color32((byte) 56, byte.MaxValue, (byte) 221, byte.MaxValue),
        new Color32((byte) 80, (byte) 240, (byte) 57, byte.MaxValue),
        new Color32((byte) 168, (byte) 50, (byte) 62, byte.MaxValue),
        new Color32((byte) 60, (byte) 48, (byte) 44, byte.MaxValue),
        new Color32((byte) 61, (byte) 129, byte.MaxValue, byte.MaxValue),
        new Color32((byte) 240, (byte) 211, (byte) 165, byte.MaxValue),
        new Color32((byte) 236, (byte) 61, byte.MaxValue, byte.MaxValue),
        new Color32((byte) 61, byte.MaxValue, (byte) 181, byte.MaxValue),
        new Color32((byte) 0, (byte) 0, (byte) 0, byte.MaxValue)
      };
      Color32[] color32Array2 = new Color32[19]
      {
        new Color32((byte) 122, (byte) 8, (byte) 56, byte.MaxValue),
        new Color32((byte) 9, (byte) 21, (byte) 142, byte.MaxValue),
        new Color32((byte) 10, (byte) 77, (byte) 46, byte.MaxValue),
        new Color32((byte) 172, (byte) 43, (byte) 174, byte.MaxValue),
        new Color32((byte) 180, (byte) 62, (byte) 21, byte.MaxValue),
        new Color32((byte) 195, (byte) 136, (byte) 34, byte.MaxValue),
        new Color32((byte) 30, (byte) 31, (byte) 38, byte.MaxValue),
        new Color32((byte) 132, (byte) 149, (byte) 192, byte.MaxValue),
        new Color32((byte) 59, (byte) 23, (byte) 124, byte.MaxValue),
        new Color32((byte) 94, (byte) 38, (byte) 21, byte.MaxValue),
        new Color32((byte) 36, (byte) 169, (byte) 191, byte.MaxValue),
        new Color32((byte) 21, (byte) 168, (byte) 66, byte.MaxValue),
        new Color32((byte) 168, (byte) 50, (byte) 62, byte.MaxValue),
        new Color32((byte) 60, (byte) 48, (byte) 44, byte.MaxValue),
        new Color32((byte) 61, (byte) 129, byte.MaxValue, byte.MaxValue),
        new Color32((byte) 240, (byte) 211, (byte) 165, byte.MaxValue),
        new Color32((byte) 236, (byte) 61, byte.MaxValue, byte.MaxValue),
        new Color32((byte) 61, byte.MaxValue, (byte) 181, byte.MaxValue),
        new Color32((byte) 0, (byte) 0, (byte) 0, byte.MaxValue)
      };
      string[] strArray2 = new string[19]
      {
        "Red",
        "Blue",
        "Green",
        "Pink",
        "Orange",
        "Yellow",
        "Black",
        "White",
        "Purple",
        "Brown",
        "Cyan",
        "Lime",
        "Watermelon",
        "Chocolate",
        "Sky Blue",
        "Beige",
        "Hot Pink",
        "Turquoise",
        "Rainbow"
      };
      LOCPGOACAJF.set_OKIPHGGAPMH(Il2CppStringArray.op_Implicit(strArray1));
      LOCPGOACAJF.set_OPKIKLENHFA(Il2CppStructArray<Color32>.op_Implicit(color32Array1));
      LOCPGOACAJF.set_KBMIDEGKPLP(Il2CppStructArray<Color32>.op_Implicit(color32Array2));
      BFAIBLAAGEI.set_JLLPDGFDEDF(Il2CppStringArray.op_Implicit(strArray2));
    }
  }
}
