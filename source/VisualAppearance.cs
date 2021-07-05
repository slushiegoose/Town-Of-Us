using UnityEngine;

namespace TownOfUs
{
    public class VisualAppearance
    {
        public float SpeedFactor { get; set; } = 1.0f;
        public Vector3 SizeFactor { get; set; } = new Vector3(0.7f, 0.7f, 1.0f);

        public int ColorId { get; set; }
        public uint HatId { get; set; }
        public uint SkinId { get; set; }
        public uint PetId { get; set; }
    }
}
