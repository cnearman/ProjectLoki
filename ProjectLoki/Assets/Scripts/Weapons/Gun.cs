using UnityEngine;
using System.Collections;
using System;

namespace ProjectLoki.Weapons
{
    public class Gun : IWeapon
    {

        public Gun()
        {
            this.Projectile = new DefaultProjectile();
            this.Distribution = new DefaultDistribution(0.0f);
            this.FireRate = 0.2f;
            this.ClipSize = 10;
            this.ReloadSpeed = 1.5f;
            this.Range = 20;

            this.CurrentCooldown = 0;
            this.CurrentAmmo = this.ClipSize;
            this.CurrentReloadTime = 0;
            this.IsReloading = false;
        }

        public IProjectile Projectile { get; internal set; }
        public FireDistribution Distribution { get; internal set; }

        public float CurrentCooldown { get; internal set; }
        public float FireRate { get; internal set; }

        public int CurrentAmmo { get; internal set; }
        public int ClipSize { get; internal set; }

        public float CurrentReloadTime { get; internal set; }
        public float ReloadSpeed { get; internal set; }
        public bool IsReloading { get; internal set; }

        public float Range { get; internal set; }


        public bool CanFire
        {
            get
            {
                return !this.IsReloading && this.CurrentAmmo > 0 && this.CurrentCooldown == 0;
            }
        }

        public bool CanReload
        {
            get
            {
                return this.CurrentAmmo < this.ClipSize;
            }
        }

        public void Activate(Vector3 position)
        {
            if(this.CanFire)
            {
                float accuracy = 1.0f;

                foreach(Fire bulletSetting in this.Distribution.GetDistribution(accuracy))
                {
                    RaycastHit hit = new RaycastHit();
                    if (Physics.Raycast(bulletSetting.Position, bulletSetting.Rotation, out hit, this.Range))
                    {
                        this.Projectile.ApplyEffects(hit);
                    }
                }

                this.CurrentAmmo -= 1;
                this.CurrentCooldown = FireRate;
            }
        }

        public void Reload()
        {
            if(CanReload)
            {
                this.IsReloading = true;
                // We will need to put some start time here dealio while we run the reload animation.
                // then do this.
                this.CurrentAmmo = ClipSize;
                this.IsReloading = false;
            }
        }
    }
}