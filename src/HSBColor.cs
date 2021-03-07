// Decompiled with JetBrains decompiler
// Type: HSBColor
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using Il2CppSystem;
using System;
using UnityEngine;

[Serializable]
public struct HSBColor
{
  public float h;
  public float s;
  public float b;
  public float a;

  public HSBColor(float h, float s, float b, float a)
  {
    this.h = h;
    this.s = s;
    this.b = b;
    this.a = a;
  }

  public HSBColor(float h, float s, float b)
  {
    this.h = h;
    this.s = s;
    this.b = b;
    this.a = 1f;
  }

  public HSBColor(Color col)
  {
    HSBColor hsbColor = HSBColor.FromColor(col);
    this.h = hsbColor.h;
    this.s = hsbColor.s;
    this.b = hsbColor.b;
    this.a = hsbColor.a;
  }

  public static HSBColor FromColor(Color color)
  {
    HSBColor hsbColor = new HSBColor(0.0f, 0.0f, 0.0f, (float) color.a);
    float r = (float) color.r;
    float g = (float) color.g;
    float b = (float) color.b;
    float num1 = Mathf.Max(r, Mathf.Max(g, b));
    if ((double) num1 <= 0.0)
      return hsbColor;
    float num2 = Mathf.Min(r, Mathf.Min(g, b));
    float num3 = num1 - num2;
    if ((double) num1 > (double) num2)
    {
      hsbColor.h = (double) g != (double) num1 ? ((double) b != (double) num1 ? ((double) b <= (double) g ? (float) (((double) g - (double) b) / (double) num3 * 60.0) : (float) (((double) g - (double) b) / (double) num3 * 60.0 + 360.0)) : (float) (((double) r - (double) g) / (double) num3 * 60.0 + 240.0)) : (float) (((double) b - (double) r) / (double) num3 * 60.0 + 120.0);
      if ((double) hsbColor.h < 0.0)
        hsbColor.h += 360f;
    }
    else
      hsbColor.h = 0.0f;
    hsbColor.h *= 1f / 360f;
    hsbColor.s = (float) ((double) num3 / (double) num1 * 1.0);
    hsbColor.b = num1;
    return hsbColor;
  }

  public static Color ToColor(HSBColor hsbColor)
  {
    float num1 = hsbColor.b;
    float num2 = hsbColor.b;
    float num3 = hsbColor.b;
    if ((double) hsbColor.s != 0.0)
    {
      float b = hsbColor.b;
      float num4 = hsbColor.b * hsbColor.s;
      float num5 = hsbColor.b - num4;
      float num6 = hsbColor.h * 360f;
      if ((double) num6 < 60.0)
      {
        num1 = b;
        num2 = (float) ((double) num6 * (double) num4 / 60.0) + num5;
        num3 = num5;
      }
      else if ((double) num6 < 120.0)
      {
        num1 = (float) (-((double) num6 - 120.0) * (double) num4 / 60.0) + num5;
        num2 = b;
        num3 = num5;
      }
      else if ((double) num6 < 180.0)
      {
        num1 = num5;
        num2 = b;
        num3 = (float) (((double) num6 - 120.0) * (double) num4 / 60.0) + num5;
      }
      else if ((double) num6 < 240.0)
      {
        num1 = num5;
        num2 = (float) (-((double) num6 - 240.0) * (double) num4 / 60.0) + num5;
        num3 = b;
      }
      else if ((double) num6 < 300.0)
      {
        num1 = (float) (((double) num6 - 240.0) * (double) num4 / 60.0) + num5;
        num2 = num5;
        num3 = b;
      }
      else if ((double) num6 <= 360.0)
      {
        num1 = b;
        num2 = num5;
        num3 = (float) (-((double) num6 - 360.0) * (double) num4 / 60.0) + num5;
      }
      else
      {
        num1 = 0.0f;
        num2 = 0.0f;
        num3 = 0.0f;
      }
    }
    return new Color(Mathf.Clamp01(num1), Mathf.Clamp01(num2), Mathf.Clamp01(num3), hsbColor.a);
  }

  public Color ToColor() => HSBColor.ToColor(this);

  public override string ToString() => "H:" + this.h.ToString() + " S:" + this.s.ToString() + " B:" + this.b.ToString();

  public static HSBColor Lerp(HSBColor a, HSBColor b, float t)
  {
    float h;
    float s;
    if ((double) a.b == 0.0)
    {
      h = b.h;
      s = b.s;
    }
    else if ((double) b.b == 0.0)
    {
      h = a.h;
      s = a.s;
    }
    else
    {
      if ((double) a.s == 0.0)
        h = b.h;
      else if ((double) b.s == 0.0)
      {
        h = a.h;
      }
      else
      {
        float num = Mathf.LerpAngle(a.h * 360f, b.h * 360f, t);
        while ((double) num < 0.0)
          num += 360f;
        while ((double) num > 360.0)
          num -= 360f;
        h = num / 360f;
      }
      s = Mathf.Lerp(a.s, b.s, t);
    }
    return new HSBColor(h, s, Mathf.Lerp(a.b, b.b, t), Mathf.Lerp(a.a, b.a, t));
  }

  public static void Test()
  {
    HSBColor hsbColor = new HSBColor(Color.get_red());
    Debug.Log(Object.op_Implicit("red: " + hsbColor.ToString()));
    hsbColor = new HSBColor(Color.get_green());
    Debug.Log(Object.op_Implicit("green: " + hsbColor.ToString()));
    hsbColor = new HSBColor(Color.get_blue());
    Debug.Log(Object.op_Implicit("blue: " + hsbColor.ToString()));
    hsbColor = new HSBColor(Color.get_grey());
    Debug.Log(Object.op_Implicit("grey: " + hsbColor.ToString()));
    hsbColor = new HSBColor(Color.get_white());
    Debug.Log(Object.op_Implicit("white: " + hsbColor.ToString()));
    hsbColor = new HSBColor(new Color(0.4f, 1f, 0.84f, 1f));
    Debug.Log(Object.op_Implicit("0.4, 1f, 0.84: " + hsbColor.ToString()));
    Debug.Log(Object.op_Implicit("164,82,84   .... 0.643137f, 0.321568f, 0.329411f  :" + HSBColor.ToColor(new HSBColor(new Color(0.643137f, 0.321568f, 0.329411f))).ToString()));
  }
}
