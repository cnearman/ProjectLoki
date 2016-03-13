using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ProjectLoki.Weapons
{
    class MeleeWeapon : IWeapon
    {

        public IProjectile Projectile { get; set; }
        public FireDistribution Distribution { get; set; }

        public float CurrentCooldown { get; set; }
        public float FireRate { get; set; }

        public float CurrentReloadTime { get; set; }
        public float ReloadSpeed { get; set; }


        public WeaponState State { get; private set; }

        private bool IsIdle
        {
            get
            {
                return this.State == WeaponState.Idle;
            }
        }

        public bool CanAttack
        {
            get
            {
                return this.CurrentCooldown == 0 && this.IsIdle;
            }
        }

        public void Activate(Vector3 position, Vector3 rotation, double timeTriggered)
        {
            throw new NotImplementedException();
        }

        [PunRPC]
        public void DisplayAnimation(Vector3 position, Vector3 rotation)
        {
            throw new NotImplementedException();
        }

        public void ReduceCooldowns(float delta)
        {
            this.CurrentCooldown = Mathf.Clamp(this.CurrentCooldown - delta, 0, float.MaxValue);
        }

        [PunRPC]
        public void SendResultToClients()
        {
            throw new NotImplementedException();
        }
    }
}
