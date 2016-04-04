using UnityEngine;
using System.Collections;
using System;

namespace ProjectLoki.Weapons
{
    public class Gun :BaseClass, IWeapon, IReloadable
    {
        public string Name = "Gun";
        public GameObject tracer;
        public GameObject spray;


        public Gun()
        {
            this.tracer = (GameObject) Resources.Load("Tracer");
            this.spray = (GameObject)Resources.Load("DirtSpray");

            this.Projectile = new DefaultProjectile();
            this.Distribution = new DefaultDistribution(0.0f);
            this.FireRate = 0.2f;
            this.ClipSize = 10;
            this.ReloadSpeed = 1.5f;
            this.Range = 200;

            this.CurrentCooldown = 0;
            this.CurrentAmmo = this.ClipSize;
            this.CurrentReloadTime = 0;
            this.State = WeaponState.Idle;
        }

        public IProjectile Projectile { get;  set; }
        public FireDistribution Distribution { get;  set; }

        public float CurrentCooldown { get; set; }
        public float FireRate { get;  set; }

        public int CurrentAmmo { get; set; }
        public int ClipSize { get;  set; }

        public float CurrentReloadTime { get;  set; }
        public float ReloadSpeed { get;  set; }

        public WeaponState State;

        public float Range { get;  set; }

        private bool IsReloading
        {
            get
            {
                return this.State == WeaponState.Reloading;
            }
        }

        private bool IsIdle
        {
            get
            {
                return this.State == WeaponState.Idle;
            }
        }


        public bool CanFire
        {
            get
            {
                return this.CurrentAmmo > 0 && this.CurrentCooldown <= 0 && this.IsIdle;
            }
        }

        public bool CanReload
        {
            get
            {
                return this.CurrentAmmo < this.ClipSize && this.IsIdle;
            }
        }

        public bool Activate(Vector3 position, Vector3 rotation, double timeTriggered)
        {
            if(CanFire)
            {
                this.State = WeaponState.Firing;
                float accuracy = 1.0f;

                foreach (Fire bulletSetting in this.Distribution.GetDistribution(accuracy))
                {
                    RaycastHit hit = new RaycastHit(); 
                    if (Physics.Raycast(position + bulletSetting.Position , (rotation + bulletSetting.Rotation), out hit, this.Range))
                    {
                        this.Projectile.ApplyEffects(hit, (rotation + bulletSetting.Rotation));
                    } 
                }

                this.CurrentAmmo -= 1;
                //this.CurrentCooldown = (float)((FireRate *0.0d) - (PhotonNetwork.time - timeTriggered));
                this.State = WeaponState.Idle;
                return true; // Was fired
            }

            return false; //was not fired.
        }

        [PunRPC]
        public void SendResultToClients()
        {

        }

        public void DisplayAnimation(Vector3 position, Vector3 rotation)
        {
            if (CanFire)
            {
                float accuracy = 1.0f;
                foreach (Fire bulletSetting in this.Distribution.GetDistribution(accuracy))
                {
                    GameObject tempTracer = (GameObject)Instantiate(tracer, Vector3.zero, Quaternion.identity);

                    RaycastHit hit = new RaycastHit();
                    if (Physics.Raycast(position + bulletSetting.Position, (rotation + bulletSetting.Rotation), out hit, this.Range))
                    {
                        tempTracer.GetComponent<Tracer>().SetLocation(position + bulletSetting.Position, hit.point);
                        GameObject tempSpray = (GameObject)Instantiate(spray, hit.point, Quaternion.identity);
                        tempSpray.transform.forward = hit.normal;
                        Debug.Log(hit.point);
                    }
                    else
                    {
                        Vector3 endpoint = (position + bulletSetting.Position) + (rotation + bulletSetting.Rotation) * this.Range;
                        tempTracer.GetComponent<Tracer>().SetLocation(position + bulletSetting.Position, endpoint);
                    }
                }
            }
        }

        public void Reload()
        {
            if(CanReload)
            {
                this.State = WeaponState.Reloading;
                // We will need to put some start time here dealio while we run the reload animation.
                // then do this.
                this.CurrentAmmo = ClipSize;
                this.State = WeaponState.Idle;
            }
        }

        public void InterruptReload()
        {
            // We will need to put some way to break out of the reloading once we implement the 
            // reloading of one bullet at a time.
            // Note: may be gun specific?
            this.State = WeaponState.Idle;
        }

        public void ReduceCooldowns(float delta)
        {
            this.CurrentCooldown = Mathf.Clamp(this.CurrentCooldown - delta, 0, float.MaxValue);
            this.CurrentReloadTime = Mathf.Clamp(this.CurrentReloadTime - delta, 0, float.MaxValue);
        }
    }
}