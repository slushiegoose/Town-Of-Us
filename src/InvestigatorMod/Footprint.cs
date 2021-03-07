// Decompiled with JetBrains decompiler
// Type: TownOfUs.InvestigatorMod.Footprint
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using System.Collections.Generic;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.InvestigatorMod
{
  public class Footprint
  {
    public static List<Footprint> AllPrints = new List<Footprint>();
    public Color Color;
    public Vector3 Position;
    private Vector2 _velocity;
    private GameObject _gameObject;
    private SpriteRenderer _spriteRenderer;
    public readonly FFGALNAPKCD Player;
    private float _time;

    public static float Duration => CustomGameOptions.FootprintDuration;

    public static void DestroyAll()
    {
      while ((uint) Footprint.AllPrints.Count > 0U)
        Footprint.AllPrints[0].Destroy();
    }

    public Footprint(FFGALNAPKCD player)
    {
      this.Position = ((Component) player).get_transform().get_position();
      this._velocity = ((Rigidbody2D) ((Component) player).get_gameObject().GetComponent<Rigidbody2D>()).get_velocity();
      this.Player = player;
      this._time = (float) (int) Time.get_time();
      this.Color = !CustomGameOptions.AnonymousFootPrint ? Color32.op_Implicit(((Il2CppArrayBase<Color32>) LOCPGOACAJF.get_OPKIKLENHFA()).get_Item((int) player.get_Data().get_EHAHBDFODKC())) : new Color(0.2f, 0.2f, 0.2f, 1f);
      this.Start();
    }

    private void Start()
    {
      this._gameObject = new GameObject(nameof (Footprint));
      this._gameObject.get_transform().set_position(this.Position);
      this._gameObject.get_transform().Rotate(Vector3.op_Multiply(Vector3.get_forward(), Vector2.SignedAngle(Vector2.get_up(), this._velocity)));
      this._gameObject.get_transform().SetParent(((Component) this.Player).get_transform().get_parent());
      this._spriteRenderer = (SpriteRenderer) this._gameObject.AddComponent<SpriteRenderer>();
      this._spriteRenderer.set_sprite(TownOfUs.TownOfUs.Footprint);
      this._spriteRenderer.set_color(this.Color);
      this._spriteRenderer.set_size(Vector2.op_Multiply(new Vector2(1.2f, 1f), CustomGameOptions.FootprintSize / 10f));
      this._gameObject.SetActive(true);
      Footprint.AllPrints.Add(this);
    }

    private void Destroy()
    {
      Object.Destroy((Object) this._gameObject);
      Footprint.AllPrints.Remove(this);
    }

    public bool Update()
    {
      float time = Time.get_time();
      float num = Mathf.Max((float) (1.0 - ((double) time - (double) this._time) / (double) Footprint.Duration), 0.0f);
      if ((double) num < 0.0 || (double) num > 1.0)
        num = 0.0f;
      if (RainbowUtils.IsRainbow((int) this.Player.get_Data().get_EHAHBDFODKC()) & !CustomGameOptions.AnonymousFootPrint)
        this.Color = RainbowUtils.Rainbow;
      this.Color = new Color((float) this.Color.r, (float) this.Color.g, (float) this.Color.b, num);
      this._spriteRenderer.set_color(this.Color);
      if ((double) this._time + (double) (int) Footprint.Duration >= (double) time)
        return false;
      this.Destroy();
      return true;
    }
  }
}
