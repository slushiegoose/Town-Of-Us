using UnityEngine;

namespace TownOfUs.TimeLordMod
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