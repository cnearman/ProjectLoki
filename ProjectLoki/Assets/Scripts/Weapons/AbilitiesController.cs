﻿using UnityEngine;

namespace ProjectLoki.Weapons
{
    public class AbilitiesController : BaseClass
    {
        private PhotonView m_PhotonView;

        void Awake()
        {
            m_PhotonView = GetComponent<PhotonView>();
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

        public void FireWeapon(Vector3 position)
        {
            this.CurrentWeapon.Activate(position);
        }

        public void SecondaryAction()
        {
            IReloadable reloadingWeapon = this.CurrentWeapon as IReloadable;
            if (reloadingWeapon != null)
            {
                reloadingWeapon.Reload();
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
