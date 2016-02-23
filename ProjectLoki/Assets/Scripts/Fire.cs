using UnityEngine;

namespace ProjectLoki.Weapons
{
    public class Fire
    {
        public Fire() { }

        public Fire(Vector3 pos, Vector3 rot)
        {
            this.Position = pos;
            this.Rotation = rot;
        }

        public Vector3 Position { get; set; }

        public Vector3 Rotation { get; set; }
    }
}