using UnityEngine;

namespace ProjectLoki.Weapons
{
    public class AbilitiesController : BaseClass
    {
        private PhotonView m_PhotonView;

        void Awake()
        {
            m_PhotonView = GetComponent<PhotonView>();
            this.PrimaryWeapon = new Gun();
            this.SecondaryWeapon = new Gun();
        }

        public WeaponSlot SelectedWeapon;

        public IWeapon PrimaryWeapon;
        public IWeapon SecondaryWeapon;

        public IWeapon CurrentWeapon
        {
            get
            {
                IWeapon result = null;
                if (this.SelectedWeapon == WeaponSlot.Primary)
                {
                    result = this.PrimaryWeapon;
                }
                else if (this.SelectedWeapon == WeaponSlot.Secondary)
                {
                    result = this.SecondaryWeapon;
                }

                return result;
            }
        }

        public void Update()
        {
            if(m_PhotonView.isMine)
            {
                PrimaryWeapon.ReduceCooldowns(Time.deltaTime);
                SecondaryWeapon.ReduceCooldowns(Time.deltaTime);
            }
        }


        public void FireWeapon(Vector3 position, Vector3 rotation)
        {
            this.m_PhotonView.RPC("FireWeaponOnServer", PhotonTargets.All, position, rotation);
            //this.CurrentWeapon.DisplayAnimation(position, rotation);
        }

        [PunRPC]
        public void FireWeaponOnServer(Vector3 position, Vector3 rotation)
        {
            this.CurrentWeapon.Activate(position, rotation);
        }

        public void SecondaryAction()
        {
            IReloadable reloadingWeapon = this.CurrentWeapon as IReloadable;
            if (reloadingWeapon != null)
            {
                reloadingWeapon.Reload();
            }
        }

        public void SwitchWeapon()
        {
            if (this.SelectedWeapon == WeaponSlot.Primary)
            {
                this.SelectedWeapon = WeaponSlot.Secondary;
            }
            else
            {
                this.SelectedWeapon = WeaponSlot.Primary;
            }
        }

    }

    public enum WeaponSlot
    {
        None = 0,
        Primary = 1,
        Secondary = 2
    }
}
