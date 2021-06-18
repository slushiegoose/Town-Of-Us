using UnityEngine;

namespace TownOfUs.CrewmateRoles.TimeLordMod
{
    public class PointInTime
    {
        public Vector3 position;
        public float unix;
        public Vector2 velocity;

        public PointInTime(Vector3 position, Vector2 velocity, float unix)
        {
            this.position = position;
            this.velocity = velocity;
            this.unix = unix;
        }
    }
}