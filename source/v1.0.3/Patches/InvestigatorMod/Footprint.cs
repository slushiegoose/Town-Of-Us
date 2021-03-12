using System.Collections.Generic;
using UnityEngine;

namespace TownOfUs.InvestigatorMod
{
    public class Footprint
    {
        public static List<Footprint> AllPrints = new List<Footprint>();
        public static float Duration => CustomGameOptions.FootprintDuration;

        public Color Color;
        public Vector3 Position;
        private Vector2 _velocity;
        private GameObject _gameObject;
        private SpriteRenderer _spriteRenderer;
        public readonly PlayerControl Player;
        private float _time;

        public static void DestroyAll()
        {
            while (AllPrints.Count != 0)
            {
                AllPrints[0].Destroy();
            }
        }

        public Footprint(PlayerControl player)
        {
            Position = player.transform.position;
            _velocity = player.gameObject.GetComponent<Rigidbody2D>().velocity;
            
            Player = player;
            _time = (int)Time.time;

            if (CustomGameOptions.AnonymousFootPrint)
            {
                Color = new Color(0.2f, 0.2f, 0.2f, 1f);
            }
            else
            {
                Color = Palette.PlayerColors[player.Data.ColorId];
            }

            Start();
        }


        private void Start()
        {
            _gameObject = new GameObject("Footprint");
            _gameObject.transform.position = Position;
            _gameObject.transform.Rotate(Vector3.forward * Vector2.SignedAngle(Vector2.up, _velocity));
            _gameObject.transform.SetParent(Player.transform.parent);

            _spriteRenderer = _gameObject.AddComponent<SpriteRenderer>();
            _spriteRenderer.sprite = TownOfUs.Footprint;
            _spriteRenderer.color = Color;
            _spriteRenderer.size = new Vector2(1.2f, 1f) * (CustomGameOptions.FootprintSize / 10);
            

            _gameObject.SetActive(true);
            AllPrints.Add(this);
        }

        private void Destroy()
        {
            Object.Destroy(_gameObject);
            AllPrints.Remove(this);
        }
        
        public bool Update() {
            var currentTime = Time.time;
            var alpha = Mathf.Max((1f - ((currentTime - _time) / Duration)), 0f);
            
            if (alpha < 0 || alpha > 1)
                alpha = 0;

            if (RainbowUtils.IsRainbow(Player.Data.ColorId) & !CustomGameOptions.AnonymousFootPrint)
            {
                Color = RainbowUtils.Rainbow;
            }
            
            Color = new Color(Color.r, Color.g, Color.b, alpha);
            _spriteRenderer.color = Color;

            if (_time + (int) Duration < currentTime)
            {
                Destroy();
                return true;
            }

            return false;
        }

    }
}